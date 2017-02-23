using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cancel : MonoBehaviour {

    public Canvas modelMenu;

    void onClick()
    {
        modelMenu.gameObject.SetActive(false);

        GameObject controller = GameObject.Find("Controller");
        controller.BroadcastMessage("menuButton");
    }

}
