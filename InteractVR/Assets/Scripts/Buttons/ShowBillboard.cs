using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBillboard : MonoBehaviour {

    private GameObject billboard;
    private GameObject Camera;

    //Finds the child object Billboard with a specific tag
    public static GameObject FindComponentInChildWithTag(GameObject parent, string tag)
    {

        Transform t = parent.transform;
        foreach (Transform child in t)
        {
            Debug.Log(child.name);
            if (child.tag == tag)
            {
                Debug.Log(child.name);
                return child.gameObject;
            }

        }

        return null;

    }

    void onSelect()
    {
       
        Camera = GameObject.Find("Main Camera");
        billboard = FindComponentInChildWithTag(this.gameObject, "Billboard");
        Debug.Log(this.name);
        Debug.Log(billboard);

        if (billboard != null)
        {
            //billboard.transform.position = Camera.transform.position + (5 * Camera.transform.forward);
            billboard.transform.forward = -Camera.transform.forward;
            billboard.SetActive(true);
        }
    }

   
}
