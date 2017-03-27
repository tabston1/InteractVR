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

		detachBillboard ();
	}

	//Enable the Rotation tool specifically from the TransformGizmo script
	protected override void disableTool ()
	{
		base.disableTool ();

		reattachBillboard ();
	}
}