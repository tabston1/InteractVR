using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {

    GameObject camera;
    GameObject hitObject;
    Transform cameraTrans;
    RaycastHit hit;

	// Use this for initialization
	void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraTrans = camera.GetComponent<Transform>();
    }

    void Update()
    {




    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.GetButtonDown("Jump"))
        {

            Debug.DrawRay(cameraTrans.position, cameraTrans.forward * 10, Color.green);
            if (Physics.Raycast(cameraTrans.position, cameraTrans.forward, out hit, 10))
            {
                //Debug.DrawRay(cameraTrans.position, cameraTrans.forward * 20, Color.green);
                Debug.Log("Hit an Object");
                hitObject = hit.transform.gameObject;
                Debug.Log(hitObject.name);
            }

        }
    }
}
