using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class BillboardExit : MonoBehaviour
{
	public GameObject emptyParentContainer;

	//Object associated with the billboard and wrapper script for object models
	private GameObject obj;
	private BasicObject objScript;

	//Reference to billboard/toolbar object
	private GameObject billboard;


	void Start ()
	{
		/*
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
		*/

		//Current hierarchy: this button -> Slot (grid layout) -> Billboard -> Empty parent object wrapper
		//Empty parent object wrapper has 2 children: billboard and model object
		billboard = transform.parent.transform.parent.gameObject;

		if (billboard != null) {
			emptyParentContainer = billboard.transform.parent.gameObject;
		
			if (emptyParentContainer != null) {
				obj = emptyParentContainer.transform.GetChild (0).gameObject;

				if (obj != null) {
					//Grab reference to this object's object wrapper script
					objScript = obj.GetComponent<BasicObject> ();
					if (objScript == null)
						Debug.Log ("Could not grab basic object script for " + obj.name + " from button " + name);
				} else
					Debug.Log ("Could not grab model object reference from " + name);
			} else
				Debug.Log ("Could not grab emptyParentContainer reference from " + name);
		} else
			Debug.Log ("Could not grab billboard reference from " + name);
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
				objScript.enableMotion ();
				objScript.enableGravity ();
			} else {
				objScript.disableMotion ();
				objScript.disableGravity ();
			}
		}

	}
}
