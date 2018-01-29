﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GamepadInput;

public class SelectSceneController : MonoBehaviour {

	const string NEXT_SCENE_NAME = "TestPlayerSpawn";

	public GamePad.Button entryButton;

	public UnitData playerBase;
	public CustomizeCharactor[] custom;

	int[] entryControllerID = new int[InputManager.MAX_PAYER_NUM];
	int playerCount = 0;
	int readyCount = 0;

	// Use this for initialization
	void Awake () {
		Init();
	}

	public void Init() {

		for(int i = 0;i < entryControllerID.Length;i++) {
			entryControllerID[i] = -1;
		}

		//初期化
		foreach(var item in custom) {
			item.Init();
		}
	}

	void Update() {

		int joystickNum;
		if(InputManager.GetButtonDownAny(entryButton, out joystickNum)) {
			Entry(joystickNum);
		}


	}

	void Entry(int joystickNum) {

		if(entryControllerID
			.Where(item => item == joystickNum)
			.Count() > 0) return;

		var playerID = -1;
		for(int i = 0;i < entryControllerID.Length;i++) {
			if(entryControllerID[i] == -1) {
				playerID = i;
				break;
			}
		}

		if(playerID == -1) return;

		//コントローラを登録
		entryControllerID[playerID] = joystickNum;

		//プレイヤーと紐づけ
		if(joystickNum == 0) {
			InputManager.SetControllerData(playerID, ControlType.Keyboard, (GamePad.Index)3);
		}
		else {
			InputManager.SetControllerData(playerID, ControlType.GamePadXBOX, (GamePad.Index)(joystickNum - 1));
		}

		//カスタムのエントリー処理
		custom[playerID].Entry(playerID);

		playerCount++;
	}

	void StartGame() {

		Debug.Log("GameStart");

		//ゲームデータ初期化
		GameData.InitData();

		for(int i = 0; i < custom.Length;i++) {
			if(!custom[i].isReady) continue;

			//データ登録
			custom[i].isFreeze = true;
			custom[i].Customize();

			//エントリー情報登録
			GameData.instance.isEntryPlayer[i] = true;

		}

		//シーン移動
		FadeManager.Instance.LoadScene(NEXT_SCENE_NAME, 2);
	}

	public void Exit(int playerID) {

		entryControllerID[playerID] = -1;
		playerCount--;

	}

	public void Ready() {
		readyCount++;

		if(readyCount == playerCount) StartGame();
	}

	public void ReadyCancel() {
		readyCount--;
	}

}