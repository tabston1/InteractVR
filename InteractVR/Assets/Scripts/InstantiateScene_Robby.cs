using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateScene_Robby : MonoBehaviour
{

    
	void Start ()
	{
		//Load the scene with the models in it
		SceneManager.LoadScene ("Robby2", LoadSceneMode.Single);
	}
   
	// Update is called once per frame
	void Update ()
	{
		
	}
}
