﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
	//True if any transformation tool is active on any billboard
	public static bool activeTransformGizmo = false;

	//Global reference to the Controller object
	public static GameObject controller = null;




	//Global reference to the Controller's Select script (for laser information)
	public static Select select = null;

  public bool menuIsActive;
  public bool modelMenuIsActive;
  public bool syncMenuIsActive;
	public bool authoring;
	public UnityEngine.UI.Text mode;

	public Canvas menu;
	public Canvas modelMenu;
  public Canvas syncMenu;
	

   public Vector3 controllerOffset;
  
  // Use this for initialization
	void Start ()
	{
   
        
       
    menuIsActive = false;
        modelMenuIsActive = false;
        syncMenuIsActive = false;

        
        
    
  
  
		authoring = false;
		mode.text = "Current mode: Run-time";

		menu.gameObject.SetActive (false);
		modelMenu.gameObject.SetActive (false);
    syncMenu.gameObject.SetActive(false);
    controllerOffset = new Vector3();


		//Grab the Controller object from the scene
		controller = GameObject.FindGameObjectWithTag ("Controller");
		if (controller != null) {
			//Grab the Select script attached to the Controller object in the scene
			select = controller.GetComponent<Select> ();
			if (select == null) {
				Debug.Log ("Could not grab a reference to the Controller's Select script");
			}
		} else
			Debug.Log ("Could not grab a reference to the Controller object");
	}

	//Utility function to disable any open Transform Gizmo tool by broadcasting to all open Billboards
	public static void disableAllTransformTools ()
	{
		GameObject[] allBillboards = GameObject.FindGameObjectsWithTag ("Billboard");
		foreach (GameObject billboard in allBillboards) {
			billboard.BroadcastMessage ("disableTool");
		}
	}
  
   void Sync()
    {
        //controllerOffset = controllerOffset * controller.transform.rotation;
        controllerOffset = controllerOffset + controller.transform.rotation.eulerAngles;
    }

}
