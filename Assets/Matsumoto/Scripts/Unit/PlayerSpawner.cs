using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PlayerSpawner : MonoBehaviour {

	public PlayerData[] playerData;
	public Transform[] spawnPoint;

	void Awake() {

		if(playerData.Length == 0) playerData = new PlayerData[InputManager.MAX_PAYER_NUM];
		if(spawnPoint.Length == 0) spawnPoint = new Transform[InputManager.MAX_PAYER_NUM];

		foreach(Transform item in spawnPoint) {

			if(!item) continue;
			if(item.childCount == 0) continue; 
			
			if(Application.isPlaying) {
				Destroy(item.GetChild(0).gameObject);
			}
			else {
				DestroyImmediate(item.GetChild(0).gameObject);
			}
		}
	}

	void Update() {

		if(Application.isPlaying) return;

		for(int i = 0;i < InputManager.MAX_PAYER_NUM;i++) {
			if(!spawnPoint[i]) continue;
			if(spawnPoint[i].childCount > 1) Awake();
			if(playerData[i]) ModelUpdate(i);
		}
	}

	/// <summary>
	/// 表示用モデルを取得する
	/// </summary>
	bool GetModel(int playerNum) {

		if(spawnPoint[playerNum].childCount != 0) DestroyImmediate(spawnPoint[playerNum].GetChild(0));
		if(!playerData[playerNum]) return false;
		if(!playerData[playerNum].model) return false;

		var model = Instantiate(playerData[playerNum].model);
		model.name = playerData[playerNum].model.name + "(Preview)";
		model.transform.SetParent(spawnPoint[playerNum]);

		return true;
	}

	/// <summary>
	/// 表示用モデルの位置を更新する
	/// </summary>
	void ModelUpdate(int playerNum) {

		if(spawnPoint[playerNum].childCount == 0) {
			if(!GetModel(playerNum)) return;
		}

		var model = spawnPoint[playerNum].GetChild(0).gameObject;
		if(model.name != playerData[playerNum].model.name + "(Preview)") {
			if(!GetModel(playerNum)) return;
		}

		model.transform.SetPositionAndRotation(spawnPoint[playerNum].position, spawnPoint[playerNum].rotation);
	}

	/// <summary>
	/// プレイヤーをスポーンする
	/// </summary>
	/// <returns></returns>
	public void SpawnPlayer(int playerNum) {
		var player = playerData[playerNum].Spawn(spawnPoint[playerNum].position, spawnPoint[playerNum].rotation);
		((Player)player).playerIndex = playerNum;
	}
}
