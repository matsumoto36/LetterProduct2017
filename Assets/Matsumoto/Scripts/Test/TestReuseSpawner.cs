using UnityEngine;
using System.Collections;

public class TestReuseSpawner : MonoBehaviour {

	void Start() {

		var spawner = GetComponentInChildren<EnemySpawner>();

		for(int i = 0;i < 10;i++) {
			spawner.SpawnEnemy(false);
		}
	}

}
