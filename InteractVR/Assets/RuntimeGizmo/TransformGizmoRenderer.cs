using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

[RequireComponent (typeof(Camera))]
public class TransformGizmoRenderer : MonoBehaviour
{

	TransformGizmo gizmoScript = null;

	public void setGizmoReference (TransformGizmo script)
	{
		gizmoScript = script;
	}

	void OnPostRender ()
	{
		if (gizmoScript != null) {
			gizmoScript.RenderGizmo ();
		} else
			Debug.Log ("gizmoScript is null");
	}
}
