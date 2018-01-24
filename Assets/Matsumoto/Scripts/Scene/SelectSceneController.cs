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

	// Use this for initialization
	void Start () {

		for(int i = 0;i < entryControllerID.Length;i++) {
			entryControllerID[i] = -1;
		}

		ControlButtonController.DestroyController(-1);
	}

	void Update() {

		int joystickNum;
		if(InputManager.GetButtonAny(entryButton, out joystickNum)) {
			Entry(joystickNum);
		}


	}

	void Entry(int joystickNum) {

		if(playerCount >= InputManager.MAX_PAYER_NUM) return;

		if(entryControllerID
			.Where(item => item == joystickNum)
			.Count() > 0) return;

		entryControllerID[playerCount] = joystickNum;
		playerCount++;


	}

}
