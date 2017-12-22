using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;
using System;

/// <summary>
/// 特定のコントローラの特定の番号をログに出力するクラス
/// </summary>
public class DebugInputView : MonoBehaviour {

	public int playerIndex;

	public string[] names;

	void Update () {

		names = Input.GetJoystickNames();

		//Axisの入力を調べる
		foreach(GamePad.Axis item in Enum.GetValues(typeof(GamePad.Axis))) {
			CheckAxis(playerIndex, item, true);
		}

		//Triggerの入力を調べる
		foreach(GamePad.Trigger item in Enum.GetValues(typeof(GamePad.Trigger))) {
			CheckTrigger(playerIndex, item, true);
		}

		//Buttonの入力を調べる
		foreach(GamePad.Button item in Enum.GetValues(typeof(GamePad.Button))) {
			CheckButton(playerIndex, item);
		}
	}

	#region CheckInput

	void CheckAxis(int playerIndex, GamePad.Axis axisType, bool isRaw) {
		Vector2 axis = InputManager.GetAxis(playerIndex, axisType, true);
		if(!(axis.x == 0 && axis.y == 0)) {
			var controllerData = InputManager.GetControllerData(playerIndex);
			Debug.Log(GetLog(controllerData.type, axisType.ToString(), controllerData.controllerIndex, axis));
		}
	}

	void CheckTrigger(int playerIndex, GamePad.Trigger triggerType, bool isRaw) {
		float trigger = InputManager.GetTrigger(playerIndex, triggerType, true);
		if(trigger != 0) {
			var controllerData = InputManager.GetControllerData(playerIndex);
			Debug.Log(GetLog(controllerData.type, triggerType.ToString(), controllerData.controllerIndex, trigger));
		}
	}

	void CheckButton(int playerIndex, GamePad.Button buttonType) {

		bool button = InputManager.GetButtonDown(playerIndex, buttonType);
		var controllerData = InputManager.GetControllerData(playerIndex);

		if(button) {
			Debug.Log(GetLog(controllerData.type, buttonType.ToString(), controllerData.controllerIndex, false));
		}

		button = InputManager.GetButtonUp(playerIndex, buttonType);
		if(button) {
			Debug.Log(GetLog(controllerData.type, buttonType.ToString(), controllerData.controllerIndex, true));
		}
	}

	#endregion

	#region LogText

	string GetLogBase(ControlType type, string inputName, GamePad.Index playerNum) {
		return string.Format("{0}-{1}-{2}", type.ToString(), playerNum.ToString(), inputName);
	}

	string GetLog(ControlType type, string inputName, GamePad.Index playerNum, Vector2 axis) {
		return string.Format("{0} 値{1}", GetLogBase(type, inputName, playerNum), axis);
	}

	string GetLog(ControlType type, string inputName, GamePad.Index playerNum, float trigger) {
		return string.Format("{0} 値({1})", GetLogBase(type, inputName, playerNum), trigger);
	}

	string GetLog(ControlType type, string inputName, GamePad.Index playerNum, bool isUp) {
		return string.Format("{0} 状態({1})", GetLogBase(type, inputName, playerNum), isUp ? "Up" : "Down");
	}

	#endregion
}
