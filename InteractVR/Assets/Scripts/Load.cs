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
    protected float[] parentPosition;
    protected float[] parentRotation;
    protected float[] parentScale;
    protected float[] childPosition;
    protected float[] childRotation;
    protected float[] childScale;
    protected InstantiateObject newObj;
    protected GameObject obj;
    protected GameObject model;
    protected string path;

    void onClick()
    {
        //System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)
        //reader = new StreamReader(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/SavedScene.txt");
        reader = new StreamReader(Application.persistentDataPath + "/SavedScene.txt");
        manager = GameObject.FindGameObjectWithTag("Manager");
        parentPosition = new float[3];
        parentRotation = new float[3];
        parentScale = new float[3];
        childPosition = new float[3];
        childRotation = new float[3];
        childScale = new float[3];
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

            //Stores the parent and child object positions, rotations, and scales read in from the save file
            parentPosition[0] = float.Parse(text[1]);
            parentPosition[1] = float.Parse(text[2]);
            parentPosition[2] = float.Parse(text[3]);
            parentRotation[0] = float.Parse(text[4]);
            parentRotation[1] = float.Parse(text[5]);
            parentRotation[2] = float.Parse(text[6]);
            parentScale[0] = float.Parse(text[7]);
            parentScale[1] = float.Parse(text[8]);
            parentScale[2] = float.Parse(text[9]);
            childPosition[0] = float.Parse(text[10]);
            childPosition[1] = float.Parse(text[11]);
            childPosition[2] = float.Parse(text[12]);
            childRotation[0] = float.Parse(text[13]);
            childRotation[1] = float.Parse(text[14]);
            childRotation[2] = float.Parse(text[15]);
            childScale[0] = float.Parse(text[16]);
            childScale[1] = float.Parse(text[17]);
            childScale[2] = float.Parse(text[18]);

            //Waits for the model to load in and then sets the parent object and its child object accordingly.
            model = Resources.Load("Prefabs/" + buildNo) as GameObject;
            while (model == null)
            {
                yield return null;
            }
            obj = Instantiate(model);
            obj.transform.position = new Vector3(parentPosition[0], parentPosition[1], parentPosition[2]);
            obj.transform.eulerAngles = new Vector3(parentRotation[0], parentRotation[1], parentRotation[2]);
            obj.transform.localScale = new Vector3(parentScale[0], parentScale[1], parentScale[2]);
            foreach (Transform child in obj.transform)
            {
                if (child.tag == "Movable")
                {
                    child.position = new Vector3(childPosition[0], childPosition[1], childPosition[2]);
                    child.eulerAngles = new Vector3(childRotation[0], childRotation[1], childRotation[2]);
                    child.localScale = new Vector3(childScale[0], childScale[1], childScale[2]);
                }
            }
         
        }
        reader.Close();

    }	
}


