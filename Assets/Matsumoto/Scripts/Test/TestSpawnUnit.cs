using UnityEngine;
using System.Collections;

/// <summary>
/// プレイヤーをスポーンするクラス。GameManagerに移行する予定。
/// </summary>
public class TestSpawnUnit : MonoBehaviour {

	public PlayerSpawner spawner;

	public bool useCustomSpawn;
	public bool[] customSpawnFlg;

	// Use this for initialization
	void Start() {

		var spawnflgArray = useCustomSpawn ? customSpawnFlg : GameData.instance.isEntryPlayer;
		for(int i = 0;i < InputManager.MAX_PAYER_NUM;i++) {
			if(spawnflgArray[i]) spawner.SpawnPlayer(i);
		}

	}
}
