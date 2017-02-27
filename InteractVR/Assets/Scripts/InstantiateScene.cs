using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateScene : MonoBehaviour {

    UnityEngine.WWW www;
    string []names;

    /*
    void Start()
    {
     
    }
    */
    IEnumerator Start()
    {
        Debug.Log("Outside");
        // GameObject l = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        // Instantiate(l);

        www = UnityEngine.WWW.LoadFromCacheOrDownload("https://s3.amazonaws.com/immersacad-storage/demos/mars/5846ce4637518_Android.unity3d", 0, 0);
        //www = new WWW("https://s3.amazonaws.com/immersacad-storage/demos/mars/5846ce4637518_Android.unity3d");
        yield return www;
        if (www == null)
        {
            Debug.Log("Didnt load anything.");
        }
        names = www.assetBundle.GetAllAssetNames();
        foreach (string element in names)
        {
            Debug.Log(element);
        }

        if (Application.platform == RuntimePlatform.Android)
        {
           


            if (www != null)
            {
                
            }
            //Instantiate(www.assetBundle.mainAsset, new Vector3(0, 45, 0), new Quaternion(0, 0, 0, 0));
            
            //www = new WWW("https://s3.amazonaws.com/immersacad-storage/demos/mars/5846ce4637518_Android.unity3d");
            //yield return www;
            //Object[] mars;
            //mars = www.assetBundle.LoadAllAssets();
            //foreach(Object n in mars)
           // {
             //   Instantiate(n);
          //  }
            
            // Instantiate(www.assetBundle.LoadAllAssets());
        }
        
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
