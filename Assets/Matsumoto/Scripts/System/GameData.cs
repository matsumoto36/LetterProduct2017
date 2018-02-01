using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 進行中のゲームデータを持っているクラス
/// </summary>
public sealed class GameData : SingletonMonoBehaviour<GameData> {

	public bool[] isEntryPlayer { get; set; }
	public bool[] isDeath { get; set; }

	public int[] sumDamage { get; set; }

	public bool isSpawnedPlayer;

	//new禁止
	private GameData() { }

	protected override void Init() {
		base.Init();

		InitData();
	}

	//ゲームデータの初期化
	public static void InitData() {

		instance.isEntryPlayer = new bool[InputManager.MAX_PAYER_NUM];
		instance.isDeath = new bool[InputManager.MAX_PAYER_NUM];
		instance.sumDamage = new int[4];

		instance.isSpawnedPlayer = false;
	}

	// Update is called once per frame
	void Update() {

	}
}
