using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class BulletObjectPool : SingletonMonoBehaviour<BulletObjectPool> {

	const string BULLET_PATH = "Model/Weapon/Bullet";

	Dictionary<string, ObjectPooler> poolTable;

	protected override void Init() {
		base.Init();

		Load();
	}

	void Load() {

		poolTable = new Dictionary<string, ObjectPooler>();

		foreach(var item in Resources.LoadAll<GameObject>(BULLET_PATH)) {

			var pool = ObjectPooler.GetObjectPool(item);
			pool.maxCount = 200;
			pool.prepareCount = 100;
			pool.Generate(transform);
			poolTable.Add(item.name, pool);
		}
	}

	public static GameObject GetInstance(string prefabName) {

		if(!instance.poolTable.ContainsKey(prefabName)) {
			Debug.LogError(prefabName + " is not found.");
			return null;
		}

		return instance.poolTable[prefabName].GetInstance();
	}
}