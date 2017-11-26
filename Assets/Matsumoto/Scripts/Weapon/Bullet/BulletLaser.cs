using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー光線
/// </summary>
public class BulletLaser : Bullet {

	float buffDamage = 0;

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
		if((buffDamage += bData.bulletOwner.power * Time.deltaTime) >= 1) {
			if(!target) return;
			//ダメージ量を合計して1を超えた時に実際に攻撃
			var damage = (int)buffDamage;
			buffDamage -= damage;
			Unit.Attack(bData.bulletOwner.unitOwner, target, damage);
		}
	}

	public override void OnHitting(Collider other) {
		Attack(other.GetComponent<Unit>());
	}
}
