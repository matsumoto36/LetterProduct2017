using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが近づいたら敵をスポーンさせるトリガー
/// </summary>
[RequireComponent(typeof(Collider))]
public class EnemySpawnTrigger : MonoBehaviour {

	EnemySpawner[] spawnerArray;

	void Awake() {
		spawnerArray = GetComponentsInChildren<EnemySpawner>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.GetComponent<Player>()) {

			//子オブジェクトすべてのスポーナーを発動させる
			foreach(var item in spawnerArray) {
				item.SpawnEnemy();
			}

			Destroy(gameObject);
		}
	}

}
