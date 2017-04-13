using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class BasicObject : MonoBehaviour
{
	public string buildNo { get; set; }

	public Rigidbody rigidBod { get; private set; }

	//Boolean representing whether gravity is enabled/disabled for this object
	public bool gravityOn = false;

	void Start ()
	{
		rigidBod = gameObject.GetComponent<Rigidbody> ();
		if (rigidBod == null) {
			Debug.Log (gameObject.name + " needs a Rigidbody component");
		}
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
}