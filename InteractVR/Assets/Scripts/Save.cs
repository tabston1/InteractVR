using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save : MonoBehaviour
{

	protected StreamWriter writer;
	protected Dictionary<string, int> buildNumbers;
	protected GameObject mainCamera;
	protected GameObject modelMenu;
	protected GameObject[] allGameObjects;

	// Use this for initialization
	//void Start()
	void onClick ()
	{
        //writer = new StreamWriter(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/SavedScene.txt");
        //(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/SavedScene.txt");
        writer = new StreamWriter (Application.persistentDataPath + "/SavedScene.txt");
        buildNumbers = new Dictionary<string, int> ();
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera");

		//Find the model menu so build numbers can be accessed
		foreach (Transform child in mainCamera.transform) {
			if (child.name == "ModelMenu") {
				modelMenu = child.gameObject;
			}
		}

		addBuildNumbers ();
		saveState ();
	}

	//Adds all of the models' build numbers from the model menu to be easily looked up
	protected void addBuildNumbers ()
	{
		foreach (Transform child in modelMenu.transform) {
			if (child.name == "ModelButtons") {
                foreach (Transform button in child)
                {
                    buildNumbers.Add (button.name, 0);
                }
            }
      
		}

	}

	protected void saveState ()
	{
        Debug.Log("Saving");
		allGameObjects = (GameObject[])GameObject.FindObjectsOfType (typeof(GameObject));
		Transform objTrans;
        BasicObject bobj;
        GameObject model;

		//Iterate through all objects in the scene to find the ones that are currently instantiated
		foreach (GameObject obj in allGameObjects) {
			bobj = (BasicObject)obj.GetComponent (typeof(BasicObject));
            model = bobj.transform.parent.gameObject;
            if (model != null) {
                Debug.Log(model.name);
				//BasicObject bobj = (BasicObject)obj.GetComponent(typeof(BasicObject));
				if (buildNumbers.ContainsKey (bobj.buildNo)) {
                    Debug.Log("Save Build No" + bobj.buildNo);
                    objTrans = bobj.transform;
					buildNumbers [bobj.buildNo] += 1;

					//Save the state of every user instantiated object in the scene
					//Saves position, rotation, and scale
					writer.WriteLine (bobj.buildNo + " " +
					objTrans.position.x + " " + objTrans.position.y + " " + objTrans.position.z + " " +
					objTrans.rotation.eulerAngles.x + " " + objTrans.rotation.eulerAngles.y + " " + objTrans.rotation.eulerAngles.z + " " +
					objTrans.localScale.x + " " + objTrans.localScale.y + " " + objTrans.localScale.z);
                  
				}
			}
		}
		writer.Close ();

	}

	// Update is called once per frame
	void Update ()
	{
        
	}
}
