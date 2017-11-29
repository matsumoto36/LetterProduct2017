using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

	public float defaultAttackDuration { get; set; }

	public override void Awake() {
		base.Awake();

		defaultAttackDuration = 2;

		//適当に装備
		EquipWeapon(WeaponDataContainer.CreateWeapon(0), 0);
		EquipWeapon(WeaponDataContainer.CreateWeapon(0), 1);

        //パッシブ効果の適用
        CalcStatus();

		Debug.Log("EnemyInitEnd");
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

		//即時に交換
		var w = equipWeapon[0];
		equipWeapon[0] = equipWeapon[weaponNum];
		equipWeapon[weaponNum] = w;
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

	public override void Move() {
		//* 移動処理はAIで実装？ *//
	}

	/// <summary>
	/// 一定時間攻撃する
	/// </summary>
	/// <returns></returns>
	IEnumerator Attacking(float duration) {

		//攻撃中なら中止
		if(isAttack) yield break;

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
