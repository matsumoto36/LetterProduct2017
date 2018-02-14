﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GamepadInput;

public class GameManager : MonoBehaviour
{
    public PlayerSpawner spawner;
    public string nextSceneName;

    public string playBGMName;

    public bool useCustomSpawn;
    public bool[] customSpawnFlg;

    public EnemyActiveSpawner enemySpawner;

    public static bool nowPlayingGame { get; private set; }
    static bool isGameOver = false;
    static bool isGameClear = false;

    void Start()
    {

        if (!nowPlayingGame) InitGameManager();

        isGameOver = false;
        isGameClear = false;

        Unit.Init();
        if (GameData.instance.isSpawnedPlayer)
            Unit.CollectPlayer();

        if (useCustomSpawn) GameData.instance.isEntryPlayer = customSpawnFlg;
        spawner.SpawnPlayer();

        GameData.instance.isSpawnedPlayer = true;
        UIManager.instance.HPvarSwich(true);//HP表示

        if (GameData.instance.startTime == 0)
            GameData.instance.startTime = Time.time;

        //BGMの再生
        AudioManager.FadeIn(2, playBGMName);

		//マネージャー系を起動しておく
		var manager = ParticleManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!nowPlayingGame) return;

        if (!isGameOver) CheckGameOverUpdate();
		CheckSpawnerDeathUpdate();

        //一時的につける
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.instance.OptionSwich(!UIManager.instance.isOpenOption);
        }
    }

    void CheckSpawnerDeathUpdate()
    {
        if (!enemySpawner) GameClear();
    }

    void CheckGameOverUpdate()
    {

        var checkFlag = false;
        for (int i = 0; i < GameData.MAX_PLAYER_NUM; i++)
        {
            if (GameData.instance.isEntryPlayer[i] && !GameData.instance.isDeath[i])
            {
                checkFlag = true;
            }
        }

        if (checkFlag == false)
        {
            GameOver();
        }
    }

    public void InitGameManager()
    {
        nowPlayingGame = true;
    }

    /// <summary>
    /// リトライする
    /// </summary>
    public static void Retry()
    {

        Debug.Log("Retry");

        AudioManager.FadeOut(2);


        //シーン移動
        FadeManager.instance.LoadScene(GameData.START_STAGE, 2, () => {

            if (isGameOver) UIManager.instance.GameOverSwich(false);
            if (isGameClear) UIManager.instance.ResultSwich(false);

            //エントリー情報を取っておく
            bool[] entryBff = new bool[GameData.MAX_PLAYER_NUM];
            for (int i = 0; i < entryBff.Length; i++)
            {
                entryBff[i] = GameData.instance.isEntryPlayer[i];
            }
            Unit.Clear();
            EnemySpawner.Clear();

            //ゲームデータ初期化
            GameData.InitData();
            //エントリー情報復元
            GameData.instance.isEntryPlayer = entryBff;

        });
    }

    /// <summary>
    /// セレクトシーンへ行く
    /// </summary>
    public static void GoToSelectScene()
    {

        Debug.Log("Select");

        AudioManager.FadeOut(2);

        //シーン移動
        FadeManager.instance.LoadScene("Main_Select", 2, () => {

            if (isGameOver) UIManager.instance.GameOverSwich(false);
            if (isGameClear) UIManager.instance.ResultSwich(false);

            UIManager.instance.HPvarSwich(false);

            Unit.Clear();
            EnemySpawner.Clear();
        });
    }

    /// <summary>
    /// タイトルシーンへ行く
    /// </summary>
    public static void GoToTitleScene()
    {

        Debug.Log("Title");

        AudioManager.FadeOut(2);

        //シーン移動
        FadeManager.instance.LoadScene("Main_Title", 2, () => {

            if (isGameOver) UIManager.instance.GameOverSwich(false);
            if (isGameClear) UIManager.instance.ResultSwich(false);

            UIManager.instance.HPvarSwich(false);

            Unit.Clear();
            EnemySpawner.Clear();
        });
    }

    void GameClear()
    {

        isGameClear = true;
        nowPlayingGame = false;

        Debug.Log("GameClear");

        AudioManager.FadeOut(2);

        StartCoroutine(GameClearAnim());
    }
    IEnumerator GameClearAnim()
    {
        yield return new WaitForSeconds(2);

        UIManager.instance.ResultSwich(true);

        AudioManager.PlayBGM("Result");
    }

    void GameOver()
    {

        isGameOver = true;
        nowPlayingGame = false;

        Debug.Log("GameOver");

        AudioManager.FadeOut(2);

        StartCoroutine(GameOverAnim());
    }
    IEnumerator GameOverAnim()
    {
        yield return new WaitForSeconds(2);

        UIManager.instance.GameOverSwich(true);

    }
}

