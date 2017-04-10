using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDelete : MonoBehaviour
{
	public GameObject emptyParentContainer;

	//Object associated with the billboard and wrapper script for object models
	private GameObject obj;
	private BasicObject objScript;

	//Billboard on which this button is placed
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
