using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

/// <summary>
/// コントローラ入力に対応したキーボード
/// </summary>
public static class InputKeyboard {

	public static Vector2 GetAxis(GamePad.Axis axis, bool isRaw) {

		//キーボードはindex関係なしに返す

		var axisName = GetAxisName(axis);

		Vector2 keyboardAxis;

		if(isRaw) {
			keyboardAxis.x = Input.GetAxisRaw(axisName[0]);
			keyboardAxis.y = Input.GetAxisRaw(axisName[1]);
		}
		else {
			keyboardAxis.x = Input.GetAxis(axisName[0]);
			keyboardAxis.y = Input.GetAxis(axisName[1]);
		}

		return keyboardAxis.normalized;
	}

	public static bool GetKeyDown(GamePad.Button button) {

		var name = GetButtonName(button);
		if(name != "NONE") return Input.GetButtonDown(name);

		return false;
	}

	public static bool GetKey(GamePad.Button button) {

		var name = GetButtonName(button);
		if(name != "NONE") return Input.GetButton(name);

		return false;
	}

	public static float GetKey(GamePad.Trigger trigger) {
		var name = GetTriggerName(trigger);
		if(name != "NONE") return Input.GetButton(name) ? 1 : 0;

		return 0;
	}

	public static bool GetKeyUp(GamePad.Button button) {

		var name = GetButtonName(button);
		if(name != "NONE") return Input.GetButtonUp(name);

		return false;
	}

	#region ConvertName

	static string[] GetAxisName(GamePad.Axis axis) {

		switch(axis) {
			case GamePad.Axis.LeftStick:
				return new string[] { "Horizontal", "Vertical" };
			case GamePad.Axis.RightStick:
				return new string[] { "Mouse X", "Mouse Y" };
			case GamePad.Axis.Dpad:
				return new string[] { "Mouse X", "Mouse Y" };
			default:
				return new string[] { "ERR", "ERR" };
		}
	}

	static string GetTriggerName(GamePad.Trigger trigger) {

		switch(trigger) {
			case GamePad.Trigger.LeftTrigger:
				return "FireLeft";
			case GamePad.Trigger.RightTrigger:
				return "FireRight";
			default:
				return "ERR";
		}
	}

	static string GetButtonName(GamePad.Button button) {

		switch(button) {
			case GamePad.Button.A:
				return "DuraEgg";
			case GamePad.Button.B:
				return "NONE";
			case GamePad.Button.Y:
				return "NONE";
			case GamePad.Button.X:
				return "NONE";
			case GamePad.Button.RightShoulder:
				return "FireRight";
			case GamePad.Button.LeftShoulder:
				return "FireLeft";
			case GamePad.Button.RightStick:
				return "NONE";
			case GamePad.Button.LeftStick:
				return "NONE";
			case GamePad.Button.Back:
				return "NONE";
			case GamePad.Button.Start:
				return "NONE";
			default:
				return "ERR";
		}
	}

	#endregion
}
