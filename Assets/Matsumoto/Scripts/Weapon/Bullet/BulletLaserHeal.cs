using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// レーザー光線
/// </summary>
public class BulletLaserHeal : BulletLaser {

	public override void Init() {
		base.Init();

		//回復なので判定を書き換える
		var maskList = UnitGroupMatrix.GetAttackableGroup(bulletOwner.unitOwner.group)
			.Select((item) => item.ToString() + "Layer")
			.ToList();

		maskList.Add("BulletLayer");
		maskList.Add("IgnoreHit");

		foreach(var item in maskList) {
			Debug.Log(item);
		}

		bulletOwner.SetHitMask(maskList.ToArray());
	}

	public override void OnHitting(Collider other) {
		Irradiation(other.GetComponent<Unit>(), (unit, heal) => {
			Unit.Heal(bulletOwner.unitOwner, unit, heal);
		});
	}
}
