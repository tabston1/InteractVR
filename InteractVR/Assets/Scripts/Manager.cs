using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public bool authoring;
    public UnityEngine.UI.Text mode;

    public Canvas menu;
    public Canvas modelMenu;
    public Canvas syncMenu;
    public Canvas landingMenu;

    public bool menuIsActive;
    public bool modelMenuIsActive;
    public bool syncMenuIsActive;
    public bool landingMenuIsActive;

    private GameObject controller;
    public Vector3 controllerOffset;

    public GameObject landingController;

    // Use this for initialization
    void Start () {
        authoring = false;
        mode.text = "Current mode: Run-time";

        menu.gameObject.SetActive(false);
        modelMenu.gameObject.SetActive(false);
        syncMenu.gameObject.SetActive(false);
        landingMenu.gameObject.SetActive(true);

        menuIsActive = false;
        modelMenuIsActive = false;
        syncMenuIsActive = false;
        landingMenuIsActive = true;

        controller = GameObject.FindGameObjectWithTag("Controller");
        controllerOffset = new Vector3();

        landingController.SetActive(true);
    }

    /*
    void Update()
    {
        
        if (Input.GetButton("Fire1")) Debug.Log("Fire1");
        if (Input.GetButton("Fire2")) Debug.Log("Fire2");
        if (Input.GetButton("Jump")) Debug.Log("Jump");
        if (Input.GetButton("Submit")) Debug.Log("Submit");
        if (Input.GetButton("Cancel")) Debug.Log("Cancel");
    }
*/
    void Sync()
    {
        //controllerOffset = controllerOffset * controller.transform.rotation;
        controllerOffset = controllerOffset + controller.transform.rotation.eulerAngles;
    }
}
