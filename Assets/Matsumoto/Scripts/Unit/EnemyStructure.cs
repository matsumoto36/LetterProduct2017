using UnityEngine;
using System.Collections;
using System;
using System.Linq;

/// <summary>
/// 近くにいるプレイヤーに攻撃するだけの敵
/// </summary>
public class EnemyStructure : Enemy {

	const float TARGET_CHECK_INTERVAL = .5f;

	public float detectAngle;
	public float detectRadius;
	public bool isDetectAttacked;

	Unit target;
	Quaternion lookRotation;

	public override void InitFinal() {
		base.InitFinal();

		lookRotation = body.rotation;

		StartCoroutine(CheckTargetUpdate());
	}

	// Update is called once per frame
	void Update() {

		if(target && CheckCanAttack()) Attack();
		else if(isAttack) {
			equipWeapon[0].AttackEnd();
			isAttack = false;
		}

		Move();
	}

	/// <summary>
	/// 一番近い敵対キャラクターを取得
	/// </summary>
	/// <returns></returns>
	Unit GetNearestTarget() {

		var attackableGroup = UnitGroupMatrix.GetAttackableGroup(group);

		//攻撃可能なキャラクターを抽出
		var attackablelist = unitList
			.Where((item) => !item.isDead)
			.Where((item) => attackableGroup.Any((groupItem) => groupItem == item.group))
			.ToList();

		//一番近い検知範囲内のキャラクターを取得
		Unit unit = null;
		var minLength = detectRadius;
		foreach(var item in attackablelist) {
			var length = (item.transform.position - body.position).magnitude;
			if(length < minLength) {
				minLength = length;
				unit = item;
			}
		 }

		if(!unit) return null;
		if(Vector3.Angle(unit.transform.position - body.position, body.forward) > detectAngle / 2) return null;

		return unit;

	}

	/// <summary>
	/// ターゲットが検出できているか確認
	/// </summary>
	/// <returns></returns>
	bool CheckTrackTarget() {
		if(!target || target.isDead) return false;
		if(Vector3.Angle(target.transform.position - body.position, body.forward) > detectAngle / 2) return false;
		if((target.transform.position - body.position).magnitude > detectRadius) return false;

		return true;
	}

	IEnumerator CheckTargetUpdate() {

		if(!CheckTrackTarget()) target = GetNearestTarget();

		yield return new WaitForSeconds(TARGET_CHECK_INTERVAL);
		StartCoroutine(CheckTargetUpdate());
	}

	/// <summary>
	/// 撃ちっぱなし
	/// </summary>
	public override void Attack() {

		if(!equipWeapon[0]) return;

		if(!isAttack) {
			equipWeapon[0].AttackStart();
			isAttack = true;
		}

		equipWeapon[0].Attack();
	}

	public override void Move() {

		//回転のみ
		if(target) {
			var dir = target.transform.position - body.position;
			lookRotation = Quaternion.LookRotation(dir);
		}

		body.rotation = Quaternion.RotateTowards(body.rotation, lookRotation, rotSpeed);
	}

	protected override void OnAttacked(Unit from) {

		if(!isDetectAttacked) return;
		if(!from) return;
		if(target) return;

		//振り向く(検出ではない)
		var dir = from.transform.position - body.position;
		lookRotation = Quaternion.LookRotation(dir);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, detectRadius);
	}
}