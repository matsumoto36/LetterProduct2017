using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : Unit {

	public GamePad.Index playerIndex;
	public ControlType inputType;

	public float rotSpeed;

	// Use this for initialization
	public override void Awake() {
		base.Awake();
	}

	// Update is called once per frame
	public override void FixedUpdate () {
		base.FixedUpdate();
	}

	public override void Move() {

		//移動先
		var axis = InputManager.GetAxis(inputType, GamePad.Axis.LeftStick, playerIndex, true);
		moveVec.x = axis.x;
		moveVec.z = axis.y;

		//回転
		Vector3 plDir;
		if(inputType != ControlType.Keyboard) {
			plDir = InputManager.GetAxis(inputType, GamePad.Axis.RightStick, playerIndex, true);
			plDir.z = plDir.y;
			plDir.y = 0;
		}
		else {

			//初期設定
			Camera mainCam = Camera.main;
			var camTransform = mainCam.transform;
			var eyeOffset = 0.5f;

			var plPos = transform.position;
			plPos.y += eyeOffset;

			//マウスのベクトルを得る
			var screenMousePos = Input.mousePosition;
			screenMousePos.z = 10;
			var camVec = 
				(mainCam.ScreenToWorldPoint(screenMousePos) - camTransform.position).normalized;

			//ベクトルと縦軸の角度を調べ、縦軸の長さを調べる
			var deg = Vector3.Angle(camVec, new Vector3(0, -1, 0));
			var dHeight = Mathf.Cos(deg * Mathf.Deg2Rad);

			//比率からマウスの位置を計算
			var ratio = (camTransform.position.y - plPos.y) / dHeight;
			var mousePos = camTransform.position + camVec * ratio;

			//マウスの位置からプレイヤーの向きを決める
			plDir = (mousePos - plPos).normalized;
		}

		//実際の回転 TODO
		if(plDir.magnitude != 0) {
			body.transform.rotation = 
				Quaternion.RotateTowards(body.transform.rotation, Quaternion.LookRotation(plDir), rotSpeed);
		}

		base.Move();
	}

	void OnDrawGizmo() {

	}
}
