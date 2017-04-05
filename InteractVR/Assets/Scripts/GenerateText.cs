using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateText : MonoBehaviour {

    protected GameObject parentObject;
    protected GameObject billboard;
    protected GameObject information;
    protected Text text;
    protected float []scale;
    protected float[] rotation;
    protected int[] worldRotation;
    protected string buildNo;

	// Use this for initialization
	void Start () {
        billboard = this.transform.parent.gameObject;
        parentObject = billboard.transform.root.gameObject;
        information = this.gameObject;
        text = information.AddComponent<Text>();
        buildNo = parentObject.GetComponent<BasicObject>().buildNo;

        //Set the text attributes
        text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        text.color = Color.black;
        text.fontSize = 2;

        //initialize the default scale and rotation values
        rotation = new float[3];
        worldRotation = new int[3];
        scale = new float[3];
        scale[0] = parentObject.transform.localScale.x;
        scale[1] = parentObject.transform.localScale.y;
        scale[2] = parentObject.transform.localScale.z;
        rotation[0] = 0;
        rotation[1] = 0;
        rotation[2] = 0;
        worldRotation[0] = (int)parentObject.transform.localRotation.x;
        worldRotation[1] = (int)parentObject.transform.localRotation.y;
        worldRotation[2] = (int)parentObject.transform.localRotation.z;
        
        
        //Initialize the text when the object is first instantiated
        text.text = "Object: " + buildNo + "\n" +
                    "Scale: " + "\n" +
                    "    X = " + scale[0] + "\n" +
                    "    Y = " + scale[1] + "\n" +
                    "    Z = " + scale[2] + "\n" +
                    "Rotation: " + "\n" +
                    "    X = " + rotation[0] + "\n" +
                    "    Y = " + rotation[1] + "\n" +
                    "    Z = " + rotation[2] + "\n";
       

    }
	
    void updateRelativeRotation()
    {
        //rotation[0] = (int)parentObject.transform.localRotation.x - worldRotation[0];
        //rotation[1] = (int)parentObject.transform.localRotation.y - worldRotation[1];
        //rotation[2] = (int)parentObject.transform.localRotation.z - worldRotation[2];
       
    }


    public void updateText()
    {
        
        rotation[0] = parentObject.transform.rotation.x;
        rotation[1] = parentObject.transform.rotation.y;
        rotation[2] = parentObject.transform.rotation.z;

        scale[0] = parentObject.transform.localScale.x;
        scale[1] = parentObject.transform.localScale.y;
        scale[2] = parentObject.transform.localScale.z;

        text.text = "Object: " + billboard.name + "\n" +
                    "Scale: " + "\n" +
                    "    X = " + scale[0] + "\n" +
                    "    Y = " + scale[1] + "\n" +
                    "    Z = " + scale[2] + "\n" +
                    "Rotation: " + "\n" +
                    "    X = " + rotation[0] + "\n" +
                    "    Y = " + rotation[1] + "\n" +
                    "    Z = " + rotation[2] + "\n";

        Debug.Log("updating text");
    }

}
