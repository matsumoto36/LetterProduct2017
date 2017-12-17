using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIで動く敵のキャラクター
/// </summary>
public class Enemy : Unit {

	public float defaultAttackDuration { get; set; }

	public override void InitFinal() {
		base.InitFinal();

		tag = "Enemy";
		gameObject.layer = LayerMask.NameToLayer("EnemyLayer");

		//勢力のセット
		group = UnitGroup.Enemy;

		defaultAttackDuration = 1;
	}

	/// <summary>
	/// 攻撃をキャンセルする
	/// </summary>
	public void AttackCancel() {
		if(isAttack) {

			StopCoroutine(Attacking(defaultAttackDuration));
			equipWeapon[0].AttackEnd();
			isAttack = false;
		}
	}

	/// <summary>
	/// 武器をノーモーションですぐ交換する
	/// </summary>
	/// <param name="weaponNum"></param>
	public void SwitchWeapon(int weaponNum) {

		if(weaponNum == 0) return;
		if(!equipWeapon[0]) return;
		if(!equipWeapon[weaponNum]) return;
		if(isPlayMeleeAnim) return;

		//攻撃を終了する
		if(isAttack) equipWeapon[0].AttackEnd();
		isAttack = false;

		switchingWeaponNum = weaponNum;
		OnSwitchWeaponModel();

		//即時に交換
		var w = equipWeapon[0];
		equipWeapon[0] = equipWeapon[weaponNum];
		equipWeapon[weaponNum] = w;
		switchingWeaponNum = 0;
		equipWeapon[0].OnSwitchActive();

	}

	public override void Move() {
		//* 移動処理はAIで実装？ *//
	}

	/// <summary>
	/// 指定された時間攻撃する
	/// </summary>
	/// <param name="duration"></param>
	public void Attack(float duration) {
		StartCoroutine(Attacking(duration));
	}

	/// <summary>
	/// defaultAttackDurationで設定されている時間の間攻撃する
	/// 初期値は1秒。
	/// </summary>
	public override void Attack() {
		StartCoroutine(Attacking(defaultAttackDuration));
	}

	public override void Death() {
		base.Death();

		//敵の死亡時は現時点では削除
		Destroy(gameObject);
	}

	/// <summary>
	/// 一定時間攻撃する
	/// </summary>
	/// <returns></returns>
	IEnumerator Attacking(float duration) {

		//攻撃してよいか調べる
		if(!CheckCanAttack() || isAttack) yield break;

		isAttack = true;
		equipWeapon[0].AttackStart();

		//設定した時間の間、攻撃し続ける
		do {
			equipWeapon[0].Attack();
			yield return null;
		} while((duration -= Time.deltaTime) > 0);

		isAttack = false;
		equipWeapon[0].AttackEnd();
	}
}
