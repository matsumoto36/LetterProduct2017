using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GamepadInput;

public class SelectSceneController : MonoBehaviour {

	public GamePad.Button entryButton;
	public GamePad.Button exitButton;

	public UnitData playerBase;
	public CustomizeCharactor[] custom;

	int[] entryControllerID = new int[GameData.MAX_PLAYER_NUM];
	int playerCount = 0;
	int readyCount = 0;

	bool isBackTitle;

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

		AudioManager.FadeIn(1, "Menu");

		//カーソルを隠す
		Cursor.visible = false;
	}

	void Update() {

		if(isBackTitle) return;

		ControllerInfo info;
		if(InputManager.GetButtonDownAny(entryButton, out info)) {
			Entry(info);
		}

		if(InputManager.GetButtonDownAny(exitButton) && playerCount == 0) {
			isBackTitle = true;
			FadeManager.instance.LoadScene("Main_Title", 1);
		}

		foreach(var item in custom) {
			item.SelectUpdate();
		}
	}

	void Entry(ControllerInfo info) {

		if(entryControllerID
			.Where(item => item == info.joystickNum)
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
		entryControllerID[playerID] = info.joystickNum;

		//プレイヤーと紐づけ
		var num = info.type == ControlType.Keyboard ? 0 : info.joystickNum - 1;
		InputManager.SetControllerData(playerID, info.type, (GamePad.Index)num);

		//カスタムのエントリー処理
		custom[playerID].Entry(playerID);

		//SEの再生
		AudioManager.PlaySE("Player_Entry");

		//キーボードであればカーソルを出す
		if(info.type == ControlType.Keyboard) Cursor.visible = true;

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

		AudioManager.PlaySE("Weapon_ALL_Ready");
		AudioManager.FadeOut(2);
		
		//シーン移動
		FadeManager.instance.LoadScene(GameData.START_STAGE, 2);
	}

	public void Exit(int playerID) {

		//キーボードであればカーソルを消す
		if(playerID == entryControllerID[playerID]) Cursor.visible = false;

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
