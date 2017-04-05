using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class controllerOrientation : MonoBehaviour {

    private SerialPort stream;
    private GameObject controller;

    private GameObject manager;
    private Manager managerScript;

    private float x;
    private float y;
    private float z;

    // Use this for initialization
    void Start () {

        controller = GameObject.Find("Controller");
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();

        stream = new SerialPort("COM5", 115200);
        stream.ReadTimeout = 50;
        stream.Open();

        StartCoroutine
        (
            AsynchronousReadFromArduino
            (   (string s) => updateController(s),  // Callback
                () => Debug.LogError("Error!"),     // Error callback
                10000f                                 // Timeout (seconds)
            )
        );


    }

	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator AsynchronousReadFromArduino(Action<string> callback, Action fail = null, float timeout = float.PositiveInfinity)
    {
        DateTime initialTime = DateTime.Now;
        DateTime nowTime;
        TimeSpan diff = default(TimeSpan);

        string dataString = null;

        do
        {
            try
            {
                dataString = stream.ReadLine();
            }
            catch (TimeoutException)
            {
                dataString = null;
            }

            if (dataString != null)
            {
                callback(dataString);
                yield return null;
            }
            else
                yield return new WaitForSeconds(0.05f);

            nowTime = DateTime.Now;
            diff = nowTime - initialTime;

        } while (diff.Milliseconds < timeout);

        if (fail != null)
            fail();
        yield return null;
    }

    void updateController(string s)
    {
        // rotate the controller
        string[] orientation = s.Split(' ');

        x = -Convert.ToSingle(orientation[1]) - managerScript.controllerOffset.x;
        y = -Convert.ToSingle(orientation[2]) - managerScript.controllerOffset.y;
        z = Convert.ToSingle(orientation[0]) - managerScript.controllerOffset.z;

        Quaternion rotation = Quaternion.Euler(x, y, z);

        Debug.Log("rotation:" + rotation);
        Debug.Log("offset:" + managerScript.controllerOffset);

        //controller.transform.rotation = rotation * Quaternion.Inverse(managerScript.controllerOffset);
        controller.transform.rotation = rotation;

    }
}
