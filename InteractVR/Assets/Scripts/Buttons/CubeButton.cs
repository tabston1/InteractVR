using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeButton : MonoBehaviour {

    private GameObject camera;

    void onClick()
    {
        camera = GameObject.Find("Main Camera");

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = camera.transform.position + (5 * camera.transform.forward);
    }
}
