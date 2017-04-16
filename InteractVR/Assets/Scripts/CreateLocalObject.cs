using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLocalObject : MonoBehaviour {

    protected GameObject mainCamera;
    protected GameObject instantiatedModel;
    public GameObject model;


    void onClick()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");

        //Instantiates the model and places it in front of the camera
        if (model != null)
        {
            instantiatedModel = Instantiate(model);
            if (mainCamera != null)
            {
                instantiatedModel.transform.position = mainCamera.transform.position + (5 * mainCamera.transform.forward);
            }
        }
    }
}
