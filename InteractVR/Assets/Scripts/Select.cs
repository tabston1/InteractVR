using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour
{

	private GameObject camera;
	private Rigidbody cameraRigidBod;
	private Transform cameraTrans;

	private GameObject selectedObject;
	private bool holdingObject;
	private bool canSelect;
	private float timer;
	private const float grabTime = 1.5f;

	[Tooltip ("Maximum gaze distance, in meters, for calculating a hit.")]
	public float MaxGazeDistance = 40.0f;

	[Tooltip ("Select the layers raycast should target.")]
	public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;

	public bool Hit { get; private set; }

	public RaycastHit HitInfo { get; private set; }

	public Ray HitRay { get; private set; }

	/// Position of the intersection of the user's gaze and the object in the scene.
	public Vector3 Position { get; private set; }

	/// RaycastHit Normal direction.
	public Vector3 Normal { get; private set; }

	public GameObject FocusedObject { get; private set; }

	public BasicObject objScript { get; private set; }

	private Vector3 gazeOrigin;
	private Vector3 gazeDirection;
	private Quaternion gazeRotation;
	private float lastHitDistance = 15.0f;
	private float distance;
	private float initialGrabDistance = 0f;
	private float previousDistanceMagnitude = Mathf.Infinity;
	private float distanceThreshold = 3.0f;

	private Vector3 lastCameraPosition;

	public GameObject controller;
	public Vector3 controllerOrigin;
	public Vector3 controllerDirection;
	public Quaternion controllerRotation;
	private float grabForce = 750f;

	private LineRenderer linePointer;
	public float axisSpeed;

	private GameObject manager;
	private Manager managerScript;

	private float speed = 30.0f;
	private float maxSpeed = 50.0f;
	private float maxDistance = 5.0f;

	// Finds the description child of an object and makes the description appear
	void OnSelect ()
	{
		if (selectedObject != null) {
			Deselect ();
		}

		//Ensure the object's rigidbody is not kinematic at this point
		if (HitInfo.rigidbody) {
			HitInfo.rigidbody.isKinematic = false;
		}

		selectedObject = HitInfo.transform.gameObject;

		//Ignore walls
		if (selectedObject.tag == "Wall")
			return;

		//Enable the description for the object
		if (selectedObject.tag == "Button")
			selectedObject.BroadcastMessage ("onClick");
		else {
			try {
				selectedObject.BroadcastMessage ("onSelect");
			} catch (Exception e) {
				selectedObject = null;
			}
		}
	}

	//Disables an active description for a child, making it disappear.
	void Deselect ()
	{
		if (selectedObject == null)
			return;
		foreach (Transform child in selectedObject.transform) {
			if (child.CompareTag ("Description")) {
				child.gameObject.SetActive (false);
			}
		}
		selectedObject = null;
	}


	private void UpdateRaycast ()
	{
		// Get the raycast hit information from Unity's physics system.
		RaycastHit hitInfo;
		GameObject oldFocusedObject = FocusedObject;

		//Hit = Physics.Raycast(gazeOrigin, gazeDirection, out hitInfo, MaxGazeDistance, RaycastLayerMask);
		Hit = Physics.Raycast (controllerOrigin, controllerDirection, out hitInfo, MaxGazeDistance, RaycastLayerMask);

		// Update the HitInfo property so other classes can use this hit information.
		HitInfo = hitInfo;

		if (Hit) {
			// If the raycast hits a hologram, set the position and normal to match the intersection point.
			Position = hitInfo.point;
			Normal = hitInfo.normal;
			lastHitDistance = hitInfo.distance;
			FocusedObject = hitInfo.collider.gameObject;
		} else {
			// If the raycast does not hit a hologram, default the position to last hit distance in front of the user,
			// and the normal to face the user.
			//Position = gazeOrigin + (gazeDirection * lastHitDistance);
			Position = controllerOrigin + (controllerDirection * lastHitDistance);
			//Normal = -gazeDirection;
			Normal = -controllerDirection;
			FocusedObject = null;
		}

		// Check if the currently hit object has changed
		if (oldFocusedObject != FocusedObject) {
			if (oldFocusedObject != null) {
				OnGazeLeave (oldFocusedObject);
			}
			if (FocusedObject != null) {
				OnGazeEnter (FocusedObject);
			}
		}
	}

	private void OnGazeEnter (GameObject FocusedObject)
	{
		if (FocusedObject.tag == "Button")
			FocusedObject.GetComponent<Button> ().Select ();
 
		if (!(FocusedObject.tag == "UI" || FocusedObject.tag == "Wall"))
			lineColor (Color.green, Color.green);  
	}

	private void OnGazeLeave (GameObject OldFocusedObject)
	{
		if (OldFocusedObject.tag == "Button")
			EventSystem.current.SetSelectedGameObject (null);

		lineColor (Color.red, Color.red);  
	}

	void Grab ()
	{
		//Don't allow a user to grab an object if a transformation tool is active for another object
		if (Manager.activeTransformGizmo)
			return;
			
		if (HitInfo.transform.gameObject.tag == "Movable") {
			holdingObject = true;

			//Also toggle beingHeld bool in the specific object's Basic Object script
			//BasicObject objScript = HitInfo.transform.gameObject.GetComponent<BasicObject> ();
			if (objScript == null) {
				objScript = HitInfo.transform.gameObject.GetComponent<BasicObject> ();
			}

			if (objScript != null) {
				objScript.beingHeld = true;
				objScript.enableMotion ();

				//objScript.disableGravity();
			}

			distance = Vector3.Distance (controller.transform.position, HitInfo.transform.position);

			//Save the initial grab distance if the object was just picked up
			if (initialGrabDistance == 0f) {
				initialGrabDistance = distance;
			}


			lineColor (Color.blue, Color.blue);
		}
	}

	void Drop ()
	{
		if (HitInfo.rigidbody) {
			HitInfo.rigidbody.velocity = Vector3.zero;
		}

		holdingObject = false;

		//Also toggle beingHeld bool in the specific object's Basic Object script
		//BasicObject objScript = HitInfo.transform.gameObject.GetComponent<BasicObject> ();
		if (objScript != null) {
			objScript.beingHeld = false;
			if (!objScript.billboard.activeSelf) {
				StartCoroutine (objScript.DelayedObjectMotionFreeze ());
			}
			objScript = null;
		}

		//Reset the initial grab distance to prepare for the next object to be picked up
		initialGrabDistance = 0f;

		canSelect = false;
		lineColor (Color.green, Color.green);
	}

	void GetInputs ()
	{
		//Checks for holding an object
		if (Input.GetButton ("Jump") || managerScript.use) {
			if (Hit) {
				timer += Time.deltaTime;
				if (timer >= grabTime) {
					Grab ();
				}
			}
		}

		//Used to make the information about an object pop up
		if (Input.GetButtonUp ("Jump") || managerScript.useUp) {
			if (Hit) {
				if (!holdingObject && canSelect)
					OnSelect ();
			} else {
				if (!holdingObject)
					Deselect ();
			}

			if (holdingObject)
				Drop ();
			canSelect = true;
			timer = 0f;
		}

		// Control the direction the controller faces
		var xRotation = Input.GetAxis ("Vertical") * axisSpeed;
		var yRotation = Input.GetAxis ("Horizontal") * axisSpeed;

		if (xRotation != 0.0 || yRotation != 0.0) {
			transform.Rotate (xRotation, yRotation, 0);
			transform.eulerAngles = new Vector3 (transform.eulerAngles.x, transform.eulerAngles.y, 0f);
		}

	}

	void updateLine ()
	{
		// Updates the laser from the controller
		linePointer = controller.GetComponent<LineRenderer> ();

		var points = new Vector3[2];

		points [0] = transform.position;

		if (Hit)
			points [1] = transform.position + lastHitDistance * transform.forward;
		else
			points [1] = transform.position + MaxGazeDistance * transform.forward;

		linePointer.SetPositions (points);
	}

	public void lineColor (Color start, Color end)
	{
		linePointer.startColor = start;
		linePointer.endColor = end;
	}

	// Use this for initialization
	void Start ()
	{
		controller = GameObject.FindGameObjectWithTag ("Controller");
		camera = GameObject.FindGameObjectWithTag ("MainCamera");
		cameraRigidBod = camera.GetComponent<Rigidbody> ();
		lastCameraPosition = camera.transform.position;

		selectedObject = null;
		timer = 0f;
		holdingObject = false;
		canSelect = true;

		manager = GameObject.Find ("Manager");
		managerScript = manager.GetComponent<Manager> ();
	}

	void Update ()
	{
		// Update controller information
		controllerOrigin = controller.transform.position;
		controllerDirection = controller.transform.forward;
		controllerRotation = controller.transform.rotation;

		// Grab functionality
		if (!holdingObject)
			UpdateRaycast ();

		
		lastCameraPosition = camera.transform.position;

		// Get inputs from controller
		GetInputs ();
	}

	void LateUpdate ()
	{
		// Update controller's laser
		updateLine ();
	}

	void FixedUpdate ()
	{
		if (holdingObject) {
			if (HitInfo.rigidbody) {
				HitRay = new Ray (controllerOrigin, controllerDirection);
				Vector3 laserEndPoint = HitRay.GetPoint (initialGrabDistance);
				Vector3 directionVector = laserEndPoint - HitInfo.transform.position;
				float offsetDistance = Vector3.Distance (laserEndPoint, HitInfo.transform.position);


				//Counteract gravitational force each frame if gravity is currently influencing the object
				if (HitInfo.rigidbody.useGravity) {
					HitInfo.rigidbody.AddForce (-Physics.gravity, ForceMode.Acceleration);
				}

				//float currentDistanceMagnitude = (HitRay.GetPoint (initialGrabDistance) - HitInfo.transform.position).sqrMagnitude;
				Vector3 newVelocity = directionVector.normalized * speed;
				if (newVelocity.magnitude > maxSpeed) {
					Debug.Log ("Reducing object velocity");
					newVelocity *= maxSpeed / newVelocity.magnitude;
				}
					
				//Dampen velocity changes when the object is close to the end of the laser pointer
				if (offsetDistance < (distanceThreshold / 75f)) {
					HitInfo.rigidbody.velocity = Vector3.zero;
				} else if (offsetDistance < (distanceThreshold / 25f)) {
					HitInfo.rigidbody.velocity = newVelocity * 0.05f;
				} else if (offsetDistance < (distanceThreshold / 10f)) {
					HitInfo.rigidbody.velocity = newVelocity * 0.2f;
				} else if (offsetDistance < (distanceThreshold / 5f)) {
					HitInfo.rigidbody.velocity = newVelocity * 0.6f;
				} else if (offsetDistance < (distanceThreshold)) {
					HitInfo.rigidbody.velocity = newVelocity * 0.8f;
				} else {
					HitInfo.rigidbody.velocity = newVelocity;
				}
			} else {
				HitInfo.transform.position = (controllerOrigin + (distance * controllerDirection));
			}
		}
	}
}
