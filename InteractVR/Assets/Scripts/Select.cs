using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Select : MonoBehaviour
{

	private GameObject camera;
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

	private Vector3 gazeOrigin;
	private Vector3 gazeDirection;
	private Quaternion gazeRotation;
	private float lastHitDistance = 15.0f;
	private float distance;

	public GameObject controller;
	public Vector3 controllerOrigin;
	public Vector3 controllerDirection;
	public Quaternion controllerRotation;

	private LineRenderer linePointer;
	public float axisSpeed;

	private GameObject manager;
	private Manager managerScript;

	private float speed = 5.0f;
	private float maxSpeed = 15.0f;

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

		//Enable the description for the object
		if (selectedObject.tag == "Button")
			selectedObject.BroadcastMessage ("onClick");
		else
			selectedObject.BroadcastMessage ("onSelect");
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
		if (HitInfo.transform.gameObject.tag == "Movable") {

			//Ensure the object's rigidbody is not kinematic
			if (HitInfo.rigidbody) {
				HitInfo.rigidbody.isKinematic = false;
			}


			holdingObject = true;

			//Also toggle beingHeld bool in the specific object's Basic Object script
			BasicObject objScript = HitInfo.transform.gameObject.GetComponent<BasicObject> ();
			if (objScript != null) {
				objScript.beingHeld = true;
				//objScript.disableGravity ();
				//objScript.enableKinematic();
			}

			distance = Vector3.Distance (controller.transform.position, HitInfo.transform.position);
			lineColor (Color.blue, Color.blue);
		}
	}

	void Drop ()
	{
		/*
		if (HitInfo.rigidbody) {
			HitInfo.rigidbody.velocity = Vector3.zero;
			if (!objScript.gravityOn)
				HitInfo.rigidbody.isKinematic = true;
		}
		*/

		if (HitInfo.rigidbody) {
			HitInfo.rigidbody.velocity = Vector3.zero;
		}

		holdingObject = false;

		//Also toggle beingHeld bool in the specific object's Basic Object script
		BasicObject objScript = HitInfo.transform.gameObject.GetComponent<BasicObject> ();
		if (objScript != null) {
			objScript.beingHeld = false;
			if (!objScript.billboard.activeSelf) {
				//objScript.disableKinematic();
			}
			//if (!objScript.billboard.activeSelf)
			//objScript.enableGravity ();
		}


		/*
		if (objScript != null)
			objScript.beingHeld = false;
		*/

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
		//camera = GameObject.FindGameObjectWithTag("MainCamera");
		//cameraTrans = camera.GetComponent<Transform>();

		controller = GameObject.FindGameObjectWithTag ("Controller");

		selectedObject = null;
		timer = 0f;
		holdingObject = false;
		canSelect = true;

		manager = GameObject.Find ("Manager");
		managerScript = manager.GetComponent<Manager> ();
	}

	void Update ()
	{
		//gazeOrigin = Camera.main.transform.position;
		//gazeDirection = Camera.main.transform.forward;
		//gazeRotation = Camera.main.transform.rotation;

		// Update controller information
		controllerOrigin = controller.transform.position;
		controllerDirection = controller.transform.forward;
		controllerRotation = controller.transform.rotation;

		/*
		// Grab functionality
		if (!holdingObject)
			UpdateRaycast ();
		else
			HitInfo.transform.position = (controllerOrigin + (distance * controllerDirection));
		*/

		// Grab functionality
		if (!holdingObject)
			UpdateRaycast ();
		else {
			if (HitInfo.rigidbody) {
				HitRay = new Ray (controllerOrigin, controllerDirection);
				Vector3 velocity = (HitRay.GetPoint (distance) - HitInfo.transform.position) * speed;

				if (velocity.magnitude > maxSpeed)
					velocity *= maxSpeed / velocity.magnitude;

				HitInfo.rigidbody.velocity = velocity;
			} else {
				HitInfo.transform.position = (controllerOrigin + (distance * controllerDirection));
			}
		}
		


		// Get inputs from controller
		GetInputs ();
	}

	void LateUpdate ()
	{
		// Update controller's laser
		updateLine ();
	}
}
