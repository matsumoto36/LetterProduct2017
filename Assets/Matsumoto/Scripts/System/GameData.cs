using UnityEngine;
using System.Collections;

/// <summary>
/// 進行中のゲームデータを持っているクラス
/// </summary>
public sealed class GameData : SingletonMonoBehaviour<GameData> {

	public bool[] isEntryPlayer { get; set; }

	//new禁止
	private GameData() { }

	//ゲームデータの初期化
	public static void InitData() {

		instance.isEntryPlayer = new bool[InputManager.MAX_PAYER_NUM];

	}

	// Update is called once per frame
	void Update() {

	}
}
