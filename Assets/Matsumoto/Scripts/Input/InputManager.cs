using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System;

/// <summary>
/// 入力のタイプ
/// </summary>
public enum ControlType {
	GamePadXBOX,
	GamePadPS4,
	Keyboard,
	OtherGamePad,
}

/// <summary>
/// 入力デバイスの違いを吸収するマネージャークラス
/// </summary>
public sealed class InputManager : SingletonMonoBehaviour<InputManager> {

	const int MAX_PAYER_NUM = 4;		//最大のプレイヤー人数

	ControllerData[] controllerData;	//各プレイヤーのコントローラのデータ

	// new禁止
	private InputManager() { }

	protected override void Init() {
		base.Init();

		controllerData = new ControllerData[MAX_PAYER_NUM];

		//適当に初期値を設定
		for(int i = 0;i < MAX_PAYER_NUM;i++) {
			SetControllerData(i, ControlType.Keyboard, (GamePad.Index)i);
		}
	}

	/// <summary>
	/// Axisの入力を取る
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <param name="axis"></param>
	/// <param name="isRaw"></param>
	/// <returns></returns>
	public static Vector2 GetAxis(int playerIndex, GamePad.Axis axis, bool isRaw) {

		if(playerIndex < 0 || playerIndex >= MAX_PAYER_NUM) return new Vector2();

		var controllerData = instance.controllerData[playerIndex];
		return GetAxis(controllerData.type, axis, controllerData.controllerIndex, isRaw);
	}
	/// <summary>
	/// Axisの入力を取る(コントローラ用)
	/// </summary>
	/// <param name="type"></param>
	/// <param name="axis"></param>
	/// <param name="controllerIndex"></param>
	/// <param name="isRaw"></param>
	/// <returns></returns>
	static Vector2 GetAxis(ControlType type, GamePad.Axis axis, GamePad.Index controllerIndex, bool isRaw) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetAxis(axis, controllerIndex, isRaw);
			case ControlType.GamePadPS4:
				return InputPS4.GetAxis(axis, controllerIndex, isRaw);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetAxis(axis, isRaw);
			case ControlType.OtherGamePad:
				return GamePad.GetAxis(axis, controllerIndex, isRaw);
			default:
				return new Vector2();
		}
	}


	/// <summary>
	/// Triggerの入力を取る
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <param name="trigger"></param>
	/// <param name="isRaw"></param>
	/// <returns></returns>
	public static float GetTrigger(int playerIndex, GamePad.Trigger trigger, bool isRaw) {

		if(playerIndex < 0 || playerIndex >= MAX_PAYER_NUM) return 0;

		var controllerData = instance.controllerData[playerIndex];
		return GetTrigger(controllerData.type, trigger, controllerData.controllerIndex, isRaw);
	}
	/// <summary>
	/// Triggerの入力を取る(コントローラ用)
	/// </summary>
	/// <param name="type"></param>
	/// <param name="trigger"></param>
	/// <param name="controllerIndex"></param>
	/// <param name="isRaw"></param>
	/// <returns></returns>
	static float GetTrigger(ControlType type, GamePad.Trigger trigger, GamePad.Index controllerIndex, bool isRaw) {
		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetTrigger(trigger, controllerIndex, isRaw);
			case ControlType.GamePadPS4:
				return InputPS4.GetTrigger(trigger, controllerIndex, isRaw);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKey(trigger);
			case ControlType.OtherGamePad:
				return GamePad.GetTrigger(trigger, controllerIndex, isRaw);
			default:
				return 0;
		}
	}


	/// <summary>
	/// ボタンが押されたときの入力を取る
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <param name="button"></param>
	/// <returns></returns>
	public static bool GetButtonDown(int playerIndex, GamePad.Button button) {

		if(playerIndex < 0 || playerIndex >= MAX_PAYER_NUM) return false;

		var controllerData = instance.controllerData[playerIndex];
		return GetButtonDown(controllerData.type, button, controllerData.controllerIndex);
	}
	/// <summary>
	/// ボタンが押されたときの入力を取る(コントローラ用)
	/// </summary>
	/// <param name="type"></param>
	/// <param name="button"></param>
	/// <param name="controllerIndex"></param>
	/// <returns></returns>
	static bool GetButtonDown(ControlType type, GamePad.Button button, GamePad.Index controllerIndex) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetButtonDown(button, controllerIndex);
			case ControlType.GamePadPS4:
				return InputPS4.GetButtonDown(button, controllerIndex);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKeyDown(button);
			case ControlType.OtherGamePad:
				return GamePad.GetButtonDown(button, controllerIndex);
			default:
				return false;
		}
	}


	/// <summary>
	/// ボタンを押しているときの入力を取る
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <param name="button"></param>
	/// <returns></returns>
	public static bool GetButton(int playerIndex, GamePad.Button button) {

		if(playerIndex < 0 || playerIndex >= MAX_PAYER_NUM) return false;

		var controllerData = instance.controllerData[playerIndex];
		return GetButton(controllerData.type, button, controllerData.controllerIndex);
	}
	/// <summary>
	/// ボタンを押しているときの入力を取る(コントローラ用)
	/// </summary>
	/// <param name="type"></param>
	/// <param name="button"></param>
	/// <param name="controllerIndex"></param>
	/// <returns></returns>
	static bool GetButton(ControlType type, GamePad.Button button, GamePad.Index controllerIndex) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetButton(button, controllerIndex);
			case ControlType.GamePadPS4:
				return InputPS4.GetButton(button, controllerIndex);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKey(button);
			case ControlType.OtherGamePad:
				return GamePad.GetButton(button, controllerIndex);
			default:
				return false;
		}
	}


	/// <summary>
	/// ボタンを離した時の入力を取る
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <param name="button"></param>
	/// <returns></returns>
	public static bool GetButtonUp(int playerIndex, GamePad.Button button) {

		if(playerIndex < 0 || playerIndex >= MAX_PAYER_NUM) return false;

		var controllerData = instance.controllerData[playerIndex];
		return GetButtonUp(controllerData.type, button, controllerData.controllerIndex);
	}
	/// <summary>
	/// ボタンを離した時の入力を取る(コントローラ用)
	/// </summary>
	/// <param name="type"></param>
	/// <param name="button"></param>
	/// <param name="controllerIndex"></param>
	/// <returns></returns>
	static bool GetButtonUp(ControlType type, GamePad.Button button, GamePad.Index controllerIndex) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetButtonUp(button, controllerIndex);
			case ControlType.GamePadPS4:
				return InputPS4.GetButtonUp(button, controllerIndex);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKeyUp(button);
			case ControlType.OtherGamePad:
				return GamePad.GetButtonUp(button, controllerIndex);
			default:
				return false;
		}
	}


	/// <summary>
	/// 対象のプレイヤー番号のコントローラ情報を登録する
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <param name="type"></param>
	/// <param name="controllerIndex"></param>
	public static void SetControllerData(int playerIndex, ControlType type, GamePad.Index controllerIndex) {

		if(playerIndex < 0 || playerIndex >= MAX_PAYER_NUM) return;

		instance.controllerData[playerIndex] = new ControllerData(type, controllerIndex);
	}
	/// <summary>
	/// 対象のプレイヤーのコントローラのデータを取得する
	/// </summary>
	/// <param name="playerIndex"></param>
	/// <returns></returns>
	public static ControllerData GetControllerData(int playerIndex) {

		if(playerIndex < 0 || playerIndex >= MAX_PAYER_NUM) return null;

		return instance.controllerData[playerIndex];
	}
}