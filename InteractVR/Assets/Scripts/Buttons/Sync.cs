using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sync : MonoBehaviour {

    private GameObject manager;
    private Manager managerScript;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void onClick()
    {
        GameObject controller = GameObject.Find("Controller");
        controller.BroadcastMessage("menuButton");

        managerScript.syncMenu.gameObject.SetActive(true);
        managerScript.syncMenuIsActive = true;
    }
}
