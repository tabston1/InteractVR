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

    private bool submit;
    private bool fire1;
    private bool select;

    // Use this for initialization
    void Start () {

        controller = GameObject.Find("Controller");
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();

        submit = false;
        fire1 = false;
        select = false;

        stream = new SerialPort("COM1", 9600);
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
        string buttons;

        //Debug.Log(s);

        try
        {
            x = -Convert.ToSingle(orientation[1]) - managerScript.controllerOffset.x;
            y = -Convert.ToSingle(orientation[2]) - managerScript.controllerOffset.y;
            z = Convert.ToSingle(orientation[0]) - managerScript.controllerOffset.z;
            buttons = orientation[3];

            Quaternion rotation = Quaternion.Euler(x, y, z);

            //controller.transform.rotation = rotation * Quaternion.Inverse(managerScript.controllerOffset);
            controller.transform.rotation = rotation;
            handleButtons(buttons);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log(s + "  IndexOutOfRange");
            return;

        }
        catch (FormatException)
        {
            Debug.Log(s + "  FormatException");
            return;
        }
    }

    void handleButtons(string buttons)
    {
        // Fire1
        if (buttons[0] == '1')
        {
            managerScript.fire1 = true;
            fire1 = true;

            if (managerScript.fire1Up) managerScript.fire1Up = false;
        }
        else
        {
            managerScript.fire1 = false;

            if (fire1) managerScript.fire1Up = true;
            else managerScript.fire1Up = false;

            fire1 = false;
        }

        // Select
        if (buttons[1] == '1')
        {
            managerScript.use = true;
            select = true;

            if (managerScript.useUp) managerScript.useUp = false;
        }
        else
        {
            managerScript.use = false;

            if (select) managerScript.useUp = true;
            else managerScript.useUp = false;

            select = false;
        }

        // Submit
        if (buttons[2] == '1')
        {
            managerScript.submit = true;
            submit = true;

            if (managerScript.submitUp) managerScript.submitUp = false;
        }
        else
        {
            managerScript.submit = false;

            if (submit) managerScript.submitUp = true;
            else managerScript.submitUp = false;

            submit = false;
        }
    }
}
