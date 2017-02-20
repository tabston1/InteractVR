using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public bool authoring;
    public UnityEngine.UI.Text mode;

	// Use this for initialization
	void Start () {
        authoring = false;
        mode.text = "Current mode: Run-time";
	}
}
