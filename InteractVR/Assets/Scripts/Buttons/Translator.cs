using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class Translator : MonoBehaviour
{
	Transform obj;
	//Transform of object being manipulated

	void onClick ()
	{
		//try to grab the object being manipulated before enabling the Translate tool
		if (getObject ()) {
			enableTranslateTool ();
		} else {
			Debug.Log ("Could not grab a reference to the object being transformed. Did not enable the Translate tool.");
		}
	}

	//Enable the Translation tool specifically from the TransformGizmo script
	void enableTranslateTool ()
	{
		var leftCameraScript = GameObject.Find ("Main Camera Left").GetComponent<TransformGizmo> ();
		var rightCameraScript = GameObject.Find ("Main Camera Right").GetComponent<TransformGizmo> ();

		if (leftCameraScript == null || rightCameraScript == null) {
			Debug.Log ("Couldn't find one of the TransformGizmos scripts on a camera --> BillboardExit/disableTransformTool()");
			//return;
		}

		//enable the TransformGizmo script on both the left and right Main Cameras
		//leftCameraScript.enabled = true;
		//rightCameraScript.enabled = true;

		//enable the Translation tool specifically within the TransformGizmo script
		leftCameraScript.SetType ("Translate");
		rightCameraScript.SetType ("Translate");

		//set the TrasnformGizmo target to the object being manipulated
		leftCameraScript.SetTarget (obj);
		rightCameraScript.SetTarget (obj);
	}

	//Grab a reference to the GameObject being manipulated
	bool getObject ()
	{
		//Current hierarchy: this button -> Billboard -> GameObject being manipulated
		Transform billboard = transform.parent;
		if (billboard == null)
			return false;

		obj = billboard.parent;
		if (obj == null)
			return false;

		return true;
	}
}
