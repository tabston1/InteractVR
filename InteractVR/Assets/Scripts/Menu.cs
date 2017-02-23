using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public Canvas menu;
    private bool isActive;

	// Use this for initialization
	void Start () {
        isActive = false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetButtonDown("Submit")) menuButton();
	}

    void menuButton()
    {
        menu.gameObject.SetActive(!isActive);
        isActive = !isActive;
    }
}
