using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using GamepadInput;

public class SelectSceneController : MonoBehaviour {

	public GamePad.Button entryButton;
	public CustomizeCharactor[] custom;

	int[] entryControllerID = new int[InputManager.MAX_PAYER_NUM];
	int playerCount = 0;
	int readyCount = 0;

	// Use this for initialization
	void Awake () {
		Init();
	}

	public void Init() {

		//
		for(int i = 0;i < entryControllerID.Length;i++) {
			entryControllerID[i] = -1;
		}

		ControlButtonController.DestroyController(-1);

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
			InputManager.SetControllerData(playerID, ControlType.Keyboard, (GamePad.Index)joystickNum);
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

		foreach(var item in custom) {
			if(!item.isReady) continue;

			item.isFreeze = true;
			item.Customize();
		}

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
