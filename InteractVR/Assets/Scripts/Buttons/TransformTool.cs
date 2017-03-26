using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

//Base class for the Transform Tools (Translator, Rotator, Scaler)
public abstract class TransformTool : MonoBehaviour
{
	//Transform of object being manipulated
	public Transform obj;

	public TransformGizmo leftCameraScript;
	public TransformGizmo rightCameraScript;

	//True if a given tool is active
	private bool _active = false;

	public bool Active {
		get { return _active; }
		set {
			_active = value;
			Manager.activeTransformGizmo = value;

			Debug.Log ("Current Active value: " + Active);

			//if(_active);
			//if(!_active);
		}
	}

	void Start ()
	{
	}

	protected void onClick ()
	{
		//Try to grab the object being manipulated before enabling the tool
		if (getObject ()) {
			enableTool ();
		} else {
			Debug.Log ("Could not grab a reference to the object being transformed. Did not enable the Transform Gizmo.");
		}
	}

	//Enable a particular transformation tool
	protected virtual void enableTool ()
	{
		Active = true;

		leftCameraScript = GameObject.Find ("Main Camera Left").GetComponent<TransformGizmo> ();
		rightCameraScript = GameObject.Find ("Main Camera Right").GetComponent<TransformGizmo> ();

		if (leftCameraScript == null || rightCameraScript == null) {
			Debug.Log ("Couldn't find one of the TransformGizmos scripts on a camera --> BillboardExit/disableTransformTool()");
			//return;
		}

		//enable the TransformGizmo script on both the left and right Main Cameras
		//leftCameraScript.enabled = true;
		//rightCameraScript.enabled = true;

		//enable the Translation tool specifically within the TransformGizmo script
		//leftCameraScript.SetType ("Translate");
		//rightCameraScript.SetType ("Translate");

		//set the TrasnformGizmo target to the object being manipulated
		leftCameraScript.SetTarget (obj);
		rightCameraScript.SetTarget (obj);
	}

	//Disable the current transformation tool
	protected virtual void disableTool ()
	{
		//Don't try to disable an inactive tool
		if (!Active)
			return;
		
		//Set the TrasnformGizmo target back to null (no longer manipulating an object)
		leftCameraScript.SetTarget (null);
		rightCameraScript.SetTarget (null);

		//Indicate the current Transform tool is no longer active
		Active = false;
	}

	//Grab a reference to the GameObject being manipulated
	bool getObject ()
	{
		//If a reference to the object has already been grabbed, just return true
		if (obj != null)
			return true;

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
