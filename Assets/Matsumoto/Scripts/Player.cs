using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : Unit {

	public GamePad.Index playerIndex;
	public ControlType inputType;

	Quaternion playerRot;

	// Use this for initialization
	public override void Awake() {
		base.Awake();

		playerRot = transform.rotation;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	public override void Move() {

		//移動先
		var axis = InputManager.GetAxis(inputType, GamePad.Axis.LeftStick, playerIndex, true);
		moveVec.x = axis.x;
		moveVec.z = axis.y;

		//回転
		Vector2 plDir;
		if(inputType != ControlType.Keyboard) {
			plDir = InputManager.GetAxis(inputType, GamePad.Axis.RightStick, playerIndex, true);
		}
		else {
			plDir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
			plDir.Normalize();
		}
		body.transform.rotation = Quaternion.Euler(plDir);

		base.Move();
	}
}
