using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class BillboardExit : MonoBehaviour
{
	//Object associated with the billboard and wrapper script for object models
	private GameObject obj;
	private BasicObject objScript;

	//Reference to billboard/toolbar object
	private GameObject billboard;


	void Start ()
	{
		//Current hierarchy: this button -> Slot (grid layout) -> Billboard -> GameObject being manipulated
		if (this.transform.parent != null) {
			//billboard = this.transform.parent.gameObject;
			billboard = this.transform.parent.transform.parent.gameObject;

			if (billboard.transform.parent != null) {
				obj = billboard.transform.parent.gameObject;

				//Grab reference to this object's object wrapper script
				objScript = obj.GetComponent<BasicObject> ();
			}
		}
	}

	void onClick ()
	{
		if (billboard != null) {
			//Disable any active tool
			billboard.BroadcastMessage ("disableTool");

			//Hide the billboard
			billboard.SetActive (false);

			//Enable/Disable gravity if it is turned on/off for this object (as billboard closes)
			if (objScript.gravityOn) {
				objScript.enableGravity ();
			} else {
				objScript.disableGravity ();
			}
		}

	}

	//Called whenever a particular Billboard is closed or destroyed (if it has this script attached)
	void OnDisable ()
	{
		/*
		if (billboard != null) {
			//Debug.Log ("BillboardExit: Disabling object: " + billboard.name);
			billboard.BroadcastMessage ("disableTool");
		}
		*/
	}
}
