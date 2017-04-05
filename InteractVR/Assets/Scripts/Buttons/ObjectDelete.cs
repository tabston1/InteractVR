using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDelete : MonoBehaviour
{
	//Object associated with the billboard
	private GameObject obj;

	//Billboard on which this button is placed
	private GameObject billboard;

	void Start ()
	{
		if (this.transform.parent != null) {
			billboard = this.transform.parent.gameObject;

			if (billboard.transform.parent != null) {
				obj = billboard.transform.parent.gameObject;
			}
		}
	}

	void onClick ()
	{
		if (billboard != null) {
			//Disable any active tool
			billboard.BroadcastMessage ("disableTool");

			//If the Billboard is currently detached, it will not be destroyed with the object, so destroy it manually first
			Destroy (billboard);
		}
			
		//Now destroy the actual object
		Destroy (obj);

		//Emulate the "OnGazeLeave()" functionality from the Select script
		if (Manager.select != null) {
			EventSystem.current.SetSelectedGameObject (null);
			Manager.select.lineColor (Color.red, Color.red);
		}
	}
}
