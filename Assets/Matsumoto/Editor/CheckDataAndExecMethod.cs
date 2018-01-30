using UnityEngine;
using UnityEditor;

/// <summary>
/// コンパイルが走ったときに何かしたいときに実行される機能を提供(エディタのみ)
/// </summary>
[InitializeOnLoad]
public class CheckDataAndExecMethod : Editor {

	static CheckDataAndExecMethod() {

		if(EditorApplication.isPlayingOrWillChangePlaymode)
			return;

		EditorApplication.delayCall += () => {
			CheckGameBalanceData();
		};
	}

	static void CheckGameBalanceData() {

		if(AssetDatabase.LoadAssetAtPath<GameBalanceData>("Assets/Resources/" + GameBalanceData.ASSET_PATH + ".asset")) return;

		Debug.Log("Create GameBalanceData.");

		var asset = CreateInstance<GameBalanceData>();
		AssetDatabase.CreateAsset(asset, GameBalanceData.ASSET_PATH + ".asset");
		AssetDatabase.Refresh();
	}

	[MenuItem("Assets/Create/Create GameBalanceData")]
	static void CreateGameBalanceData() {
		var exampleAsset = CreateInstance<GameBalanceData>();

		AssetDatabase.CreateAsset(exampleAsset, "Assets/Resources/" + GameBalanceData.ASSET_PATH + ".asset");
		AssetDatabase.Refresh();
	}
}