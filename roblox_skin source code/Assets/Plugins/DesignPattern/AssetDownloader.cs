using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Sirenix.OdinInspector;

public class AssetDownloader : MonoBehaviour
{
    public string url = "https://github.com/truongson1510/Girls-Skins-For-Roblox-Cloud/raw/main/Storage/StandaloneWindows/pants";
    public AssetBundle bundle;
    private void Start()
    {
        WWW www = new WWW(url);
        StartCoroutine(WebRequest(www));
    }

    [Button]
    private void Download()
    {
        WWW www = new WWW(url);
        StartCoroutine(WebRequest(www));
    }

    [Button]
    private void Unload()
    {
        if (bundle != null) 
            bundle.Unload(true);
    }

    IEnumerator WebRequest(WWW www)
    {
        yield return www;
        while (www.isDone == false)
        {
            yield return null;
        }

        bundle = www.assetBundle;

        //if (bundle.)
        if (www.error == null)
        {
            /*var obj = (GameObject)bundle.LoadAsset("Level " + levelIndex);
            CurrentLevel = Instantiate(obj, parentMap).GetComponent<LevelManager>();*/

            //var obj = bundle.LoadAsset("Pant data 01");
            //Debug.Log(obj);

            var obj = bundle.LoadAllAssets();

            for(int i = 0; i < obj.Length; i++)
            {
                Debug.Log(obj[i].name);
            }    
            
        }
        else
        {
            Debug.Log(www.error);
        }
    }
}
