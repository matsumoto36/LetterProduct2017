using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー光線
/// </summary>
public class BulletLaser : Bullet {

	Unit lastAttackUnit;
	float buffDamage = 0;
	float oldTime = 0;

	public override void Init() {
		base.Init();
	}

	public float length {
		get { return _length; }
		set {
			var s = transform.localScale;
			s.z = _length = value;
			transform.localScale = s;
		}
	}
	float _length;

	protected override void Attack(Unit target) {

		if(!target) return;

		if(lastAttackUnit != target) {
			buffDamage = 0;
			oldTime = Time.time;
		}

		lastAttackUnit = target;
		buffDamage += bData.bulletOwner.power * (Time.time - oldTime);
		Debug.Log("buffDamage:" + buffDamage);
		//ダメージ量を合計して1以上になったら実際に攻撃
		if(buffDamage >= 1) {
			var damage = (int)buffDamage;
			buffDamage -= damage;
			Unit.Attack(bData.bulletOwner.unitOwner, target, damage);
		}

		oldTime = Time.time;
	}

	public override void OnHitting(Collider other) {
		Attack(other.GetComponent<Unit>());
	}
}
