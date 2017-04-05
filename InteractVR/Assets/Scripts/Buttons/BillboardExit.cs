using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class BillboardExit : MonoBehaviour
{

	private GameObject billboard;


	void Start ()
	{
		if (this.transform.parent != null) {
			//billboard = this.transform.parent.gameObject;
			billboard = this.transform.parent.transform.parent.gameObject;
		}
	}

	void onClick ()
	{
		if (billboard != null) {
			//Disable any active tool
			billboard.BroadcastMessage ("disableTool");

			//Hide the billboard
			billboard.SetActive (false);
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
