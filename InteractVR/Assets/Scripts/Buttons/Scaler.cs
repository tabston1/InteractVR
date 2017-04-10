using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

//inherits from TransformTool, which inherits from Monobehavior
public class Scaler : TransformTool
{
	//Enable the Scale tool specifically from the TransformGizmo script
	protected override void enableTool ()
	{
		base.enableTool ();

		gizmoScript.SetType ("Scale");

		//Detach the billboard as a child object so it is not scaled with the transform target object
		//detachBillboard ();
	}

	//Reattach the billboard to the object after the Scale tool has been disabled
	protected override void disableTool ()
	{
		base.disableTool ();

		//reattachBillboard ();
	}
}