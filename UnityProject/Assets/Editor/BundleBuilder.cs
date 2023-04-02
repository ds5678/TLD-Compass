using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public sealed class BundleBuilder : EditorWindow
{
	string targetDirectory = "AssetBundles";
	string fileExtension = ".unity3d";
	BuildAssetBundleOptions buildFlags = BuildAssetBundleOptions.ForceRebuildAssetBundle;
	BuildTarget buildTarget = BuildTarget.StandaloneWindows;
	bool logAssetPaths = false;
	bool buildSettingsGroupEnabled = true;
	bool assetBundlesGroupEnabled = true;
	bool[] bundleBuildMarkers;

	[MenuItem("Modding/Asset Bundle Builder")]
	public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(BundleBuilder));
	}
	
	void OnGUI()
	{
		GUILayout.Label ("Build Asset Bundles", EditorStyles.boldLabel);

		buildSettingsGroupEnabled = EditorGUILayout.Foldout(buildSettingsGroupEnabled, "Build Settings");
		if (buildSettingsGroupEnabled)
		{
			EditorGUI.indentLevel++;
			targetDirectory = EditorGUILayout.TextField("Target Directory", targetDirectory);
			fileExtension = EditorGUILayout.TextField("File Extension", fileExtension);
			buildFlags = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("Build Flags", buildFlags);
			buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", buildTarget);
			logAssetPaths = EditorGUILayout.Toggle("Log Asset Paths", logAssetPaths);
			EditorGUI.indentLevel--;
		}

		string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
		if (bundleBuildMarkers == null || bundleBuildMarkers.Length != assetBundleNames.Length)
		{
			bundleBuildMarkers = new bool[assetBundleNames.Length];
		}

		assetBundlesGroupEnabled = EditorGUILayout.Foldout(assetBundlesGroupEnabled, "Asset Bundles");
		if (assetBundlesGroupEnabled)
		{
			EditorGUI.indentLevel++;
			for (int i = 0; i < assetBundleNames.Length; i++)
			{
				string eachAssetBundleName = assetBundleNames[i];
				bundleBuildMarkers[i] = Toggle(eachAssetBundleName, bundleBuildMarkers[i]);
			}
			EditorGUI.indentLevel--;
		}

		if (Button("Build Asset Bundles"))
		{
			if (!Directory.Exists(targetDirectory))
			{
				Directory.CreateDirectory(targetDirectory);
			}

			List<AssetBundleBuild> buildList = new List<AssetBundleBuild>(assetBundleNames.Length);

			for (int i = 0; i < assetBundleNames.Length; i++)
			{
				string eachAssetBundleName = assetBundleNames[i];
				if (bundleBuildMarkers[i])
				{
					AssetBundleBuild build = new AssetBundleBuild
					{
						assetBundleName = eachAssetBundleName.EndsWith(fileExtension, System.StringComparison.Ordinal)
							? eachAssetBundleName
							: eachAssetBundleName + fileExtension,
						assetNames = AssetDatabase.GetAssetPathsFromAssetBundle(eachAssetBundleName)
					};

					if (logAssetPaths)
					{
						Debug.Log("Asset Bundle '" + eachAssetBundleName + "' has assets with paths:");
						foreach (string eachAssetPath in build.assetNames)
						{
							Debug.Log("  " + eachAssetPath);
						}
					}

					buildList.Add(build);
				}
			}

			if (buildList.Count > 0)
			{
				BuildPipeline.BuildAssetBundles(targetDirectory, buildList.ToArray(), buildFlags, buildTarget);
			}
			else
			{
				Debug.LogWarning("No asset bundle has been selected to build.");
			}
		}
	}

	private static bool Button(string label)
	{
		Rect r = EditorGUILayout.BeginHorizontal("Button");
		bool result = GUI.Button(r, GUIContent.none);
		GUILayout.Label(label);
		EditorGUILayout.EndHorizontal();
		return result;
	}

	/// <summary>
	/// Makes a left-sided toggle with unlimited space on the right
	/// </summary>
	/// <param name="label"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	private static bool Toggle(string label, bool value)
	{
		bool result = EditorGUILayout.BeginToggleGroup(label, value);
		EditorGUILayout.EndToggleGroup();
		return result;
	}
}