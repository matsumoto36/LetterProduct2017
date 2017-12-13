using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// ScriptableObjectでほしい機能を盛り込んだクラス
/// </summary>
public abstract class ScriptableObjectBase : ScriptableObject {


	/// <summary>
	/// アセットを新規作成する
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path">拡張子なしの出力するパス</param>
	protected static void CreateAsset<T>(string path) where T : ScriptableObjectBase {
		AssetDatabase.CreateAsset(CreateInstance<T>(), path + ".asset");
		AssetDatabase.Refresh();
	}

	/// <summary>
	/// アセットをロードする
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="path">拡張子なしのパス</param>
	/// <returns></returns>
	public static T LoadAsset<T>(string path) where T : ScriptableObjectBase {

		var assetData = AssetDatabase.LoadAssetAtPath<T>(path + ".asset");
		if(!assetData) Debug.LogError("Asset : " + path + ".asset is not found!");
		return assetData;
	}
}
