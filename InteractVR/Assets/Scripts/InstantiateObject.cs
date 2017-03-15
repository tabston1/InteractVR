using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstantiateObject : MonoBehaviour {

    Dictionary<string, GameObject> Objects;
    UnityEngine.WWW www;
    GameObject[] sceneObjects;
    private GameObject newObj { get; set; }
    private GameObject Camera { get; set; }

    void Awake()
    {
        Objects = new Dictionary<string, GameObject>();
        Camera = GameObject.Find("Main Camera");
    }

    public void loadObject(string buildNo)
    {
        Debug.Log("loadObject");
        StartCoroutine(Loading(buildNo));
    }
    IEnumerator Loading(string buildNo)
    {
        string asset;
        Scene newScene;

        //Wait until the cache is ready to be used
        while (!UnityEngine.Caching.ready)
        {
            yield return null;
        }

        asset = "https://s3.amazonaws.com/immersacad-storage/demos/utk/" + buildNo + "_Android.unity3d";

        //Pull the asset bundle from either the cache or the web.
        www = UnityEngine.WWW.LoadFromCacheOrDownload(asset, 0, 0);

        //Wait for the model to load into the scene before continuing
        while (!www.isDone)
        {
            yield return null;
        }

        //Instantiates the asset bundle that was downloaded
        if (www != null)
        {
            UnityEngine.AssetBundle bundle = www.assetBundle;
        }

        //Loads the scene using the Build Number
        SceneManager.LoadScene(buildNo, LoadSceneMode.Additive);

        //Grabs the Object from the scene and makes the objects in the created scene invisible
        newScene = SceneManager.GetSceneByName(buildNo);
        while (!newScene.isLoaded)
        {
            yield return null;
        }
        sceneObjects = newScene.GetRootGameObjects();
        sceneObjects[0].SetActive(false);

        newObj = Instantiate(sceneObjects[0]);
        newObj.SetActive(false);

        //Remove the scene that was created for the object
        SceneManager.UnloadSceneAsync(buildNo);
        newScene = SceneManager.GetSceneByName(buildNo);
        while (newScene.isLoaded)
        {
            yield return null;
        }

        //Add it to the dictionary for future uses and make the object visible to the user.
        Objects.Add(buildNo, newObj);
        newObj.SetActive(true);
        newObj.transform.position = Camera.transform.position + (5 * Camera.transform.forward);
        AddCollider(newObj);
        addShader(newObj);

    }


    public void addObject(string buildNo)
    {
        GameObject current;
        GameObject main;

        main = GameObject.FindGameObjectWithTag("MainCamera");

        //If the object is already in the dictionary, grab it so it can be instantiated
        if (Objects.ContainsKey(buildNo))
        {
            current = (GameObject)Objects[buildNo];
            current = Instantiate(current);
            current.transform.position = Camera.transform.position + (5 * Camera.transform.forward);
            AddCollider(current);
            addShader(current);
        }

        //Else load the scene with the object in it, remove the object from the scene and add it to the dictionary
        else
        {
            loadObject(buildNo);
        }

    }

    //Removes colliders from the child objects making up the object and adds just one to the parent object
    public void AddCollider(GameObject obj)
    {
        Mesh objMesh;
        MeshFilter[] childrenMesh;
        MeshCollider objMeshCollider;
        MeshCollider[] children;
        children = obj.GetComponentsInChildren<MeshCollider>();

        //Adds a single mesh collider to the parent object
        objMeshCollider = obj.GetComponent<MeshCollider>();
        if (objMeshCollider == null) {
            objMeshCollider = obj.AddComponent<MeshCollider>();
            objMesh = obj.GetComponent<Mesh>();
            if (objMesh == null)
            {
                //Search children of object for a mesh if one is not found on the parent
                childrenMesh = obj.GetComponentsInChildren<MeshFilter>();
                objMeshCollider.sharedMesh = childrenMesh[0].mesh;
               
            }
            obj.AddComponent<MeshRenderer>();
            
            //Removes mesh colliders from the children of the main object
            foreach (MeshCollider element in children)
            {
                if (element != null)
                {
                    Destroy(element);
                }
            }
        }  
    }

    //Adds a standard shader to all of the components of the Immersafied Object
    public void addShader(GameObject obj)
    {

        Renderer[] objRenderers;
        Material[] objMaterials;

        objRenderers = obj.GetComponentsInChildren<Renderer>();

        foreach(Renderer element in objRenderers)
        {
            objMaterials = element.materials;
            foreach(Material mat in objMaterials)
            {
                mat.shader = Shader.Find("Standard");
            }
        }


    }
}
