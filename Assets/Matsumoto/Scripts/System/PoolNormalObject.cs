using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 普通のゲームオブジェクトをプールする
/// </summary>
public sealed class PoolNormalObject : SingletonMonoBehaviour<PoolNormalObject> {

	const string OBJECT_PATH = "System/GameObject";

	public int maxCount = 500;
	public int prepareCount = 100;

	ObjectPooler pool;

	//外部からのnew禁止
	private PoolNormalObject() { }

	protected override void Init() {
		base.Init();
		
		pool = ObjectPooler.GetObjectPool(Resources.Load<GameObject>(OBJECT_PATH));
		pool.maxCount = maxCount;
		pool.prepareCount = prepareCount;
		pool.Generate(transform);
	}

	public static GameObject GetInstance() {
		return instance.pool.GetInstance();
	}
}
