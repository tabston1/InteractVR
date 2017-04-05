using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancel : MonoBehaviour {

    void Start()
    {

    }

    void onClick()
    {
        GameObject controller = GameObject.Find("Controller");
        controller.BroadcastMessage("menuButton");
    }

}
