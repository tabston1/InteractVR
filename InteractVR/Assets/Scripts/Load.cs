using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Load : MonoBehaviour {

    protected FileStream stream;
    protected StreamReader reader;
    protected GameObject manager;
    protected string line;
    protected string[] text;
    protected string buildNo;
    protected float[] itemPosition;
    protected float[] itemRotation;
    protected float[] itemScale;
    protected InstantiateObject newObj;
    protected GameObject obj;

    void onClick()
    {
        //System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
        //reader = new StreamReader(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/SavedScene.txt");
        reader = new StreamReader(Application.persistentDataPath + "/SavedScene.txt");
        manager = GameObject.FindGameObjectWithTag("Manager");
        itemPosition = new float[3];
        itemRotation = new float[3];
        itemScale = new float[3];
        StartCoroutine(readData());
    }

    IEnumerator readData()
    {
        Debug.Log("Loading");
        while (reader.Peek() != -1)
        {
            line = reader.ReadLine();
            text = line.Split(' ');
            buildNo = text[0];
            itemPosition[0] = float.Parse(text[1]);
            itemPosition[1] = float.Parse(text[2]);
            itemPosition[2] = float.Parse(text[3]);
            itemRotation[0] = float.Parse(text[4]);
            itemRotation[1] = float.Parse(text[5]);
            itemRotation[2] = float.Parse(text[6]);
            itemScale[0] = float.Parse(text[7]);
            itemScale[1] = float.Parse(text[8]);
            itemScale[2] = float.Parse(text[9]);

            Debug.Log(itemPosition[1]);

            newObj = (InstantiateObject)manager.GetComponent(typeof(InstantiateObject));
            newObj.addObject(buildNo);
            while (newObj.returnedObj == null) {
                yield return null;
            };
            obj = newObj.returnedObj;
            obj.transform.position = new Vector3(itemPosition[0], itemPosition[1], itemPosition[2]);
            obj.transform.eulerAngles = new Vector3(itemRotation[0], itemRotation[1], itemRotation[2]);
            obj.transform.localScale = new Vector3(itemScale[0], itemScale[1], itemScale[2]);


            //foreach (string word in text)
            //{
            //    Debug.Log(word);
            //}

        }
        reader.Close();

    }	
}


