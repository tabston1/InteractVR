using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateText : MonoBehaviour {

    protected GameObject parentObject;
    protected GameObject billboard;
    protected Component textComponent;
    protected GUIText text;
    protected float []scale;
    protected int[] rotation;
    protected int[] worldRotation;

	// Use this for initialization
	void Start () {
        billboard = this.gameObject;
        parentObject = billboard.transform.root.gameObject;
        textComponent = billboard.AddComponent<GUIText>();
        text = textComponent.GetComponent<GUIText>();
        rotation = new int[3];
        worldRotation = new int[3];
        scale = new float[3];
        scale[0] = parentObject.transform.localScale.x;
        scale[1] = parentObject.transform.localScale.y;
        scale[2] = parentObject.transform.localScale.z;
        rotation[0] = 0;
        rotation[1] = 0;
        rotation[2] = 0;
        worldRotation[0] = (int)Mathf.Round(parentObject.transform.localRotation.x);
        worldRotation[1] = (int)Mathf.Round(parentObject.transform.localRotation.y);
        worldRotation[2] = (int)Mathf.Round(parentObject.transform.localRotation.z);
        
        
        //Initialize the text when the object is first instantiated
        text.text = "Object: " + billboard.name + "\n" +
                    "Scale: " + scale + "\n" +
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
        rotation[0] = (int)Mathf.Round(parentObject.transform.rotation.x - worldRotation[0]);
        rotation[1] = (int)Mathf.Round(parentObject.transform.rotation.y - worldRotation[1]);
        rotation[2] = (int)Mathf.Round(parentObject.transform.rotation.z - worldRotation[2]);

    }


    void updateText()
    {
        updateRelativeRotation();
        scale[0] = parentObject.transform.localScale.x;
        scale[1] = parentObject.transform.localScale.y;
        scale[2] = parentObject.transform.localScale.z;

        text.text = "Object: " + billboard.name + "\n" +
                    "Scale: " + scale + "\n" +
                    "    X = " + scale[0] + "\n" +
                    "    Y = " + scale[1] + "\n" +
                    "    Z = " + scale[2] + "\n" +
                    "Rotation: " + "\n" +
                    "    X = " + rotation[0] + "\n" +
                    "    Y = " + rotation[1] + "\n" +
                    "    Z = " + rotation[2] + "\n";

    }

}
