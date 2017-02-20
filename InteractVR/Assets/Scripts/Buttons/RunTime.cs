using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTime : MonoBehaviour {

    private GameObject manager;
    private Manager managerScript;

    // Use this for initialization
    void Start()
    {
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();
    }

    void onClick()
    {
        managerScript.authoring = true;
        managerScript.mode.text = "Current mode: Run-time";
    }
}
