using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Models : MonoBehaviour {
    public Canvas modelMenu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void onClick()
    {
        GameObject controller = GameObject.Find("Controller");
        controller.BroadcastMessage("menuButton");

        modelMenu.gameObject.SetActive(true);
    }
}
