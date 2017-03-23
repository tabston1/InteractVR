using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class Scaler : MonoBehaviour
{

	void onClick ()
	{
		enableScaleTool ();
	}

	void enableScaleTool ()
	{
		var leftCameraScript = GameObject.Find ("Main Camera Left").GetComponent<TransformGizmo> ();
		var rightCameraScript = GameObject.Find ("Main Camera Right").GetComponent<TransformGizmo> ();

		if (leftCameraScript == null || rightCameraScript == null) {
			Debug.Log ("Couldn't find one of the TransformGizmos scripts on a camera --> BillboardExit/disableTransformTool()");
			return;
		}

		//enable the TransformGizmo script on both the left and right Main Cameras
		//leftCameraScript.enabled = true;
		//rightCameraScript.enabled = true;

		//enable the Rotation tool specifically within the TransformGizmo script
		leftCameraScript.SetType ("Scale");
		rightCameraScript.SetType ("Scale");
	}
}
