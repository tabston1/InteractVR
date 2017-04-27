using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBillboard : MonoBehaviour
{
	//Wrapper script for the object model on which this script is attached
	private BasicObject objScript;

	//Reference to billboard/toolbar object
	private GameObject billboard;

	private GameObject Camera;

	//Finds the child object Billboard with a specific tag
	public static GameObject FindComponentInChildWithTag (GameObject parent, string tag)
	{

		Transform t = parent.transform;
		foreach (Transform child in t) {
			Debug.Log (child.name);
			if (child.tag == tag) {
				Debug.Log (child.name);
				return child.gameObject;
			}

		}

		return null;

	}

	void Start ()
	{
		Camera = GameObject.Find ("Main Camera");

		//Grab reference to this object's object wrapper script
		objScript = gameObject.GetComponent<BasicObject> ();

		//billboard = FindComponentInChildWithTag (this.transform.parent.gameObject, "Billboard");
		if (objScript != null) {
			billboard = objScript.billboard;
		} else {
			Debug.Log ("Object " + name + " does not have the BasicObject script attached to it!");
		}
	}

	void onSelect ()
	{ 
		if (billboard != null) {
			//Temporarily disable gravity on this object's rigidbody whenever its billboard/toolbar is active
			objScript.disableMotion ();
			objScript.disableGravity ();

			//billboard.transform.position = Camera.transform.position + (5 * Camera.transform.forward);
			billboard.transform.position = gameObject.transform.position;
			billboard.transform.Translate (Vector3.up * 2, Space.World);
			//billboard.transform.forward = -Camera.transform.forward;
			billboard.transform.forward = -Manager.select.controllerDirection;
			billboard.SetActive (true);
		} else
			Debug.Log ("Could not find reference to billboard from ShowBillboard script on " + name);
	}

   
}
