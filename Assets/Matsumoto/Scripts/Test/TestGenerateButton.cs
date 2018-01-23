using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerateButton : MonoBehaviour {

	public ControlButton initSelectButton;

	// Use this for initialization
	void Start() {
		//InputManager.SetControllerData(0, ControlType.GamePadXBOX, GamepadInput.GamePad.Index.One);
		var controller = ControlButtonController.CreateController(-1);
		controller.Focus(initSelectButton);
	}

}
