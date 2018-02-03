using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public PlayerSpawner spawner;
	public string nextSceneName;

    public bool useCustomSpawn;
    public bool[] customSpawnFlg;

	Enemy boss;

	public static bool nowPlayingGame { get; private set; }
	static bool isGameOver = false;

	public bool spawnedBoss { get; private set; }

	void Start() {

		if(!nowPlayingGame) InitGameManager();

		isGameOver = false;

		Unit.Init();
		if(GameData.instance.isSpawnedPlayer)
			Unit.CollectPlayer();

		var flg = useCustomSpawn ? customSpawnFlg : GameData.instance.isEntryPlayer;
		spawner.SpawnPlayer();
		
		GameData.instance.isSpawnedPlayer = true;
		UIManager.instance.HPvarSwich(true);//HP表示
	
	}
	
	// Update is called once per frame
	void Update()
	{
		if(!nowPlayingGame) return;
	
		if(!isGameOver) CheckGameOverUpdate();
	
		if(spawnedBoss) CheckBossDeathUpdate();
	}
	
	void CheckBossDeathUpdate() {
		if(!boss) GameClear();
	}
	
	void CheckGameOverUpdate() {

		var checkFlag = false;
		for(int i = 0;i < GameData.MAX_PLAYER_NUM;i++) {
			if(GameData.instance.isEntryPlayer[i] && !GameData.instance.isDeath[i]) {
				checkFlag = true;
			}
		}
		
		if(checkFlag == false) {
			GameOver();
		}
	}
	
	public void SetSpawnedBoss(Enemy boss) {
		spawnedBoss = true;
		this.boss = boss;
	}

	public void InitGameManager() {
		nowPlayingGame = true;
	}

	/// <summary>
	/// リトライする
	/// </summary>
	public static void Retry() {
	
		Debug.Log("Retry");

		AudioManager.FadeOut(2);


		//シーン移動
		FadeManager.instance.LoadScene(GameData.START_STAGE, 2, () => {

			if(isGameOver) UIManager.instance.GameOverSwich(false);

			//エントリー情報を取っておく
			bool[] entryBff = new bool[GameData.MAX_PLAYER_NUM];
			for(int i = 0;i < entryBff.Length;i++) {
				entryBff[i] = GameData.instance.isEntryPlayer[i];
			}
			Unit.Clear();
			//ゲームデータ初期化
			GameData.InitData();
			//エントリー情報復元
			GameData.instance.isEntryPlayer = entryBff;

		});
	}

	/// <summary>
	/// セレクトシーンへ行く
	/// </summary>
	public static void GoToSelectScene() {

		Debug.Log("Select");

		AudioManager.FadeOut(2);

		//シーン移動
		FadeManager.instance.LoadScene("Main_Select", 2, () => {

			if(isGameOver) UIManager.instance.GameOverSwich(false);

			UIManager.instance.HPvarSwich(false);

			Unit.Clear();
		});
	}

	void GameClear() {
		Debug.Log("GameClear");
		UIManager.instance.ResultSwich(true);
	
		nowPlayingGame = false;
	}

	void GameOver() {

		isGameOver = true;

		Debug.Log("GameOver");
		UIManager.instance.GameOverSwich(true);
	
		nowPlayingGame = false;
	}
}

