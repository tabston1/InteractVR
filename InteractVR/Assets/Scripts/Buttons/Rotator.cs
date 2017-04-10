using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

//inherits from TransformTool, which inherits from Monobehavior
public class Rotator : TransformTool
{
	//Enable the Rotation tool specifically from the TransformGizmo script
	protected override void enableTool ()
	{
		base.enableTool ();

		gizmoScript.SetType ("Rotate");

		//Detach the billboard as a child object so it is not rotated with the transform target object
		//detachBillboard ();
	}

	//Reattach the billboard to the object after the Rotation tool has been disabled
	protected override void disableTool ()
	{
		base.disableTool ();

		//reattachBillboard ();
	}
}