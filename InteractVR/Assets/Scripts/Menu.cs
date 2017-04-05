using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{


    private GameObject manager;
    private Manager managerScript;

    private GameObject controller;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();

        controller = GameObject.FindGameObjectWithTag("Controller");
    }






	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Submit"))
			menuButton ();
	}


    void menuButton()
    {
        if (managerScript.modelMenuIsActive)
        {
            managerScript.modelMenu.gameObject.SetActive(false);
            managerScript.modelMenuIsActive = false;
        }

        else if (managerScript.syncMenuIsActive)
        {
            managerScript.BroadcastMessage("Sync");

            managerScript.syncMenu.gameObject.SetActive(false);
            managerScript.syncMenuIsActive = false;
        }

        else
        {
            if (!managerScript.menuIsActive) {
			          //Ensure no other tool is enabled already (even on a different billboard)
			          Manager.disableAllTransformTools ();
            }
    
            
            managerScript.menu.gameObject.SetActive(!managerScript.menuIsActive);
            managerScript.menuIsActive = !managerScript.menuIsActive;
        }
    }



}
