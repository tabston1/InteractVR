using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RuntimeGizmos;

public class BasicObject : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
	}

	void onSelect ()
	{
		Transform child = this.transform.FindChild ("Description");
		child.gameObject.SetActive (true);
		child = child.transform.FindChild ("Text");
		child.gameObject.SetActive (true);
	}

	void OnDisable ()
	{
		Debug.Log ("BasicObject: Disabling object: " + gameObject.name);


	}
}
