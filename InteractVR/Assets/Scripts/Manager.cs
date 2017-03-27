using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public bool authoring;
    public UnityEngine.UI.Text mode;

    public Canvas menu;
    public Canvas modelMenu;
    public Canvas syncMenu;

    public bool menuIsActive;
    public bool modelMenuIsActive;
    public bool syncMenuIsActive;

    private GameObject controller;
   // public Quaternion controllerOffset;
    public Vector3 controllerOffset;

    // Use this for initialization
    void Start () {
        authoring = false;
        mode.text = "Current mode: Run-time";

        menu.gameObject.SetActive(false);
        modelMenu.gameObject.SetActive(false);
        syncMenu.gameObject.SetActive(false);

        menuIsActive = false;
        modelMenuIsActive = false;
        syncMenuIsActive = false;

        controller = GameObject.FindGameObjectWithTag("Controller");
        //controllerOffset = Quaternion.identity;
        controllerOffset = new Vector3();
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
