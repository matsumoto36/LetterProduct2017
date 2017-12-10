using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using GamepadInput;

/// <summary>
/// プレイヤーの入力で動くキャラクター
/// </summary>
public class Player : Unit {

	const string WEAPON_SWITCH_ANIM = "TestPlayerAnimationSwitch";
	const string DURA_EGG_PREFAB_PATH = "System/DuraEgg";

	public GamePad.Index playerIndex;
	public ControlType inputType;

	bool isInDuraEgg = false;
	bool isWeak = false;
	GameObject duraEggPrefab;
	GameObject duraEgg;
	float ratio = 0.5f;	//レバーの入力閾値


	public override void InitFinal() {
		base.InitFinal();

		tag = "Player";
		gameObject.layer = LayerMask.NameToLayer("PlayerLayer");

		//勢力のセット
		group = UnitGroup.Player;

		//耐久卵オブジェクトを取得
		duraEggPrefab = Resources.Load<GameObject>(DURA_EGG_PREFAB_PATH);
		if(!duraEggPrefab) Debug.LogError("Failed load DuraEgg.");
	}

	void Update() {

		if(isDead) return;

		if(Input.GetKeyDown(KeyCode.E)) GainEXP(1);
		if(Input.GetKeyDown(KeyCode.R)) GainEXP(10);
		if(Input.GetKeyDown(KeyCode.T)) GainEXP(100);

		//攻撃
		if(CheckCanAttack()) Attack();

		//武器交換
		CheckSwitchWeapon();

		//HPが一定以下になったら耐久卵が使える
		if(HPRatio < GameBalance.instance.data.duraEggCanUseRatio
			&& InputManager.GetButtonDown(inputType, GamePad.Button.A, playerIndex)) {

			StartCoroutine(SkillWait(
				GameBalance.instance.data.duraEggChargeTime,
				() => InputManager.GetButton(inputType, GamePad.Button.A, playerIndex),
				InDuraEgg));
		}

		//耐久卵から出る
		if(isInDuraEgg
			&& InputManager.GetButtonDown(inputType, GamePad.Button.A, playerIndex)) {

			StartCoroutine(SkillWait(
				GameBalance.instance.data.duraEggExitTime,
				() => InputManager.GetButton(inputType, GamePad.Button.A, playerIndex),
				OutDuraEgg));
		}

		//近くに死んだ味方がいたら回復できる
		var rivaivablePlayer = GetRivaivablePlayer();
		if(rivaivablePlayer) {
			Debug.DrawLine(transform.position, rivaivablePlayer.transform.position, Color.red);

			if(InputManager.GetButtonDown(inputType, GamePad.Button.B, playerIndex)) {

				StartCoroutine(SkillWait(
					GameBalance.instance.data.duraEggExitTime,
					() => InputManager.GetButton(inputType, GamePad.Button.B, playerIndex),
					() => RivivePlayer(rivaivablePlayer)));
			}

		}
	}

	// Update is called once per frame
	void FixedUpdate() {

		if(isDead) return;

		//移動処理
		Move();
	}

	void RivivePlayer(Player target) {
		if(!target || !target.isDead) return;

		Debug.Log(target + "is Rivive.");

		target.isDead = false;
		var healPoint = nowHP / 2;
		//回復してダメージ
		Heal(this, target, healPoint);
		ApplyDamage(healPoint);
	}

	Player GetRivaivablePlayer() {

		var rivPlayer = unitList
			.Where((item) => item is Player && item.isDead)
			.Select((item) => (Player)item)
			.ToArray();

		//復活できるプレイヤーがいない場合はnull
		if(rivPlayer.Length == 0) return null;

		//一番近いプレイヤーを検索
		Player nearestPlayer = null;
		float minLength = GameBalance.instance.data.revivableRadius;
		foreach(var item in rivPlayer) {

			var length = (item.transform.position - transform.position).magnitude;
			if(length < minLength) {
				minLength = length;
				nearestPlayer = item;
			}
		}

		return nearestPlayer;
	}

	/// <summary>
	/// 武器を交換するかどうか判断、交換する。
	/// </summary>
	void CheckSwitchWeapon() {

		if(!equipWeapon[1]) return;

		if(InputManager.GetTrigger(inputType, GamePad.Trigger.RightTrigger, playerIndex, true) > ratio) {

			//攻撃キャンセル
			if(isAttack) equipWeapon[0].AttackEnd();
			SwitchWeapon(WEAPON_SWITCH_ANIM, 1, () => { });
		}
	}

	/// <summary>
	/// 攻撃してよいか確認
	/// </summary>
	/// <returns></returns>
	bool CheckCanAttack() {
		if(!equipWeapon[0]) return false;
		if(!canAttack) return false;
		if(isPlayMeleeAnim) return false;

		return true;
	}

	/// <summary>
	/// 耐久卵にこもる
	/// </summary>
	void InDuraEgg() {

		//卵を生成
		if(duraEgg) Destroy(duraEgg);
		duraEgg = Instantiate(duraEggPrefab);
		duraEgg.transform.position = transform.position;
		duraEgg.transform.parent = transform;

		//攻撃と移動を無効化
		canMove = canAttack = false;

		isInDuraEgg = true;
	}

	/// <summary>
	/// 耐久卵から出る
	/// </summary>
	void OutDuraEgg() {

		//卵を破壊
		Destroy(duraEgg);

		//攻撃と移動を有効化
		canMove = canAttack = true;

		isInDuraEgg = false;

		//一定時間弱くなる
		if(isWeak) StopCoroutine("WeakTime");
		StartCoroutine(WeakTime(GameBalance.instance.data.duraEggExitWeakTime));
	}

	/// <summary>
	/// 一定時間待つ
	/// </summary>
	/// <param name="waitTime"></param>
	/// <param name="predicate">判断条件</param>
	/// <param name="onComplete">成功時に実行される</param>
	/// <returns>時間</returns>
	IEnumerator SkillWait(float waitTime, Func<bool> predicate, Action onComplete) {

		if(!predicate()) yield break;

		bool buff = canMove;
		//発動待機中は攻撃と移動ができない
		canMove = canAttack = false;

		float t = 0;
		while((t += Time.deltaTime) < waitTime) {

			//続けないとキャンセル
			if(!predicate()) {
				canMove = canAttack = buff;
				yield break;
			}

			yield return t;
		}

		canMove = canAttack = true;

		//成功時に実行
		onComplete();
	}

	/// <summary>
	/// 一定時間弱くなる
	/// </summary>
	/// <param name="weakTime"></param>
	/// <returns></returns>
	IEnumerator WeakTime(float weakTime) {

		isWeak = true;

		float t = 0;
		while((t += Time.deltaTime) < weakTime) {
			yield return null;
		}

		isWeak = false;
	}

	/// <summary>
	/// 攻撃するかどうかも含め、各攻撃状態時の処理
	/// </summary>
	public override void Attack() {

		//攻撃開始
		if(InputManager.GetTrigger(inputType, GamePad.Trigger.LeftTrigger, playerIndex, true) > ratio && !isAttack) {
			equipWeapon[0].AttackStart();
			isAttack = true;
		}
		//攻撃ループ
		if(InputManager.GetTrigger(inputType, GamePad.Trigger.LeftTrigger, playerIndex, true) > ratio) {

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
		if(InputManager.GetTrigger(inputType, GamePad.Trigger.LeftTrigger, playerIndex, true) <= ratio && isAttack) {
			equipWeapon[0].AttackEnd();
			isAttack = false;
		}
	}

	public override void Death() {
		base.Death();
		Debug.Log("Player" + playerIndex + " is Dead");
		StopAllCoroutines();
	}

	public override void Move() {

		if(!canMove) return;

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
		if(!isPlayMeleeAnim && plDir.magnitude != 0) {
			body.rotation =
				Quaternion.RotateTowards(body.rotation, Quaternion.LookRotation(plDir), rotSpeed);
		}

	}

	protected override bool ApplyDamage(int damage) {

		//耐久卵の状態で変化
		if(isInDuraEgg) return false;
		if(isWeak) damage = (int)(damage * GameBalance.instance.data.duraEggExitDamageMag);

		return base.ApplyDamage(damage);
	}
}
