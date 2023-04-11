
using System;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundle
{
	#region Inspector Variables

	#endregion

	#region Member Variables

	#endregion

	#region Properties

	#endregion

	#region Unity Methods

	#endregion

	#region Public Methods

	#endregion

	#region Protected Methods

	#endregion

	#region Private Methods

	[UnityEditor.MenuItem("Assets/Build Asset Bundles")]
	private static void BuildAllAssetBundles()
	{
		string assetBundleDirectoryPath = Application.dataPath + "/AssetsBundlesBuild";
        try
        {
			Debug.Log("Build");
			BuildPipeline.BuildAssetBundles(assetBundleDirectoryPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        }
		catch (Exception e)
        {
			Debug.LogWarning(e);
        }

	}

	#endregion
}
