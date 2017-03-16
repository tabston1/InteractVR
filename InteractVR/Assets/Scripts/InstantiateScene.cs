using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateScene : MonoBehaviour {

    
    void Start()
    {
        //Load the scene with the models in it
        SceneManager.LoadScene("CloudScene", LoadSceneMode.Single);
    }
   
    // Update is called once per frame
    void Update () {
		
	}
}
