using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GravityController : MonoBehaviour
{
	//Object associated with the billboard
	private GameObject obj;

	//Billboard on which this button is placed
	private GameObject billboard;

	//Reference to this button's image/icon
	private Image buttonIcon;

	//Sprites to represent enabled/disabled gravity for this object
	private Sprite gravityEnabledSprite;
	private Sprite gravityDisabledSprite;

	//Boolean representing whether gravity is enabled/disabled for this object
	public bool gravityEnabled = false;


	// Use this for initialization
	void Start ()
	{
		//Current hierarchy: this button -> Slot (grid layout) -> Billboard -> GameObject being manipulated
		if (this.transform.parent != null) {
			//billboard = this.transform.parent.gameObject;
			billboard = this.transform.parent.transform.parent.gameObject;

			if (billboard.transform.parent != null) {
				obj = billboard.transform.parent.gameObject;
			}
		}

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
		//Disable gravity if it is currently enabled (will actually take effect on toolbar/billboard close)
		if (gravityEnabled) {
			buttonIcon.sprite = gravityDisabledSprite;
			gravityEnabled = false;
		} 
		//Enable gravity if it is currently enabled (will actually take effect on on toolbar/billboard close)
		else {
			buttonIcon.sprite = gravityEnabledSprite;
			gravityEnabled = true;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
