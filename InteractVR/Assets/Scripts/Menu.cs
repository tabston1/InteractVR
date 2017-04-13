using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    private GameObject manager;
    private Manager managerScript;

    private GameObject controller;

    private float timer;
    private const float syncTime = 1.5f;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();

        controller = GameObject.FindGameObjectWithTag("Controller");
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetButton("Submit") || managerScript.submit)
        {
            timer += Time.deltaTime;
            if (timer >= syncTime)
            {
                Sync();
            }
        }

        else if (Input.GetButtonUp("Submit") || managerScript.submitUp)
        {
            if (timer < syncTime) menuButton();
            timer = 0f;
        }
	}

    void menuButton()
    {

        if (managerScript.syncMenuIsActive)
        {
            managerScript.BroadcastMessage("Sync");

            managerScript.syncMenu.gameObject.SetActive(false);
            managerScript.syncMenuIsActive = false;
        }
        else if (managerScript.landingMenuIsActive)
        {
            managerScript.landingMenu.gameObject.SetActive(false);
            managerScript.landingMenuIsActive = false;
        }
        else if (managerScript.modelMenuIsActive)
        {
            managerScript.modelMenu.gameObject.SetActive(false);
            managerScript.modelMenuIsActive = false;
        }

        else if(managerScript.modelMenuIsActive)
        {
            managerScript.menu.gameObject.SetActive(false);
            managerScript.menuIsActive = false;
        }
        else
        {
            managerScript.menu.gameObject.SetActive(true);
            managerScript.menuIsActive = true;
        }
    }

    void Sync()
    {
        managerScript.syncMenu.gameObject.SetActive(true);
        managerScript.syncMenuIsActive = true;
    }
}
