using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

//Base class for the Transform Tools (Translator, Rotator, Scaler)
public abstract class TransformTool : MonoBehaviour
{
	//Transform of object being manipulated
	public Transform obj = null;

	//Reference to the script handling all of the Transformation Gizmo work
	public TransformGizmo gizmoScript;

	//The billboard that corresponds to this button
	public GameObject Billboard { get; set; }

	//True if a given tool is active
	private bool _active = false;

	public bool Active {
		get { return _active; }
		set {
			_active = value;

			if (_active) {
				if (Manager.activeTransformGizmo)
					Debug.Log ("Activating a transformation tool, but the Manager says a tool is already active!");
			} else {
				//Debug.Log ("Deactivating a transformation tool");
			}

			Manager.activeTransformGizmo = value;
		}
	}



	void Start ()
	{
		setObjectandBillboard ();

		gizmoScript = GameObject.Find ("Main Camera").GetComponent<TransformGizmo> ();
		if (gizmoScript == null)
			Debug.Log ("Could not grab a reference to the TransformGizmo script on the Main Camera");
	}

	protected void onClick ()
	{
		//Ensure we have a reference to the object before enabling the tool
		if (obj != null) {
			//Enable the tool, but disable it instead if the user clicks the button again while it is already enabled
			if (!Active)
				enableTool ();
			else
				disableTool ();
		} else {
			Debug.Log ("No reference to the object being transformed. Did not enable the Transform Gizmo.");
		}
	}

	//Enable a particular transformation tool
	protected virtual void enableTool ()
	{
		if (gizmoScript == null)
			return;

		//Ensure no other tool is enabled already (even on a different billboard)
		Manager.disableAllTransformTools ();

		Active = true;

		//Change the object's layer so that the laser will ignore it while a tool is enabled
		obj.gameObject.layer = LayerMask.NameToLayer ("Ignore Raycast");

		gizmoScript.SetTarget (obj);
	}

	//Disable the current transformation tool
	protected virtual void disableTool ()
	{
		//Don't try to disable an inactive tool
		if (!Active)
			return;
		
		//Set the TrasnformGizmo target back to null (no longer manipulating an object)
		gizmoScript.SetTarget (null);

		//Indicate the current Transform tool is no longer active
		Active = false;

		//Change the object's layer back to Movable
		obj.gameObject.layer = LayerMask.NameToLayer ("Movable");
	}

	//Grab a reference to the GameObject being manipulated
	void setObjectandBillboard ()
	{
		//Current hierarchy: this button -> Billboard -> GameObject being manipulated
		Transform billboard = transform.parent;
		if (billboard == null) {
			Debug.Log ("Could not grab a reference to the Billboard associated with this object.");
			return;
		}

		Billboard = billboard.gameObject;
		obj = billboard.parent;
	}
}
