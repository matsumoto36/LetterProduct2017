using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 進行中のゲームデータを持っているクラス
/// </summary>
public sealed class GameData : SingletonMonoBehaviour<GameData> {

	public const int MAX_PLAYER_NUM = 4;
	public const string START_STAGE = "NewStage";

	public bool[] isEntryPlayer { get; set; }
	public bool[] isDeath { get; set; }

	public Player[] spawnedPlayer;
	public int[] sumDamage { get; set; }

	public int maxCombo { get; set; }
	public float startTime { get; set; }
	public float bossStartTime { get; set; }
	public float clearTime { get; set; }

	public bool isSpawnedPlayer;

	//new禁止
	private GameData() { }

	protected override void Init() {
		base.Init();

		InitData();
	}

	//ゲームデータの初期化
	public static void InitData() {

		instance.isEntryPlayer = new bool[MAX_PLAYER_NUM];
		instance.isDeath = new bool[MAX_PLAYER_NUM];
		instance.spawnedPlayer = new Player[MAX_PLAYER_NUM];
		instance.sumDamage = new int[4];

		instance.maxCombo = 0;
		instance.startTime = 0;
		instance.bossStartTime = 0;
		instance.clearTime = 0;

		instance.isSpawnedPlayer = false;
	}

	// Update is called once per frame
	void Update() {

	}
}
