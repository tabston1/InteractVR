using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{

    private GameObject manager;
    InstantiateObject newObj;

    void onClick()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        newObj = (InstantiateObject)manager.GetComponent(typeof(InstantiateObject));
        newObj.addObject(this.name);

    }

}
