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

	const string ANIM_HAND_R = "[AnimHandR]";
	const string COMBO_STATUS_MOD = "COMBO_MOD";
	const string DURA_EGG_PREFAB_PATH = "System/DuraEgg";

	public int playerIndex;

	int combo = 0;
	float comboDuration = 0;

	Transform animHandR;

	StatusModifier comboStatus;
	bool isInDuraEgg = false;
	bool isWeak = false;

	Renderer[] rendererArray;
	Color headEmission;

	PKFxFX duraEgg;
	PKFxFX linkEffect;
	PKFxFX circleEffect;
	float ratio = 0.5f; //レバーの入力閾値


	public override void InitFirst() {
		base.InitFirst();

		DontDestroyOnLoad(gameObject);

		//描画を取得
		rendererArray = GetComponentsInChildren<Renderer>();
		foreach(var item in rendererArray) {
			item.material.EnableKeyword("_EMISSION");
		}
		headEmission = rendererArray[0].material.GetColor("_EmissionColor");

		//アニメーション用の右手を取得
		animHandR = transform.GetComponentsInChildren<Transform>()
			.Where((item) => item.name == ANIM_HAND_R)
			.ToArray()[0];
	}

	public override void InitFinal() {
		base.InitFinal();

		tag = "Player";
		gameObject.layer = LayerMask.NameToLayer("PlayerLayer");

		//コンボで上昇するステータスをパッシブ効果として実装
		comboStatus = new StatusModifier();
		ApplyModifier(comboStatus, COMBO_STATUS_MOD);

		//勢力のセット
		group = UnitGroup.Player;

		//ヒット通知
		OnAttackHit += (other) => {
			//総ダメージ加算
			GameData.instance.sumDamage[playerIndex] += other.attackedUnitList[other.attackedUnitList.Count - 1].damage;
			//コンボ加算
			AddCombo();
			};


		//アニメーションの初期状態をセット
		anim.SetBool("IsRanged", equipWeapon[0].weaponType == WeaponType.Ranged);
	}

	void Update() {

		if(isDead) return;

		//攻撃
		if(CheckCanAttack()) Attack();

		//コンボの処理
		ComboDurationUpdate();
		//武器交換
		CheckSwitchWeapon();
		//耐久卵の処理
		DurationEggUpdate();
		//復活の処理
		RiviveUpdate();

	}

	void FixedUpdate() {

		if(isDead) return;

		//移動処理
		Move();
	}

	/// <summary>
	/// 武器を交換するかどうか判断、交換する。
	/// </summary>
	void CheckSwitchWeapon() {

		if(!equipWeapon[1]) return;

		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.LeftTrigger, true) > ratio) {

			//攻撃キャンセル
			if(isAttack) equipWeapon[0].AttackEnd();
			if(SwitchWeapon(1.1f, 0.5f, 1, () => {
				//アニメーション用のHandから戻す
				foreach(var item in equipWeapon) {
					if(item.weaponType == WeaponType.Ranged) {
						item.transform.SetParent(handAnchor);
						item.transform.SetPositionAndRotation(handAnchor.position, handAnchor.rotation);
					}
				}
			})) {
				//アニメーション用のHandに移す
				foreach(var item in equipWeapon) {
					if(item.weaponType == WeaponType.Ranged) {
						item.transform.SetParent(animHandR);
						item.transform.SetPositionAndRotation(animHandR.position, animHandR.rotation);
					}
				}

				Debug.Log(equipWeapon[1].weaponType);
				anim.SetBool("IsRanged", equipWeapon[1].weaponType == WeaponType.Ranged);
			}
		}
	}

	/// <summary>
	/// 耐久卵の更新処理
	/// </summary>
	void DurationEggUpdate() {

		//HPが一定以下になったら耐久卵が使える
		if(!isInDuraEgg
			&& HPRatio < GameBalance.instance.data.duraEggCanUseRatio
			&& InputManager.GetButtonDown(playerIndex, GamePad.Button.A)) {

			duraEgg = ParticleManager.Spawn("DuraEgg", transform.position, Quaternion.identity, 0);

			var intensity = duraEgg.GetAttribute("Intensity");
			var endIntensity = intensity.ValueFloat;
	
			StartCoroutine(SkillWait(
				GameBalance.instance.data.duraEggChargeTime,
				() => InputManager.GetButton(playerIndex, GamePad.Button.A),
				(ratio) => {
					intensity.ValueFloat = endIntensity * ratio;
				},
				InDuraEgg,
				() => { Destroy(duraEgg.gameObject); }));
		}

		//耐久卵から出る
		if(isInDuraEgg
			&& InputManager.GetButtonDown(playerIndex, GamePad.Button.A)) {

			var intensity = duraEgg.GetAttribute("Intensity");
			var startIntensity = intensity.ValueFloat;

			StartCoroutine(SkillWait(
				GameBalance.instance.data.duraEggExitTime,
				() => InputManager.GetButton(playerIndex, GamePad.Button.A),
				(ratio) => {
					intensity.ValueFloat = startIntensity * (1 - ratio);
				},
				OutDuraEgg,
				() => { }));
		}
	}
	/// <summary>
	/// 耐久卵にこもる
	/// </summary>
	void InDuraEgg() {

		//攻撃と移動を無効化
		canMove = canAttack = false;

		//エフェクトを再生
		ParticleManager.Spawn("InDuraEgg", transform.position, Quaternion.identity);

		//アニメーションを止める
		anim.speed = 0;

		isInDuraEgg = true;
	}
	/// <summary>
	/// 耐久卵から出る
	/// </summary>
	void OutDuraEgg() {

		//攻撃と移動を有効化
		canMove = canAttack = true;

		//こもっているときのエフェクトを消す
		Destroy(duraEgg.gameObject);

		//エフェクトを再生
		ParticleManager.Spawn("OutDuraEgg", transform.position, Quaternion.identity);

		//アニメーションを再開
		anim.speed = 1;

		isInDuraEgg = false;

		//一定時間弱くなる
		if(isWeak) StopCoroutine("WeakTime");
		StartCoroutine(WeakTime(GameBalance.instance.data.duraEggExitWeakTime));
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
	/// 復活の更新処理
	/// </summary>
	void RiviveUpdate() {

		//近くに死んだ味方がいたら回復できる
		var rivaivablePlayer = GetRivaivablePlayer();
		if(rivaivablePlayer) {

			//つなぐ線のエフェクトを再生
			if(!linkEffect) linkEffect = ParticleManager.Spawn("RevivalLine", new Vector3(), Quaternion.identity, 0);
			linkEffect.GetAttribute("Position1").ValueFloat3 = transform.position;
			linkEffect.GetAttribute("Position2").ValueFloat3 = rivaivablePlayer.transform.position;

			//円のエフェクトを再生
			if(!circleEffect) {
				circleEffect = ParticleManager.Spawn("RevivalCircle", rivaivablePlayer.transform.position, Quaternion.identity, 0);
				circleEffect.transform.SetParent(rivaivablePlayer.transform);
			}
			var intensity = linkEffect.GetAttribute("Intensity");
			var startIntensity = intensity.ValueFloat;

			if(InputManager.GetButtonDown(playerIndex, GamePad.Button.B)) {

				//SE再生
				var se = AudioManager.PlaySE("Revive_Motion", autoDelete:false);
				se.loop = true;
				se.transform.position = transform.position;

				StartCoroutine(SkillWait(
					GameBalance.instance.data.reviveTime,
					() => InputManager.GetButton(playerIndex, GamePad.Button.B),
					(ratio) => { intensity.ValueFloat = startIntensity + Mathf.Abs(Mathf.Sin(Time.time * 4) * 2); },
					() => { RivivePlayer(rivaivablePlayer); Destroy(se.gameObject); },
					() => {
						intensity.ValueFloat = startIntensity;
						Destroy(se.gameObject);
					}));
			}

		}
		else {

			//削除
			if(linkEffect) {
				linkEffect.GetAttribute("State").ValueInt = 1;
				Destroy(linkEffect.gameObject, linkEffect.GetAttribute("FadeDuration").ValueFloat + 0.1f);
			}
			if(circleEffect) Destroy(circleEffect.gameObject, 0.1f);
		}
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

		//エフェクトの再生
		ParticleManager.Spawn("Revaive", target.transform.position, Quaternion.identity);

		//SE再生
		var se = AudioManager.PlaySE("Revive");
		se.transform.position = target.transform.position;

		//頭の光を点ける
		target.StartCoroutine(target.HeadLightSwitchAnim(true, 1));

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

	void OnAttackAnimStart() {

		var weapon = ((WeaponMelee)equipWeapon[0]);
		anim.speed = weapon.motionSpeed;

		//振るSEの再生
		var se = AudioManager.PlaySE(weapon.useSound);
		se.transform.position = transform.position;
	}

	void OnAttackAnimComplete() {
		anim.speed = 1;
	}

	/// <summary>
	/// 一定時間待つ
	/// </summary>
	/// <param name="waitTime"></param>
	/// <param name="predicate">判断条件</param>
	/// <param name="onComplete">成功時に実行される</param>
	/// <returns>時間</returns>
	IEnumerator SkillWait(float waitTime, Func<bool> predicate, Action<float> execute, Action onComplete, Action onFailed) {

		if(!predicate()) yield break;

		bool buff = canMove;
		//発動待機中は攻撃と移動ができない
		canMove = canAttack = false;

		float t = 0;
		while((t += Time.deltaTime) < waitTime) {

			execute(t / waitTime);

			//続けないとキャンセル
			if(!predicate()) {
				canMove = canAttack = buff;

				//失敗時に実行
				onFailed();
				yield break;
			}

			yield return t;
		}

		canMove = canAttack = true;

		//

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
	/// 頭の明かりをつけたり消したりする
	/// </summary>
	/// <param name="enable"></param>
	/// <param name="lightingTime"></param>
	/// <returns></returns>
	IEnumerator HeadLightSwitchAnim(bool enable, float lightingTime) {

		var startValue = enable ? 0.0f : 1.0f;
		var endValue   = enable ? 1.0f : 0.0f;

		var t = 0.0f;
		while((t += Time.deltaTime / lightingTime) < 1.0f) {

			for(int i = 0;i < rendererArray.Length;i++) {
				var color = headEmission * Mathf.Lerp(startValue, endValue, t);
				rendererArray[i].material.SetColor("_EmissionColor", color);
			}

			yield return null;
		}

		foreach(var item in rendererArray) {
			item.material.SetColor("_EmissionColor", headEmission * endValue);
		}
	}

	public override void Move() {

		if(!canMove) return;

		//移動先
		var axis = InputManager.GetAxis(playerIndex, GamePad.Axis.LeftStick, true);
		moveVec.x = axis.x;
		moveVec.z = axis.y;

		var newPos = moveVec * moveSpeed * Time.deltaTime;

		//移動量の差でアニメーションを決定
		anim.SetFloat("Speed", (newPos).magnitude);

		transform.position += newPos;

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


		int state;
		if(newPos.magnitude > 0.1f) {

			//移動方向と回転で走る向きを選択
			//0,4 後ろ 1 左 2 前 3 右
			var angle = Vector3.SignedAngle(body.forward, moveVec, Vector2.up);
			state = (int)((angle + 180) / 90 + 0.5f);
			if(state == 4) state = 0;
		}
		else {
			state = -1;
		}

		//走るモーションをセット
		anim.SetInteger("State", state);

	}

	/// <summary>
	/// 攻撃するかどうかも含め、各攻撃状態時の処理
	/// </summary>
	public override void Attack() {

		//攻撃開始
		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.RightTrigger, true) > ratio && !isAttack) {
			equipWeapon[0].AttackStart();
			isAttack = true;
		}
		//攻撃ループ
		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.RightTrigger, true) > ratio) {

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
		if(InputManager.GetTrigger(playerIndex, GamePad.Trigger.RightTrigger, true) <= ratio && isAttack) {
			equipWeapon[0].AttackEnd();
			isAttack = false;
		}
	}

	public override void Death() {
		base.Death();
		GameData.instance.isDeath[playerIndex] = true;
		Debug.Log("Player" + playerIndex + " is Dead");
		StopAllCoroutines();

		//死亡時は頭の光を消す
		StartCoroutine(HeadLightSwitchAnim(false, 2));
	}

	public override void EquipWeapon(Weapon weapon, int slot) {
		base.EquipWeapon(weapon, slot);

		//近接武器の場合は手の位置に移動
		if(weapon.weaponType == WeaponType.Melee) {
			weapon.transform.SetParent(animHandR);
			weapon.transform.localPosition = new Vector3();
			weapon.transform.localRotation = Quaternion.identity;
		}
	}

	protected override bool ApplyDamage(int damage) {

		//耐久卵の状態で変化
		if(isInDuraEgg) {

			//エフェクトを再生
			ParticleManager.Spawn("DuraEggHit", transform.position, Quaternion.identity);

			return false;
		}

		if(isWeak) damage = (int)(damage * GameBalance.instance.data.duraEggExitDamageMag);

		return base.ApplyDamage(damage);
	}
}
