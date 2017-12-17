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

	public static T instance { get; private set; }

	static SingletonMonoBehaviour() {
		instance = new GameObject(string.Format("[Singleton - {0}]", typeof(T).ToString()))
			.AddComponent<T>();

		DontDestroyOnLoad(instance.gameObject);
	}

	void Awake() {
		if(instance) Destroy(gameObject);
	}
}
