using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LoadScene: MonoBehaviour {
    UnityEngine.WWW www;
    protected List<string> models { get; set; }
    protected int numModels { get; set; }

    void Start()
    {
        models = new List<string>();
        addIDs();
        numModels = 6;
        StartCoroutine(Loading());
    }

    void addIDs()
    {
        models.Add("58b3381e12d47");    //Modern Bed
        models.Add("58b33868081ef");    //Computer Desk
        models.Add("58b338d5eb081");    //Glass Table
        models.Add("58b339ae52366");    //Sofa
        models.Add("58b33a3f36d58");    //Simple Empty House
        models.Add("58b33caa845ad");    //Modern Table
    }

    IEnumerator Loading()
    {
        string asset;
        int i;

        for (i = 0; i < numModels; i++)
        {

            //Wait until the cache is ready to be used
            while (!UnityEngine.Caching.ready)
            {
                yield return null;
            }

            asset = "https://s3.amazonaws.com/immersacad-storage/demos/utk/" + models[i] + "_Android.unity3d";
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
                //Instantiate(www.assetBundle, new Vector3 (0, i, 0), new Quaternion(0,0,0,0));
                //UnityEngine.AssetBundle bundle = www.assetBundle;
            }

            //Loads the scene using the Build Number
            SceneManager.LoadScene(models[i], LoadSceneMode.Additive);
        }
    }
}
