using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー光線
/// </summary>
public class BulletLaser : Bullet {

	Unit lastAttackUnit;
	float buffPoint = 0;

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

	/// <summary>
	/// 継続的にヒットするとき
	/// </summary>
	/// <param name="target"></param>
	/// <param name="onApply">ダメージ等を与えるとき</param>
	protected void Irradiation(Unit target, Action<Unit, int> onApply) {

		if(!target) return;

		if(lastAttackUnit != target) {
			buffPoint = 0;
		}

		lastAttackUnit = target;
		buffPoint += bData.bulletOwner.power * Time.deltaTime;
		//与える量を合計して1以上になったら実際に攻撃
		if(buffPoint >= 1) {
			var point = (int)buffPoint;
			buffPoint -= point;

			//適用
			onApply(target, point);
		}
	}

	protected override void Attack(Unit target) {
		//照射系ダメージ
		Irradiation(target, (unit, damage) => {
			Unit.Attack(bData.bulletOwner.unitOwner, target, damage);
		});
	}

	public override void OnHitting(Collider other) {
		Attack(other.GetComponent<Unit>());
	}
}
