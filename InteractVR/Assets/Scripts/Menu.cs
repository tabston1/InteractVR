using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public Canvas menu;
    private bool isActive;

	// Use this for initialization
	void Start () {
        isActive = false;
        menu.transform.position = menu.transform.parent.transform.position;
        menu.transform.position = new Vector3(menu.transform.position.x, menu.transform.position.y, menu.transform.position.z + 1f);
        menu.gameObject.SetActive(false);

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
