using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{

	public Canvas menu;
	private bool isActive;

	// Use this for initialization
	void Start ()
	{
		isActive = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Submit"))
			menuButton ();
	}

	void menuButton ()
	{
		//Before opening the main menu, disable any active transformation tools
		if (!isActive) {
			//Ensure no other tool is enabled already (even on a different billboard)
			Manager.disableAllTransformTools ();
		}

		menu.gameObject.SetActive (!isActive);
		isActive = !isActive;
	}
}
