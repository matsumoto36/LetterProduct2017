using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : Unit {

	public GamePad.Index playerIndex;
	public ControlType inputType;

	// Use this for initialization
	public override void Awake() {
		base.Awake();

		moveSpeed = 5;
		rotSpeed = 10;

		//***武器のロード処理は後で移行する***
		//生成
		var w = Instantiate(Resources.Load<WeaponGun>("Weapon/TestWeapon"));
		//データ設定
		w.SetData(10, 0.1f, Resources.Load<BulletNormal>("Weapon/Bullet/TestBullet"), 10.0f);
		//装備
		EquipWeapon(w, 0);

		//生成
		var w2 = Instantiate(Resources.Load<WeaponLaser>("Weapon/TestWeaponLaser"));
		//データ設定
		w2.SetData(10, 1f, Resources.Load<BulletLaser>("Weapon/Bullet/TestBulletLaser"), 10.0f);
		//装備
		EquipWeapon(w2, 1);

	}

	// Update is called once per frame
	void FixedUpdate() {

		//移動処理
		Move();

		//武器の攻撃(左)
		if(equipWeapon[0]) {
			if(InputManager.GetButtonDown(inputType, GamePad.Button.LeftShoulder, playerIndex))
				equipWeapon[0].AttackStart();
			if(InputManager.GetButton(inputType, GamePad.Button.LeftShoulder, playerIndex))
				equipWeapon[0].Attack();
			if(InputManager.GetButtonUp(inputType, GamePad.Button.LeftShoulder, playerIndex))
				equipWeapon[0].AttackEnd();
		}

		//武器の攻撃(右)
		if(equipWeapon[1]) {
			if(InputManager.GetButtonDown(inputType, GamePad.Button.RightShoulder, playerIndex))
				equipWeapon[1].AttackStart();
			if(InputManager.GetButton(inputType, GamePad.Button.RightShoulder, playerIndex))
				equipWeapon[1].Attack();
			if(InputManager.GetButtonUp(inputType, GamePad.Button.RightShoulder, playerIndex))
				equipWeapon[1].AttackEnd();
		}

	}

	public override void Move() {

		//移動先
		var axis = InputManager.GetAxis(inputType, GamePad.Axis.LeftStick, playerIndex, true);
		moveVec.x = axis.x;
		moveVec.z = axis.y;

		unitRig.MovePosition(transform.position + moveVec * moveSpeed * Time.deltaTime);

		//回転の計算
		Vector3 plDir;
		if(inputType != ControlType.Keyboard) {
			plDir = InputManager.GetAxis(inputType, GamePad.Axis.RightStick, playerIndex, true);
			plDir.z = plDir.y;
			plDir.y = 0;
		}
		else {

			//初期設定
			var mainCam = Camera.main;
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

		//実際の回転
		if(plDir.magnitude != 0) {
			body.rotation =
				Quaternion.RotateTowards(body.rotation, Quaternion.LookRotation(plDir), rotSpeed);
		}

	}

	void OnDrawGizmo() {

	}
}
