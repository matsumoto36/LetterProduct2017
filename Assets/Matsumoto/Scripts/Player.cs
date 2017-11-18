using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

public class Player : Unit {

	const string WEAPON_SWITCH_ANIM = "SwitchWeapon";

	public GamePad.Index playerIndex;
	public ControlType inputType;
	
	// Use this for initialization
	public override void Awake() {
		base.Awake();

		moveSpeed = 5;
		rotSpeed = 10;

		//***武器のロード処理は後で移行する***

		////武器の生成
		//var weapon = Instantiate(Resources.Load<GameObject>("Model/Weapon/TestWeaponGrenade")).AddComponent<WeaponGun>();

		////弾データの作成
		//var bullet = Resources.Load<Bullet>("System/Weapon/Bullet/BulletNormal");
		//var model = Resources.Load<GameObject>("Model/Weapon/Bullet/TestBulletNormal");
		//var bData = new BulletData(bullet, weapon, model);
		//bData.SetBulletDataGrenade(10, 10, 10, 10);

		////武器のデータ設定
		//weapon.SetData(0.1f, bData);
		////装備
		//EquipWeapon(weapon, 0);

		//武器の生成
		var weapon = Instantiate(Resources.Load<GameObject>("Model/Weapon/TestWeaponSword")).AddComponent<WeaponMelee>();
		//武器のデータ設定
		weapon.SetData(1f, 1) ;
		//装備
		EquipWeapon(weapon, 0);

		//武器の生成
		var weapon2 = Instantiate(Resources.Load<GameObject>("Model/Weapon/TestWeaponLaser")).AddComponent<WeaponLaser>();

		//弾データの作成
		var bullet2 = Resources.Load<Bullet>("System/Weapon/Bullet/BulletLaser");
		var model2 = Resources.Load<GameObject>("Model/Weapon/Bullet/TestBulletLaser2");
		var bData2 = new BulletData(bullet2, weapon2, model2);
		bData2.SetBulletDataLaser(10, 20);

		//武器のデータ設定
		weapon2.SetData(1f, bData2);
		//装備
		EquipWeapon(weapon2, 1);

		Debug.Log("PlayerInit");
	}

	void Update() {

		//攻撃
		if(!equipWeapon[0]) return;
		if(!canAttack) return;
		Attack();

		//武器交換
		if(!equipWeapon[1]) return;
		CheckSwitchWeapon();
	}

	// Update is called once per frame
	void FixedUpdate() {

		//移動処理
		Move();
	}

	/// <summary>
	/// 攻撃するかどうかも含め、各攻撃状態時の処理
	/// </summary>
	void Attack() {

		//攻撃開始
		if(InputManager.GetButtonDown(inputType, GamePad.Button.LeftShoulder, playerIndex)) {
			equipWeapon[0].AttackStart();
			isAttack = true;
		}
		//攻撃ループ
		if(InputManager.GetButton(inputType, GamePad.Button.LeftShoulder, playerIndex)) {

			//攻撃キャンセル復帰用
			if(!isAttack) {
				equipWeapon[0].AttackStart();
				isAttack = true;
			}
			else {
				equipWeapon[0].Attack();
			}
		}
		//攻撃終了
		if(InputManager.GetButtonUp(inputType, GamePad.Button.LeftShoulder, playerIndex)) {
			equipWeapon[0].AttackEnd();
			isAttack = false;
		}
	}

	/// <summary>
	/// 武器を交換するかどうか判断、交換する。
	/// </summary>
	void CheckSwitchWeapon() {

		if(InputManager.GetButtonDown(inputType, GamePad.Button.RightShoulder, playerIndex)) {

			//攻撃キャンセル
			if(isAttack) equipWeapon[0].AttackEnd();
			SwitchWeapon(WEAPON_SWITCH_ANIM, 1, () => { });
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
		if(!isLockRotation && plDir.magnitude != 0) {
			body.rotation =
				Quaternion.RotateTowards(body.rotation, Quaternion.LookRotation(plDir), rotSpeed);
		}

	}


	void OnDrawGizmo() {

	}
}
