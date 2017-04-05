using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models : MonoBehaviour {

    private GameObject manager;
    private Manager managerScript;

    // Use this for initialization
    void Start () {
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void onClick()
    {
        GameObject controller = GameObject.Find("Controller");
        controller.BroadcastMessage("menuButton");

        managerScript.modelMenu.gameObject.SetActive(true);
        managerScript.modelMenuIsActive = true;
    }
}
