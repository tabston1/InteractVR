using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class BasicObject : MonoBehaviour
{
	public string buildNo { get; set; }

	public Rigidbody rigidBod { get; private set; }

	public bool beingHeld { get; set; }

	public GameObject billboard { get; private set; }


	//Boolean representing whether gravity is enabled/disabled for this object
	public bool gravityOn = false;

	void Awake ()
	{
		billboard = Instantiate ((GameObject)Resources.Load ("Prefabs/NewBillboard"));
		billboard.SetActive (false);
	}

	void Start ()
	{
		beingHeld = false;

		//Grab reference to this object's rigidbody component
		rigidBod = gameObject.GetComponent<Rigidbody> ();
		if (rigidBod == null) {
			Debug.Log (gameObject.name + " needs a Rigidbody component");
		}

		//Ensure this object's billboard is a child of the empty parent object
		billboard.transform.SetParent (transform.parent);
		billboard.transform.position = new Vector3 (transform.position.x, transform.position.y + 2, transform.position.z - 1);
	}

	public void enableGravity ()
	{
		if (rigidBod != null) {
			//rigidBod.isKinematic = false;
			rigidBod.useGravity = true;
		}
	}

	public void disableGravity ()
	{
		if (rigidBod != null) {
			/*
			if (beingHeld) {
				rigidBod.isKinematic = false;
			} else {
				rigidBod.isKinematic = true;
			}
			*/
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

	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Wall") {
			if (!beingHeld && rigidBod != null) {
				rigidBod.isKinematic = false;
			}
		}
	}
}