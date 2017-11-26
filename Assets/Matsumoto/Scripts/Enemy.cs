using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

	public float attackDuration { get; set; }

	public override void Awake() {
		base.Awake();

		//適当に装備
		EquipWeapon(WeaponDataContainer.CreateWeapon(1), 0);

		//パッシブ効果の適用
		CalcStatus();

		Debug.Log("EnemyInitEnd");
	}

	/// <summary>
	/// 攻撃をキャンセルする
	/// </summary>
	public void AttackCancel() {
		if(isAttack) {

			StopCoroutine(Attacking(attackDuration));
			equipWeapon[0].AttackEnd();
			isAttack = false;
		}
	}

	public override void Attack() {
		StartCoroutine(Attacking(attackDuration));
	}

	public override void Move() {
		
	}

	/// <summary>
	/// 一定時間攻撃する
	/// </summary>
	/// <returns></returns>
	IEnumerator Attacking(float duration) {

		isAttack = true;
		equipWeapon[0].AttackStart();

		do {
			equipWeapon[0].Attack();
			yield return null;
		} while((duration -= Time.deltaTime) > 0);

		isAttack = false;
		equipWeapon[0].AttackEnd();
	}
}
