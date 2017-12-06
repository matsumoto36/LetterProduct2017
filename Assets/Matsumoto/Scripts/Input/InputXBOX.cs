using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

/// <summary>
/// XBOXコントローラ
/// </summary>
public static class InputXBOX {

	public static Vector2 GetAxis(GamePad.Axis axis, GamePad.Index controlIndex, bool raw = false) {
		//Axisに渡す名前
		string xName = "", yName = "";

		controlIndex++;

		switch(axis) {
			case GamePad.Axis.Dpad:
				xName = "PS4DPad" + (int)controlIndex + "_XAxis";
				yName = "PS4DPad" + (int)controlIndex + "_YAxis";
				break;
			case GamePad.Axis.LeftStick:
				xName = "L_JoyStick" + (int)controlIndex + "_XAxis";
				yName = "L_JoyStick" + (int)controlIndex + "_YAxis";
				break;
			case GamePad.Axis.RightStick:
				xName = "R_JoyStick" + (int)controlIndex + "_XAxis";
				yName = "R_JoyStick" + (int)controlIndex + "_YAxis";
				break;
		}		

		//初期化
		Vector2 axisXY = Vector3.zero;

		try {
			if(raw) {
				axisXY.x = Input.GetAxisRaw(xName);
				axisXY.y = -Input.GetAxisRaw(yName);
			}
			else {
				axisXY.x = Input.GetAxis(xName);
				axisXY.y = -Input.GetAxis(yName);
			}
		}
		catch {
			Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
		}

		//変換
		switch(axis) {
			case GamePad.Axis.LeftStick:
				break;
			case GamePad.Axis.RightStick:
				break;
			case GamePad.Axis.Dpad:
				axisXY.x *= -1;
				break;
			default:
				break;
		}

		return axisXY;
	}

	public static float GetTrigger(GamePad.Trigger trigger, GamePad.Index controlIndex, bool raw = false) {
		//Axisに渡す名前
		string name = "";

		controlIndex++;

		if(trigger == GamePad.Trigger.LeftTrigger) {
			name = "L_XBOX" + (int)controlIndex + "_Trigger";
		}
		else if(trigger == GamePad.Trigger.RightTrigger) {
			name = "R_XBOX" + (int)controlIndex + "_Trigger";
		}

		float axis = 0;

		try {
			if(raw)
				axis = Input.GetAxisRaw(name);
			else
				axis = Input.GetAxis(name);
		}
		catch(System.Exception e) {
			Debug.LogError(e);
			Debug.LogWarning("Have you set up all axes correctly? \nThe easiest solution is to replace the InputManager.asset with version located in the GamepadInput package. \nWarning: do so will overwrite any existing input");
		}

		return (axis + 1) * 0.5f;
	}

	public static bool GetButtonDown(GamePad.Button button, GamePad.Index controlIndex) {
		KeyCode code = GetKeycode(button, controlIndex);
		return Input.GetKeyDown(code);
	}

	public static bool GetButtonUp(GamePad.Button button, GamePad.Index controlIndex) {
		KeyCode code = GetKeycode(button, controlIndex);
		return Input.GetKeyUp(code);
	}

	public static bool GetButton(GamePad.Button button, GamePad.Index controlIndex) {
		KeyCode code = GetKeycode(button, controlIndex);
		return Input.GetKey(code);
	}

	static KeyCode GetKeycode(GamePad.Button button, GamePad.Index controlIndex) {
		switch(controlIndex) {
			case GamePad.Index.One:
				switch(button) {
					case GamePad.Button.A: return KeyCode.Joystick1Button0;
					case GamePad.Button.B: return KeyCode.Joystick1Button1;
					case GamePad.Button.X: return KeyCode.Joystick1Button2;
					case GamePad.Button.Y: return KeyCode.Joystick1Button3;
					case GamePad.Button.RightShoulder: return KeyCode.Joystick1Button5;
					case GamePad.Button.LeftShoulder: return KeyCode.Joystick1Button4;
					case GamePad.Button.Back: return KeyCode.Joystick1Button6;
					case GamePad.Button.Start: return KeyCode.Joystick1Button7;
					case GamePad.Button.LeftStick: return KeyCode.Joystick1Button8;
					case GamePad.Button.RightStick: return KeyCode.Joystick1Button9;
				}
				break;
			case GamePad.Index.Two:
				switch(button) {
					case GamePad.Button.A: return KeyCode.Joystick2Button0;
					case GamePad.Button.B: return KeyCode.Joystick2Button1;
					case GamePad.Button.X: return KeyCode.Joystick2Button2;
					case GamePad.Button.Y: return KeyCode.Joystick2Button3;
					case GamePad.Button.RightShoulder: return KeyCode.Joystick2Button5;
					case GamePad.Button.LeftShoulder: return KeyCode.Joystick2Button4;
					case GamePad.Button.Back: return KeyCode.Joystick2Button6;
					case GamePad.Button.Start: return KeyCode.Joystick2Button7;
					case GamePad.Button.LeftStick: return KeyCode.Joystick2Button8;
					case GamePad.Button.RightStick: return KeyCode.Joystick2Button9;
				}
				break;
			case GamePad.Index.Three:
				switch(button) {
					case GamePad.Button.A: return KeyCode.Joystick3Button0;
					case GamePad.Button.B: return KeyCode.Joystick3Button1;
					case GamePad.Button.X: return KeyCode.Joystick3Button2;
					case GamePad.Button.Y: return KeyCode.Joystick3Button3;
					case GamePad.Button.RightShoulder: return KeyCode.Joystick3Button5;
					case GamePad.Button.LeftShoulder: return KeyCode.Joystick3Button4;
					case GamePad.Button.Back: return KeyCode.Joystick3Button6;
					case GamePad.Button.Start: return KeyCode.Joystick3Button7;
					case GamePad.Button.LeftStick: return KeyCode.Joystick3Button8;
					case GamePad.Button.RightStick: return KeyCode.Joystick3Button9;
				}
				break;
			case GamePad.Index.Four:

				switch(button) {
					case GamePad.Button.A: return KeyCode.Joystick4Button0;
					case GamePad.Button.B: return KeyCode.Joystick4Button1;
					case GamePad.Button.X: return KeyCode.Joystick4Button2;
					case GamePad.Button.Y: return KeyCode.Joystick4Button3;
					case GamePad.Button.RightShoulder: return KeyCode.Joystick4Button5;
					case GamePad.Button.LeftShoulder: return KeyCode.Joystick4Button4;
					case GamePad.Button.Back: return KeyCode.Joystick4Button6;
					case GamePad.Button.Start: return KeyCode.Joystick4Button7;
					case GamePad.Button.LeftStick: return KeyCode.Joystick4Button8;
					case GamePad.Button.RightStick: return KeyCode.Joystick4Button9;
				}

				break;
		}
		return KeyCode.None;
	}
}