using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDelete : MonoBehaviour
{

	private GameObject parent;

	void onClick ()
	{
		Destroy (this.transform.parent.transform.parent.gameObject);


	}


}
