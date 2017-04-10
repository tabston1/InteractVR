using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityController : MonoBehaviour
{
	public GameObject emptyParentContainer;

	//Object associated with the billboard and wrapper script for object models
	private GameObject obj;
	private BasicObject objScript;

	//Billboard on which this button is placed
	private GameObject billboard;

	//Reference to this button's image/icon
	private Image buttonIcon;

	//Sprites to represent enabled/disabled gravity for this object
	private Sprite gravityEnabledSprite;
	private Sprite gravityDisabledSprite;




	// Use this for initialization
	void Start ()
	{
		/*
		//Current hierarchy: this button -> Slot (grid layout) -> Billboard -> GameObject being manipulated
		if (this.transform.parent != null) {
			//billboard = this.transform.parent.gameObject;
			billboard = this.transform.parent.transform.parent.gameObject;

			if (billboard.transform.parent != null) {
				obj = billboard.transform.parent.gameObject;

				//Grab reference to this object's object wrapper script
				objScript = obj.GetComponent<BasicObject> ();
			}
		}
		*/

		//Current hierarchy: this button -> Slot (grid layout) -> Billboard -> Empty parent object wrapper
		//Empty parent object wrapper has 2 children: billboard and model object
		billboard = transform.parent.transform.parent.gameObject;

		if (billboard != null) {
			emptyParentContainer = billboard.transform.parent.gameObject;

			if (emptyParentContainer != null) {
				obj = emptyParentContainer.transform.GetChild (0).gameObject;

				if (obj != null) {
					//Grab reference to this object's object wrapper script
					objScript = obj.GetComponent<BasicObject> ();
					if (objScript == null)
						Debug.Log ("Could not grab basic object script for " + obj.name + " from button " + name);
				} else
					Debug.Log ("Could not grab model object reference from " + name);
			} else
				Debug.Log ("Could not grab emptyParentContainer reference from " + name);
		} else
			Debug.Log ("Could not grab billboard reference from " + name);


		//Grab references to the 2 sprites for when gravity is disabled or enabled
		gravityDisabledSprite = Resources.Load<Sprite> ("Buttons/Gravity_Disabled");
		gravityEnabledSprite = Resources.Load<Sprite> ("Buttons/Gravity_Enabled");

		//Grab reference to the Image component of this button
		buttonIcon = gameObject.GetComponent<Image> ();

		if (buttonIcon == null) {
			Debug.Log ("This button does not have an Image component attached");
		}

		if (gravityDisabledSprite == null || gravityEnabledSprite == null) {
			Debug.Log ("Ensure 2 sprites are in the Resources/Buttons folder named 'Gravity_Disabled' and 'Gravity_Enabled'");
		}
	}

	void onClick ()
	{
		if (obj != null) {
			//Disable gravity if it is currently enabled (will actually take effect on toolbar/billboard close)
			if (objScript.gravityOn) {
				buttonIcon.sprite = gravityDisabledSprite;
				objScript.gravityOn = false;
			} 
		//Enable gravity if it is currently enabled (will actually take effect on on toolbar/billboard close)
		else {
				buttonIcon.sprite = gravityEnabledSprite;
				objScript.gravityOn = true;
			}
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
