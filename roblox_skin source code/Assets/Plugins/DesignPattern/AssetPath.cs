using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

[CreateAssetMenu]
public class AssetPath : ScriptableObject
{
    public bool isLocal;

    [ValueDropdown("Root")]
    public string rootFolder;
    private static string[] Root = new string[] { "Music", "Backgrounds", "Animator", "CustomLevel" };

    [ShowIf("isLocal")]
    public string localPath;

    [InfoBox("$GetName")]
    [HideIf("isLocal")]
    public string remotePath;

    [ReadOnly]
    [HideIf("isLocal")]
    [ShowInInspector]
    private const string RemotePathAndroid = "https://github.com/atsoft-io/FNF-Saber-Server/raw/main/Android/";
    [ReadOnly]
    [HideIf("isLocal")]
    [ShowInInspector]
    private const string RemotePathIos = "https://github.com/atsoft-io/FNF-Saber-Server/raw/main/Ios/";
    [Range(0,1)]
    public float downloadPercent;
    
    [Button]
    public void Download()
    {
        Download(delegate(bool b) { Debug.Log("Save: "+b); }, f => Debug.Log(f));
    }

    
    string LocalPath
    {
        get
        {
            return "AssetBundles/";
        }
    }

    string GetName()
    {
        if (String.IsNullOrEmpty(remotePath))
        {
            return "null";
        }
        int index = remotePath.LastIndexOf("/", StringComparison.Ordinal);
        return remotePath.Remove(0,index+1);
    }
    public T LoadObject<T>(string customNameBundle = "") where T : Object
    {
        if (isLocal)
        {
            return Resources.Load<T>(rootFolder+"/"+localPath);
        }
        else
        {
            string bundleName = customNameBundle;
            if (customNameBundle == "")
            {
                int index = remotePath.LastIndexOf("/", StringComparison.Ordinal);
                bundleName = remotePath.Remove(0,index+1);
            }
            string path = FileHelper.GetWritablePath(LocalPath) +rootFolder+"/"+ remotePath;
            var bundle = AssetBundle.LoadFromFile(path);
            var asset = bundle.LoadAsset<T>(bundleName);
            bundle.Unload(false);
            return asset;
        }
    }    
    public AsyncOperation LoadObjectAsync<T>(Action<T> onLoaded,string customNameBundle = "") where T : Object
    {
        if (isLocal)
        {
            var async = Resources.LoadAsync<T>(rootFolder+"/"+localPath);
            async.completed += delegate(AsyncOperation operation)
            {
                onLoaded?.Invoke((T)async.asset); 
            };
            return async;
        }
        else
        {
            string bundleName = customNameBundle;
            if (customNameBundle == "")
            {
                bundleName = remotePath;
            }
            string path = FileHelper.GetWritablePath(LocalPath) +rootFolder+"/"+ remotePath;
            var bundle = AssetBundle.LoadFromFile(path);
            var asset = bundle.LoadAssetAsync<T>(bundleName);
            asset.completed += delegate(AsyncOperation operation)
            {
                onLoaded?.Invoke((T)asset.asset);
                bundle.Unload(false);
            };
            return asset;
        }
    }

    public bool IsDownloaded()
    {
        if (isLocal)
        {
            return true;
        }
        else
        {
            var filePath = FileHelper.GetWritablePath(LocalPath) +rootFolder+"/"+ remotePath;
            return System.IO.File.Exists(filePath);
        }
    }
    public void Download(Action<bool> complete,Action<float> onUpdate)
    {
#if UNITY_ANDROID
        string path = RemotePathAndroid+rootFolder + "/" + remotePath;

#endif
#if UNITY_IOS
        string path = RemotePathIos + rootFolder + "/" + remotePath;
#endif

        var filePath = FileHelper.GetWritablePath(LocalPath) + rootFolder + "/" + remotePath;
        if (System.IO.File.Exists(filePath))
        {
            complete?.Invoke(true);
            onUpdate?.Invoke(1);
        }
        else
        {
            Action<bool, object> func = (success, data) =>
            {
                if (success)
                {
                    Debug.Log("AssetBundleManager > DownloadAsset_WWW : download success");
                    try
                    {
                        FileHelper.SaveFile((byte[]) data, filePath, true);
                        Debug.Log("save ok: " + filePath);

                        onUpdate?.Invoke(1);
                        complete?.Invoke(true);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("AssetBundleManager > DownloadAsset_WWW : Failed to download asset " +
                                       e.Message);
                    }
                }
                else
                {
                    onUpdate?.Invoke(1);
                    complete?.Invoke(false);
                }
            };

             // AssetBundleManager.Instance.StartDownloadAsset(path,func,onUpdate);

            // AssetBundleManager.Instance.StartCoroutine(Download(path, func, onUpdate));
            // EditorCoroutineUtility.StartCoroutine(Download(path, func, onUpdate),this);
        }
    }

    private IEnumerator Download(string url, Action<bool, object> onResult,Action<float> onUpdate)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            AsyncOperation request = webRequest.SendWebRequest();

            while (!request.isDone)
            {
                Debug.Log(request.progress);
                onUpdate?.Invoke(request.progress);
                downloadPercent = request.progress;
                yield return null;
            }
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                onResult?.Invoke(false, null);
            }
            else
            {
                onResult?.Invoke(true, webRequest.downloadHandler.data);
            }
        }
    }

}
