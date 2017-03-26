using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

//inherits from TransformTool, which inherits from Monobehavior
public class Translator : TransformTool
{
	//Enable the Translation tool specifically from the TransformGizmo script
	protected override void enableTool ()
	{
		base.enableTool ();

		gizmoScript.SetType ("Translate");
	}
}
