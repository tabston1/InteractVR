using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class controllerOrientation : MonoBehaviour {

    private SerialPort stream;
    private GameObject controller;

	// Use this for initialization
	void Start () {

        controller = GameObject.Find("Controller");

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
        Debug.Log(s);

        string[] orientation = s.Split(' ');

        Quaternion rotation = Quaternion.Euler(Convert.ToSingle(orientation[1]), -Convert.ToSingle(orientation[2]), -Convert.ToSingle(orientation[0]));

        controller.transform.rotation = rotation;
        //controller.transform.Rotate(new Vector3(Convert.ToSingle(orientation[0]), Convert.ToSingle(orientation[2]), Convert.ToSingle(orientation[1])));
        //controller.transform.
    }
}
