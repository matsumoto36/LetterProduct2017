using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 近接武器
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class WeaponMelee : Weapon {

	string motionName;
	float motionSpeed;
	Collider meleeCol;

	public override void Init() {
		base.Init();

		meleeCol = GetComponent<Collider>();
		//あらかじめ当たり判定を無効にしておく
		meleeCol.enabled = false;
	}

	public override void Attack() {

		//親クラスで攻撃ができるかの判断を行っている
		if(!canAction) return;

		//アニメーションを再生
		unitOwner.PlayMeleeAnimation(motionName, motionSpeed * unitOwner.statusMod.mulAttackSpeed, () => { });

		//intervalの設定(最後に呼ぶこと)
		base.Attack();
	}

	/// <summary>
	/// 必要なデータをセットする
	/// </summary>
	/// <param name="interval"></param>
	/// <param name="motionNum"></param>
	/// <param name="motionSpeed"></param>
	public void SetData(float interval, int power, string motionName, float motionSpeed) {
		baseInterval = interval;
		basePower = power;
		this.motionName = motionName;
		this.motionSpeed = motionSpeed;
	}

	/// <summary>
	/// 当たり判定を有効・無効にする
	/// </summary>
	/// <param name="enabled"></param>
	public void SetCollider(bool enabled) {
		meleeCol.enabled = enabled;
	}

	void OnTriggerEnter(Collider other) {

		Unit unit;

		if(unit = other.GetComponent<Unit>()) {
			//攻撃
			if(CheckHit(other.gameObject))
				Unit.Attack(unitOwner, unit, power);
		}
	}
}
