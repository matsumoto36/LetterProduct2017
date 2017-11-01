using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

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

		switch(button) {
			case GamePad.Button.A:
				break;
			case GamePad.Button.B:
				break;
			case GamePad.Button.Y:
				break;
			case GamePad.Button.X:
				break;
			case GamePad.Button.RightShoulder:
				break;
			case GamePad.Button.LeftShoulder:
				break;
			case GamePad.Button.RightStick:
				break;
			case GamePad.Button.LeftStick:
				break;
			case GamePad.Button.Back:
				break;
			case GamePad.Button.Start:
				break;
			default:
				break;
		}

		return false;
	}

	public static bool GetKey(GamePad.Button button) {

		switch(button) {
			case GamePad.Button.A:
				break;
			case GamePad.Button.B:
				break;
			case GamePad.Button.Y:
				break;
			case GamePad.Button.X:
				break;
			case GamePad.Button.RightShoulder:
				break;
			case GamePad.Button.LeftShoulder:
				break;
			case GamePad.Button.RightStick:
				break;
			case GamePad.Button.LeftStick:
				break;
			case GamePad.Button.Back:
				break;
			case GamePad.Button.Start:
				break;
			default:
				break;
		}

		return false;
	}

	public static float GetKey(GamePad.Trigger trigger) {
		return Input.GetButton(GetTriggerName(trigger)) ? 1 : 0;
	}

	public static bool GetKeyUp(GamePad.Button button) {

		switch(button) {
			case GamePad.Button.A:
				break;
			case GamePad.Button.B:
				break;
			case GamePad.Button.Y:
				break;
			case GamePad.Button.X:
				break;
			case GamePad.Button.RightShoulder:
				break;
			case GamePad.Button.LeftShoulder:
				break;
			case GamePad.Button.RightStick:
				break;
			case GamePad.Button.LeftStick:
				break;
			case GamePad.Button.Back:
				break;
			case GamePad.Button.Start:
				break;
			default:
				break;
		}

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

	#endregion
}
