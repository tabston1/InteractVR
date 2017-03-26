using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDelete : MonoBehaviour
{
	private GameObject parent;

	void onClick ()
	{
		Destroy (this.transform.parent.transform.parent.gameObject);

		//Emulate the "OnGazeLeave()" functionality from the Select script
		if (Manager.select != null) {
			EventSystem.current.SetSelectedGameObject (null);
			Manager.select.lineColor (Color.red, Color.red);
		}
	}
}
