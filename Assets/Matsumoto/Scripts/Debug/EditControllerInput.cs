using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class EditControllerInput : MonoBehaviour {

	public int playerNum;
	public ControlType controllerType;
	public GamePad.Index controllerIndex;

	public bool setButton;

	void Update() {
		if(setButton) SetController();
	}

	void SetController() {
		setButton = false;
		Debug.Log(string.Format("SetControllerData [{0}P : ({1}, joystick{2})]", playerNum, controllerType, (int)controllerIndex));
		InputManager.SetControllerData(playerNum, controllerType, controllerIndex);
	}
	
}
