using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using TechTweaking.Bluetooth;
using UnityEngine.UI;

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

    private BluetoothDevice device;
    //public Text statusText;

    void Awake()
    {
//#if !UNITY_EDITOR
        device = new BluetoothDevice();
        connect();

        /* if (BluetoothAdapter.isBluetoothEnabled())
         {
             connect();
         }
         else
         {

             //BluetoothAdapter.enableBluetooth(); //you can by this force enabling Bluetooth without asking the user

             BluetoothAdapter.OnBluetoothStateChanged += HandleOnBluetoothStateChanged;
             BluetoothAdapter.listenToBluetoothState(); // if you want to listen to the following two events  OnBluetoothOFF or OnBluetoothON

             BluetoothAdapter.askEnableBluetooth();//Ask user to enable Bluetooth

         }
         */
//#endif
    }

    // Use this for initialization
    void Start()
    {

        controller = GameObject.FindGameObjectWithTag("Controller");
        manager = GameObject.Find("Manager");
        managerScript = manager.GetComponent<Manager>();

        //#if !UNITY_EDITOR
        BluetoothAdapter.OnDeviceOFF += HandleOnDeviceOff;//This would mean a failure in connection! the reason might be that your remote device is OFF
        BluetoothAdapter.OnDeviceNotFound += HandleOnDeviceNotFound; //Because connecting using the 'Name' property is just searching, the Plugin might not find it!.
                                                                     //#endif

        submit = false;
        fire1 = false;
        select = false;
        /*
        #if UNITY_EDITOR

                stream = new SerialPort("COM3", 9600);
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


        #endif
            }
            */
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

    //############### Reading Data  #####################
    //Please note that you don't have to use this Couroutienes/IEnumerator, you can just put your code in the Update() method
    IEnumerator ManageConnection(BluetoothDevice device)
    {
        while (device.IsReading)
        {
            if (device.IsDataAvailable)
            {
                byte[] msg = device.read();//because we called setEndByte(10)..read will always return a packet excluding the last byte 10.

                if (msg != null && msg.Length > 0)
                {
                    string content = System.Text.ASCIIEncoding.ASCII.GetString(msg);
                    updateController(content);
                }
            }

            yield return null;
        }
    }

    void updateController(string s)
    {
        // rotate the controller
        string[] orientation = s.Split(' ');
        string buttons;

      //  statusText.text = s;

        try
        {
            x = Convert.ToSingle(orientation[0]) - managerScript.controllerOffset.x;
            y = Convert.ToSingle(orientation[1]) - managerScript.controllerOffset.y;
            z = Convert.ToSingle(orientation[2]) - managerScript.controllerOffset.z;
            buttons = orientation[3];

            Quaternion rotation = Quaternion.Euler(x, y, z);

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

    private void connect()
    {

        /* The Property device.MacAdress doesn't require pairing. 
		 * Also Mac Adress in this library is Case sensitive,  all chars must be capital letters
		 
        device.MacAddress = "XX:XX:XX:XX:XX:XX";
        */

        device.Name = "HC-06";
		/* 
		* Trying to identefy a device by its name using the Property device.Name require the remote device to be paired
		* but you can try to alter the parameter 'allowDiscovery' of the Connect(int attempts, int time, bool allowDiscovery) method.
		* allowDiscovery will try to locate the unpaired device, but this is a heavy and undesirable feature, and connection will take a longer time
		*/


        /*
		 * 10 equals the char '\n' which is a "new Line" in Ascci representation, 
		 * so the read() method will retun a packet that was ended by the byte 10. simply read() will read lines.
		 * If you don't use the setEndByte() method, device.read() will return any available data (line or not), then you can order them as you want.
		 */
        device.setEndByte(10);


        /*
		 * The ManageConnection Coroutine will start when the device is ready for reading.
		 */
        device.ReadingCoroutine = ManageConnection;

        device.connect();

    }

    //############### Handlers/Recievers #####################
    void HandleOnBluetoothStateChanged(bool isBtEnabled)
    {
        if (isBtEnabled)
        {
            connect();
            //We now don't need our recievers
            BluetoothAdapter.OnBluetoothStateChanged -= HandleOnBluetoothStateChanged;
            BluetoothAdapter.stopListenToBluetoothState();
        }
    }

    //This would mean a failure in connection! the reason might be that your remote device is OFF
    void HandleOnDeviceOff(BluetoothDevice dev)
    { 
        return;
        /*
        if (!string.IsNullOrEmpty(dev.Name))
        {
            statusText.text = "Status : can't connect to '" + dev.Name + "', device is OFF ";
        }
        else if (!string.IsNullOrEmpty(dev.MacAddress))
        {
            statusText.text = "Status : can't connect to '" + dev.MacAddress + "', device is OFF ";
        }
        */
    }

    //Because connecting using the 'Name' property is just searching, the Plugin might not find it!.
    void HandleOnDeviceNotFound(BluetoothDevice dev)
    {
        return;
    /*
        if (!string.IsNullOrEmpty(dev.Name))
        {
            statusText.text = "Status : Can't find a device with the name '" + dev.Name + "', device might be OFF or not paird yet ";

        }
        */
    }

    public void disconnect()
    {
        if (device != null)
            device.close();
    }

    //############### Deregister Events  #####################
    void OnDestroy()
    {
        BluetoothAdapter.OnDeviceOFF -= HandleOnDeviceOff;
        BluetoothAdapter.OnDeviceNotFound -= HandleOnDeviceNotFound;

    }
}
