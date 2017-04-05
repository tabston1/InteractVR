using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

//Loads a scene from a Unity Asset which contains a single object
public class LoadScene: MonoBehaviour {
    UnityEngine.WWW www;
    GameObject[] sceneObjects;

    /*
    void addIDs()
    {
        models.Add("58b3381e12d47");    //Modern Bed
        models.Add("58b33868081ef");    //Computer Desk
        models.Add("58b338d5eb081");    //Glass Table
        models.Add("58b339ae52366");    //Sofa
        models.Add("58b33a3f36d58");    //Simple Empty House
        models.Add("58b33caa845ad");    //Modern Table
    }
    */

    void Start()
    {
        loadObject("58b3381e12d47");
    }


    public void loadObject(string buildNo)
    {
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
            //UnityEngine.AssetBundle bundle = www.assetBundle;
        }

        //Loads the scene using the Build Number
        SceneManager.LoadScene(buildNo, LoadSceneMode.Additive);

    }
}
