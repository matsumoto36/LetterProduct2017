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

	protected override void Attack(Unit target) {
		//照射系ダメージ
		Irradiation(target, (unit, heal) => {

			//ヒットエフェクト再生
			if(!laserHitParticle) {
				laserHitParticle =
				ParticleManager.Spawn(GetBulletData<BulletLaserData>().particleNameHit, unit.transform.position, unit.transform.rotation, 0);
			}

			laserHitParticle.transform.SetPositionAndRotation(unit.transform.position, unit.transform.rotation);


			Unit.Heal(bulletOwner.unitOwner, unit, heal);
		});
	}

	public override void Update() {

		if(attackParticle) attackParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
		if(currentAttackTarget) Attack(currentAttackTarget.GetComponent<Unit>());
	}
}
