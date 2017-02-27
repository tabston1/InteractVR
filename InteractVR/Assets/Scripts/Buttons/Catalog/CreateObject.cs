using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObject : MonoBehaviour
{

    private GameObject camera;
    GameObject bed;

    void onClick()
    {
        camera = GameObject.Find("Main Camera");
        bed = (GameObject)Instantiate(Resources.Load(this.name));
        bed.transform.position = camera.transform.position + (5 * camera.transform.forward);

    }

}
