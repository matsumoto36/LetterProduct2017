using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{

	PlayerSpawner spawner;

    public bool useCustomSpawn;
    public bool[] customSpawnFlg;

	public bool nowGamePlay { get; private set; }
	bool isGameOver = false;

	void Start() {
		if(useCustomSpawn) {

			spawner = FindObjectOfType<PlayerSpawner>();

			GameData.instance.isEntryPlayer = customSpawnFlg;
			spawner.SpawnPlayer();

			GameData.instance.isSpawnedPlayer = true;

			UIManager.instance.HPvarSwich(true);//HP表示

		}
	}

	// Update is called once per frame
	void Update()
	{
		if(!nowGamePlay) return;

		if(!isGameOver) CheckGameOverUpdate();
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

	public void StartGame() {

		UIManager.instance.HPvarSwich(true);//HP表示
	}

	public void MoveStage(string nextStage) {

	}

	public void Retry() {

		Debug.Log("Retry");

		//エントリー情報を取っておく
		bool[] entryBff = new bool[GameData.MAX_PLAYER_NUM];
		for(int i = 0;i < entryBff.Length;i++) {
			entryBff[i] = GameData.instance.isEntryPlayer[i];
		}

		//ゲームデータ初期化
		GameData.InitData();

		//エントリー情報復元
		GameData.instance.isEntryPlayer = entryBff;

		AudioManager.FadeOut(2);

		//シーン移動
		FadeManager.instance.LoadScene(GameData.START_STAGE, 2);
	}

	void GameOver() {

		Debug.Log("GameOver");
		UIManager.instance.GameOverSwich(true);

		nowGamePlay = false;
	}

	IEnumerator MoveStageAnim(string nextStage) {
		yield return null;

		UIManager.instance.HPvarSwich(true);//HP表示

		spawner = FindObjectOfType<PlayerSpawner>();

		if(useCustomSpawn) GameData.instance.isEntryPlayer = customSpawnFlg;
		spawner.SpawnPlayer();

		GameData.instance.isSpawnedPlayer = true;
	}
}

