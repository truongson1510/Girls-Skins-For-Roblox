
using System;
using System.IO;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine.Networking;
using System.Collections.Generic;

public class AssetBundleManager : Singleton<AssetBundleManager>
{
    #region Inspector Variables

    public string IOSFolderUrl;
    public string AndroidFolderUrl;

    public AssetBundleData pantsBundle;
    public AssetBundleData shirtsBundle;

    #endregion

    #region Member Variables

    #endregion

    #region Properties

    #endregion

    #region Unity Methods

    private void Start()
    {
    #if UNITY_ANDROID
        var folderUrl = $"{AndroidFolderUrl}";
    #elif UNITY_IOS
        var folderUrl = $"{IOSFolderUrl}";
    #else
            
    #endif

        pantsBundle.url     = $"{folderUrl}/{pantsBundle.name}";
        shirtsBundle.url    = $"{folderUrl}/{shirtsBundle.name}";

        StartCoroutine(pantsBundle.DownloadAsset());
        StartCoroutine(shirtsBundle.DownloadAsset());

        StartCoroutine(WaitingToExecute(() => { SceneAutoLoader.Instance.StartLoadingAsset(); }));
    }

    private void OnDestroy()
    {
        if (pantsBundle.bundle != null)
            pantsBundle.UnloadAsset();
        if (shirtsBundle.bundle != null)
            shirtsBundle.UnloadAsset();
    }

    #endregion

    #region Public Methods

    #endregion

    #region Protected Methods

    #endregion

    #region Private Methods

    private IEnumerator WaitingToExecute(Action onCompleteAction = null)
    {
        while(!pantsBundle.isDownloadComplete || !shirtsBundle.isDownloadComplete)
        {
            yield return null;
        }
        onCompleteAction?.Invoke();
    }

    #endregion
}

[Serializable]
public class AssetBundleData
{
    public string name;
    [HideInInspector] public string         url;
    [HideInInspector] public AssetBundle    bundle;
    [HideInInspector] public List<ItemData> dataItems;
    [HideInInspector] public bool           isDownloadComplete;

    public IEnumerator DownloadAsset()
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();

        while (!www.isDone)
        {
            yield return null;
        }
        isDownloadComplete = true;

        if (www.result == UnityWebRequest.Result.Success)
        {
            bundle = DownloadHandlerAssetBundle.GetContent(www);
            var obj = bundle.LoadAllAssets();

            dataItems = new List<ItemData>();
            for (int i = 0; i < obj.Length; i++)
            {
                dataItems.Add((ItemData)obj[i]);
            }
        }
        else
        {
            Debug.LogError("Failed to download Asset Bundle: " + www.error);
        }
    }

    public void UnloadAsset()
    {
        if (bundle != null)
            bundle.Unload(true);
    }
}
