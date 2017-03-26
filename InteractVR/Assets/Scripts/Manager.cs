using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	public static bool activeTransformGizmo = false;
	public bool authoring;
	public UnityEngine.UI.Text mode;

	public Canvas menu;
	public Canvas modelMenu;

	// Use this for initialization
	void Start ()
	{
		authoring = false;
		mode.text = "Current mode: Run-time";

		menu.gameObject.SetActive (false);
		modelMenu.gameObject.SetActive (false);
	}
}
