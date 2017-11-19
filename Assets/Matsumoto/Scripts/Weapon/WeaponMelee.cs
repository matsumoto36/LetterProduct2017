using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 近接武器
/// </summary>
[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class WeaponMelee : Weapon {

	int motionNum;
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
		unitOwner.PlayMeleeAnimation("Melee" + motionNum, motionSpeed, () => { });

		//intervalの設定(最後に呼ぶこと)
		base.Attack();
	}

	/// <summary>
	/// 必要なデータをセットする
	/// </summary>
	/// <param name="interval"></param>
	/// <param name="motionNum"></param>
	/// <param name="motionSpeed"></param>
	public void SetData(float interval, int motionNum, float motionSpeed) {
		this.interval = interval;
		this.motionNum = motionNum;
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

		//味方を除外する
		if(other.tag == "Player") return;

		Debug.Log("Hit:" + other.name);

	}
}
