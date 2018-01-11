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

	const string COMBO_STATUS_MOD = "COMBO_MOD";

	const string WEAPON_SWITCH_ANIM = "TestPlayerAnimationSwitch";
	const string DURA_EGG_PREFAB_PATH = "System/DuraEgg";

	public int playerIndex;

	int combo = 0;
	float comboDuration = 0;

	StatusModifier comboStatus;
	bool isInDuraEgg = false;
	bool isWeak = false;
	GameObject duraEggPrefab;
	GameObject duraEgg;
	float ratio = 0.5f;	//レバーの入力閾値


	public override void InitFinal() {
		base.InitFinal();

		tag = "Player";
		gameObject.layer = LayerMask.NameToLayer("PlayerLayer");

		//コンボで上昇するステータスをパッシブ効果として実装
		comboStatus = new StatusModifier();
		ApplyModifier(comboStatus, COMBO_STATUS_MOD);

		//勢力のセット
		group = UnitGroup.Player;

		//耐久卵オブジェクトを取得
		duraEggPrefab = Resources.Load<GameObject>(DURA_EGG_PREFAB_PATH);
		if(!duraEggPrefab) Debug.LogError("Failed load DuraEgg.");
	}

	void Update() {

		if(isDead) return;



		//攻撃
		if(CheckCanAttack()) Attack();

		//コンボの処理
		ComboDurationUpdate();

		//武器交換
		CheckSwitchWeapon();

		//HPが一定以下になったら耐久卵が使える
		if(HPRatio < GameBalance.instance.data.duraEggCanUseRatio
			&& InputManager.GetButtonDown(playerIndex, GamePad.Button.A)) {

			StartCoroutine(SkillWait(
				GameBalance.instance.data.duraEggChargeTime,
				() => InputManager.GetButton(playerIndex, GamePad.Button.A),
				InDuraEgg));
		}

		//耐久卵から出る
		if(isInDuraEgg
			&& InputManager.GetButtonDown(playerIndex, GamePad.Button.A)) {

			StartCoroutine(SkillWait(
				GameBalance.instance.data.duraEggExitTime,
				() => InputManager.GetButton(playerIndex, GamePad.Button.A),
				OutDuraEgg));
		}

		//近くに死んだ味方がいたら回復できる
		var rivaivablePlayer = GetRivaivablePlayer();
		if(rivaivablePlayer) {
			Debug.DrawLine(transform.position, rivaivablePlayer.transform.position, Color.red);

			if(InputManager.GetButtonDown(playerIndex, GamePad.Button.B)) {

				StartCoroutine(SkillWait(
					GameBalance.instance.data.duraEggExitTime,
					() => InputManager.GetButton(playerIndex, GamePad.Button.B),
					() => RivivePlayer(rivaivablePlayer)));
			}

		}
	}

	void FixedUpdate() {

		if(isDead) return;

		//移動処理
		Move();
	}

	/// <summary>
	/// プレイヤーを復活させる
	/// </summary>
	/// <param name="target"></param>
	void RivivePlayer(Player target) {
		if(!target || !target.isDead) return;

		Debug.Log(target + "is Rivive.");

		target.isDead = false;
		var healPoint = nowHP / 2;
		//回復してダメージ
		Heal(this, target, healPoint);
		ApplyDamage(healPoint);
	}

	/// <summary>
	/// 復活可能なプレイヤーを取得する
	/// </summary>
	/// <returns></returns>
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

		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.RightTrigger, true) > ratio) {

			//攻撃キャンセル
			if(isAttack) equipWeapon[0].AttackEnd();
			SwitchWeapon(WEAPON_SWITCH_ANIM, 1, () => { });
		}
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
	/// コンボを加算する
	/// </summary>
	void AddCombo() {

		combo++;
		//ステータスの強化
		GameBalance.ApplyNextComboStatus(comboStatus, combo);
		CalcStatus();

		//コンボが途切れるまで待つ
		comboDuration = GameBalance.CalcNextComboDuration(combo);
	}

	/// <summary>
	/// コンボをリセットする
	/// </summary>
	void ResetCombo() {
		combo = 0;
		comboStatus = new StatusModifier();
	}

	/// <summary>
	/// コンボが途切れるまで待つ
	/// </summary>
	/// <param name="comboDurationTime"></param>
	/// <returns></returns>
	void ComboDurationUpdate() {

		if(combo == 0) return;

		comboDuration -= Time.deltaTime;

		//持続時間が過ぎたらコンボをリセット
		if(comboDuration < 0) ResetCombo();
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
		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.LeftTrigger, true) > ratio && !isAttack) {
			equipWeapon[0].AttackStart();
			isAttack = true;
		}
		//攻撃ループ
		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.LeftTrigger, true) > ratio) {

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
		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.LeftTrigger, true) <= ratio && isAttack) {
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
		var axis = InputManager.GetAxis(playerIndex, GamePad.Axis.LeftStick, true);
		moveVec.x = axis.x;
		moveVec.z = axis.y;

		//unitRig.MovePosition(transform.position + moveVec * moveSpeed * Time.deltaTime);
		transform.position += moveVec * moveSpeed * Time.deltaTime;

		//回転の計算
		Vector3 plDir;
		if(InputManager.GetControllerData(playerIndex).type != ControlType.Keyboard) {
			plDir = InputManager.GetAxis(playerIndex, GamePad.Axis.RightStick, true);
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

	protected override void OnAttackHit(Unit to) {
		AddCombo();
	}

	protected override bool ApplyDamage(int damage) {

		//耐久卵の状態で変化
		if(isInDuraEgg) return false;
		if(isWeak) damage = (int)(damage * GameBalance.instance.data.duraEggExitDamageMag);

		return base.ApplyDamage(damage);
	}
}
