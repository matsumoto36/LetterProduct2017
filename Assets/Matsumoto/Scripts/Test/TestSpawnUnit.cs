using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// プレイヤーをスポーンするクラス。GameManagerに移行する予定。
/// </summary>
public class TestSpawnUnit : MonoBehaviour {

	public PlayerSpawner spawner;

	public bool useCustomSpawn;
	public bool[] customSpawnFlg;

	// Use this for initialization
	void Start() {

		if(useCustomSpawn) GameData.instance.isEntryPlayer = customSpawnFlg;
		for(int i = 0;i < InputManager.MAX_PAYER_NUM;i++) {
			if(GameData.instance.isEntryPlayer[i]) spawner.SpawnPlayer(i);
		}
		GameData.instance.isSpawnedPlayer = true;
	}
}
