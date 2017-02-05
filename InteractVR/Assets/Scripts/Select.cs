using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {

    private GameObject camera;
    private GameObject hitObject;
    private GameObject description;
    private Transform cameraTrans;
    private RaycastHit hit;
    public 


    void OnGazeEnter()
    {
        //Enable the description for the object
        foreach (Transform child in hit.transform)
        {
            if (child.CompareTag("Description"))
            {
                child.gameObject.SetActive(true);
            }
        }



    }
    
    void OnGazeExit()
    {
  

    }


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
            OnGazeEnter();
            OnGazeExit();
        }
        
    }
}
