using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー光線
/// </summary>
public class BulletLaserHeal : BulletLaser {

	public override void Init() {
		base.Init();

		//回復なので判定を書き換える
		bData.bulletOwner.SetHitMask("EnemyLayer", "BulletLayer");
	}

	public override void OnHitting(Collider other) {
		Heal(other.GetComponent<Unit>());
	}
}
