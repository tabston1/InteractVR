using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    void onSelect()
    {
        Transform child = this.transform.FindChild("Description");
        child.gameObject.SetActive(true);
    }
}
