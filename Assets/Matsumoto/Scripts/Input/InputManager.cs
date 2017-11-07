using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

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
public static class InputManager {

	public static Vector2 GetAxis(ControlType type, GamePad.Axis axis, GamePad.Index index, bool isRaw) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetAxis(axis, index, isRaw);
			case ControlType.GamePadPS4:
				return InputPS4.GetAxis(axis, index, isRaw);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetAxis(axis, isRaw);
			case ControlType.OtherGamePad:
				return GamePad.GetAxis(axis, index, isRaw);
			default:
				return new Vector2();
		}
	}

	public static float GetTrigger(ControlType type, GamePad.Trigger trigger, GamePad.Index index, bool isRaw) {
		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetTrigger(trigger, index, isRaw);
			case ControlType.GamePadPS4:
				return InputPS4.GetTrigger(trigger, index, isRaw);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKey(trigger);
			case ControlType.OtherGamePad:
				return GamePad.GetTrigger(trigger, index, isRaw);
			default:
				return 0;
		}
	}

	public static bool GetButtonDown(ControlType type, GamePad.Button button, GamePad.Index index) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetButtonDown(button, index);
			case ControlType.GamePadPS4:
				return InputPS4.GetButtonDown(button, index);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKeyDown(button);
			case ControlType.OtherGamePad:
				return GamePad.GetButtonDown(button, index);
			default:
				return false;
		}
	}

	public static bool GetButton(ControlType type, GamePad.Button button, GamePad.Index index) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetButton(button, index);
			case ControlType.GamePadPS4:
				return InputPS4.GetButton(button, index);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKey(button);
			case ControlType.OtherGamePad:
				return GamePad.GetButton(button, index);
			default:
				return false;
		}
	}

	public static bool GetButtonUp(ControlType type, GamePad.Button button, GamePad.Index index) {

		switch(type) {
			case ControlType.GamePadXBOX:
				return InputXBOX.GetButtonUp(button, index);
			case ControlType.GamePadPS4:
				return InputPS4.GetButtonUp(button, index);
			case ControlType.Keyboard:
				//キーボードはindex関係なしに返す
				return InputKeyboard.GetKeyUp(button);
			case ControlType.OtherGamePad:
				return GamePad.GetButtonUp(button, index);
			default:
				return false;
		}
	}


}