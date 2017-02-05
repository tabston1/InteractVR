using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour {

    private GameObject camera;
    private GameObject selectedObject;
    private Transform cameraTrans;

    [Tooltip("Maximum gaze distance, in meters, for calculating a hit.")]
    public float MaxGazeDistance = 40.0f;

    [Tooltip("Select the layers raycast should target.")]
    public LayerMask RaycastLayerMask = Physics.DefaultRaycastLayers;
    public bool Hit { get; private set; }
    public RaycastHit HitInfo { get; private set; }

    /// Position of the intersection of the user's gaze and the object in the scene.
    public Vector3 Position { get; private set; }

    /// RaycastHit Normal direction.
    public Vector3 Normal { get; private set; }
    public GameObject FocusedObject { get; private set; }

    private Vector3 gazeOrigin;
    private Vector3 gazeDirection;
    private Quaternion gazeRotation;
    private float lastHitDistance = 15.0f;
   
    // Finds the description child of an object and makes the description appear
    void OnSelect()
    {
        if (selectedObject != null)
        {
            Deselect();
        }

        selectedObject = HitInfo.transform.gameObject;
        //Enable the description for the object
        foreach (Transform child in HitInfo.transform)
        {
           if (child.CompareTag("Description"))
            { 
                child.gameObject.SetActive(true);
            }
        }
    }

    //Disables an active description for a child, making it disappear.
    void Deselect()
    {
        if (selectedObject == null) return;
        foreach (Transform child in selectedObject.transform)
        {
            if (child.CompareTag("Description"))
            {
                child.gameObject.SetActive(false);
            }
        }
        selectedObject = null;
    }


    private void UpdateRaycast()
    {
        // Get the raycast hit information from Unity's physics system.
        RaycastHit hitInfo;
        GameObject oldFocusedObject = FocusedObject;

        Hit = Physics.Raycast(gazeOrigin, gazeDirection, out hitInfo, MaxGazeDistance, RaycastLayerMask);

        // Update the HitInfo property so other classes can use this hit information.
        HitInfo = hitInfo;

        if (Hit)
        {
            // If the raycast hits a hologram, set the position and normal to match the intersection point.
            Position = hitInfo.point;
            Normal = hitInfo.normal;
            lastHitDistance = hitInfo.distance;
            FocusedObject = hitInfo.collider.gameObject;
        }
        else
        {
            // If the raycast does not hit a hologram, default the position to last hit distance in front of the user,
            // and the normal to face the user.
            Position = gazeOrigin + (gazeDirection * lastHitDistance);
            Normal = -gazeDirection;
            FocusedObject = null;
        }

        // Check if the currently hit object has changed
        if (oldFocusedObject != FocusedObject)
        {
            if (oldFocusedObject != null)
            {
                OnGazeLeave(oldFocusedObject);
            }
            if (FocusedObject != null)
            {
                OnGazeEnter(FocusedObject);
            }
        }
    }

    private void OnGazeEnter(GameObject FocusedObject)
    {
        FocusedObject.GetComponent<Renderer>().material.shader = Shader.Find("Self-Illumin/Outlined Diffuse");
    }

    private void OnGazeLeave(GameObject OldFocusedObject)
    {
        OldFocusedObject.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
    }


    // Use this for initialization
    void Start () {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraTrans = camera.GetComponent<Transform>();
        selectedObject = null;
    }

    void Update()
    {
        gazeOrigin = Camera.main.transform.position;
        gazeDirection = Camera.main.transform.forward;
        gazeRotation = Camera.main.transform.rotation;

        UpdateRaycast();
        
        //Used to make the information about an object pop up
        if (Input.GetButtonDown("Jump"))
        {
            if (Hit)
            {
                OnSelect();  
            }
            else
            {
                Deselect();
            }
        }

    }

}
