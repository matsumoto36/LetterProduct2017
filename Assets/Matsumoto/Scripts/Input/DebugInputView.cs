using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

/// <summary>
/// 特定のコントローラの特定の番号をログに出力するクラス
/// </summary>
public class DebugInputView : MonoBehaviour {

	public ControlType inputControllerType;
	public GamePad.Index inputControllerNum;

	void Update () {

		//Axisの入力を調べる
		CheckAxis(inputControllerType, GamePad.Axis.Dpad, inputControllerNum, true);
		CheckAxis(inputControllerType, GamePad.Axis.LeftStick, inputControllerNum, true);
		CheckAxis(inputControllerType, GamePad.Axis.RightStick, inputControllerNum, true);

		//Triggerの入力を調べる
		CheckTrigger(inputControllerType, GamePad.Trigger.LeftTrigger, inputControllerNum, true);
		CheckTrigger(inputControllerType, GamePad.Trigger.RightTrigger, inputControllerNum, true);

		//Buttonの入力を調べる
		CheckButton(inputControllerType, GamePad.Button.A, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.B, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.Back, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.LeftShoulder, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.LeftStick, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.RightShoulder, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.RightStick, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.Start, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.X, inputControllerNum);
		CheckButton(inputControllerType, GamePad.Button.Y, inputControllerNum);

	}

	#region CheckInput

	void CheckAxis(ControlType type, GamePad.Axis axisType, GamePad.Index playerNum, bool isRaw) {
		Vector2 axis = InputManager.GetAxis(type, axisType, playerNum, true);
		if(!(axis.x == 0 && axis.y == 0)) {
			Debug.Log(GetLog(type, axisType.ToString(), playerNum, axis));
		}
	}

	void CheckTrigger(ControlType type, GamePad.Trigger triggerType, GamePad.Index playerNum, bool isRaw) {
		float trigger = InputManager.GetTrigger(type, triggerType, playerNum, true);
		if(trigger != 0) {
			Debug.Log(GetLog(type, triggerType.ToString(), playerNum, trigger));
		}
	}

	void CheckButton(ControlType type, GamePad.Button buttonType, GamePad.Index playerNum) {

		bool button = InputManager.GetButtonDown(type, buttonType, playerNum);
		if(button) {
			Debug.Log(GetLog(type, buttonType.ToString(), playerNum, false));
		}

		button = InputManager.GetButtonUp(type, buttonType, playerNum);
		if(button) {
			Debug.Log(GetLog(type, buttonType.ToString(), playerNum, true));
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
