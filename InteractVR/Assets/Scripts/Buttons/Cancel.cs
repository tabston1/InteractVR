using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancel : MonoBehaviour
{

	private GameObject manager;
	private Manager managerScript;
	GameObject landingController;

	GameObject controller;

	void Start ()
	{
		manager = GameObject.Find ("Manager");
		managerScript = manager.GetComponent<Manager> ();

		controller = GameObject.FindGameObjectWithTag ("Controller");
		landingController = GameObject.Find ("LandingController");

	}

	void onClick ()
	{
		this.gameObject.transform.parent.gameObject.SetActive (false);
		if (managerScript.menu.gameObject.Equals (this.gameObject.transform.parent.gameObject))
			managerScript.menuIsActive = false;

		if (managerScript.modelMenu.gameObject.Equals (this.gameObject.transform.parent.gameObject))
			managerScript.modelMenuIsActive = false;

		if (managerScript.landingMenu.gameObject.Equals (this.gameObject.transform.parent.gameObject)) {
			managerScript.landingMenuIsActive = false;
			//GameObject landingController = GameObject.Find("LandingController");
			//landingController.SetActive(false);
			if (landingController != null) {
				landingController.SetActive (false);
			}
			return;
		}

		controller.BroadcastMessage ("menuButton");
	}

}
