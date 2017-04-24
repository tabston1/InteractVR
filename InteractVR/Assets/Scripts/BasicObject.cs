using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;
using UnityEngine.EventSystems;

public class BasicObject : MonoBehaviour
{
	public string buildNo { get; set; }

	public Rigidbody rigidBod { get; private set; }

	public bool beingHeld { get; set; }

	bool hitGround = false;

	public GameObject billboard { get; private set; }

	public GameObject emptyParentContainer { get; private set; }

	float threshold = 0.5f;


	//Boolean representing whether gravity is enabled/disabled for this object
	public bool gravityOn;

	void Awake ()
	{
		billboard = Instantiate ((GameObject)Resources.Load ("Prefabs/NewBillboard"));
		billboard.SetActive (false);
	}

	void Start ()
	{
		beingHeld = false;
		gravityOn = true;

		if (transform.parent != null)
			emptyParentContainer = transform.parent.gameObject;

		//Grab reference to this object's rigidbody component
		rigidBod = gameObject.GetComponent<Rigidbody> ();
		if (rigidBod == null) {
			Debug.Log (gameObject.name + " needs a Rigidbody component");
		} else {
			rigidBod.isKinematic = false;
			rigidBod.maxAngularVelocity = 5.0f;
			rigidBod.maxDepenetrationVelocity = 1.0f;
		}

		//Ensure this object's billboard is a child of the empty parent object
		billboard.transform.SetParent (transform.parent);
		billboard.transform.position = new Vector3 (transform.position.x, transform.position.y + 2, transform.position.z - 1);
	}

	public void enableGravity ()
	{
		if (rigidBod != null) {
			rigidBod.useGravity = true;
		}
	}

	public void disableGravity ()
	{
		if (rigidBod != null) {
			rigidBod.useGravity = false;
		}
	}

	public void enableKinematic ()
	{
		if (rigidBod != null) {
			rigidBod.isKinematic = true;
		}
	}

	public void disableKinematic ()
	{
		if (rigidBod != null) {
			rigidBod.isKinematic = false;
		}
	}

	public void disableMotion ()
	{
		if (rigidBod != null) {
			rigidBod.constraints = RigidbodyConstraints.FreezeAll;
		}
	}

	public void enableMotion ()
	{
		if (rigidBod != null) {
			rigidBod.constraints = RigidbodyConstraints.None;
		}
	}


	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Movable") {
			if (!beingHeld) {
				//disableMotion ();
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		//Delete this object if it breaks out of the boundary box surrounding the main house object
		if (other.tag == "DeletionBoundary") {
			Debug.Log ("Deleting " + name);

			if (billboard != null) {
				//Disable any active tool
				billboard.BroadcastMessage ("disableTool");

				//If the Billboard is currently detached, it will not be destroyed with the object, so destroy it manually first
				Destroy (billboard);
			}

			//Now destroy the actual object (by deleting the empty parent object)
			Destroy (emptyParentContainer);

			//Emulate the "OnGazeLeave()" functionality from the Select script
			if (Manager.select != null) {
				EventSystem.current.SetSelectedGameObject (null);
				Manager.select.lineColor (Color.red, Color.red);
			}

		}
	}

	public IEnumerator DelayedObjectMotionFreeze ()
	{
		float elapsed = 0f;
		float reductionFactor = 0.8f;

		yield return new WaitForSeconds (5f);

		while (rigidBod.velocity.magnitude > threshold && !beingHeld) {
			elapsed += Time.deltaTime;
			Debug.Log (elapsed + " seconds spent slowing down " + name + ". Velocity: " + rigidBod.velocity.magnitude);

			//Reduce the velocity and angular velocity by some reduction factor
			rigidBod.velocity *= reductionFactor;
			rigidBod.angularVelocity *= reductionFactor;

			//After trying 10 to slow down the object for 10 seconds, just stop it
			if (elapsed >= 10f) {
				rigidBod.velocity = Vector3.zero;
				rigidBod.angularVelocity = Vector3.zero;
				break;
			}
			yield return new WaitForSeconds (0.5f);
		}

		Debug.Log ("Constraining motion for " + name);
		//Fully constrain rigidbody motion after slowing it down
		if (!beingHeld)
			rigidBod.constraints = RigidbodyConstraints.FreezeAll;
		else
			Debug.Log ("Didn't freeze " + name + " because it was grabbed by the user");

		yield break;
	}
}