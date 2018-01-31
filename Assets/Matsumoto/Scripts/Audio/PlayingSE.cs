using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// SEに取り付けられている制御用クラス
/// </summary>
public class PlayingSE : MonoBehaviour {

	public event Action onDestroy;

	void OnDestroy() {
		onDestroy();
	}
}
