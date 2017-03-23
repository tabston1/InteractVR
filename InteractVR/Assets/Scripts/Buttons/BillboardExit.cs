using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class BillboardExit : MonoBehaviour
{

	private GameObject parent;
	TransformGizmo leftCameraScript;
	TransformGizmo rightCameraScript;

	void Start ()
	{
		if (this.transform.parent != null) {
			parent = this.transform.parent.gameObject;
		}

		leftCameraScript = GameObject.Find ("Main Camera Left").GetComponent<TransformGizmo> ();
		rightCameraScript = GameObject.Find ("Main Camera Right").GetComponent<TransformGizmo> ();


	}

	void onClick ()
	{
		//parent = this.transform.parent.gameObject;
		if (parent != null) {
			//disableTransformTool ();
			parent.SetActive (false);
		}

	}

	//Called whenever a particular Billboard is closed or destroyed (if it has this script attached)
	void OnDisable ()
	{
		if (parent != null)
			Debug.Log ("BillboardExit: Disabling object: " + parent.name);

		disableTransformTool ();
	}

	//Disable any Transform tool (Translate, Rotate, or Scale)
	void disableTransformTool ()
	{
		if (leftCameraScript == null || rightCameraScript == null) {
			Debug.Log ("Couldn't find one of the TransformGizmos scripts on a camera --> BillboardExit/disableTransformTool()");
			return;
		}

		//set the TrasnformGizmo target back to null
		leftCameraScript.SetTarget (null);
		rightCameraScript.SetTarget (null);
	}
}
