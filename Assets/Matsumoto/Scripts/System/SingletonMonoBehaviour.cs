using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// シングルトンの親クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {

	public static T instance {
		get
		{
			if (!_instance) Create();
			return _instance;
		}
	}
	static T _instance;

	static void Create()
	{
		_instance = new GameObject(string.Format("[Singleton - {0}]", typeof(T).ToString()))
			.AddComponent<T>();

		DontDestroyOnLoad(_instance.gameObject);

		_instance.GetComponent<SingletonMonoBehaviour<T>>().Init();
	}

	/// <summary>
	/// 初期化用
	/// </summary>
	protected virtual void Init() { }

	void Awake() {
		if(_instance) Destroy(gameObject);
	}
}
