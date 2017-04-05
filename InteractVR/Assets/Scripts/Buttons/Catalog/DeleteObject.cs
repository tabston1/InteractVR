using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour {

    GameObject currentObj;

	// Use this for initialization
	void Start () {
        currentObj = this.gameObject;
    }

    void onSelect()
    {
        Destroy(currentObj.transform.root);
    }
	
    
}
