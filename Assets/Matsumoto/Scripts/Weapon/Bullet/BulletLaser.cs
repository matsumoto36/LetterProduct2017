using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// レーザー光線
/// </summary>
public class BulletLaser : Bullet {

	public PKFxFX laserHitParticle { get; set; }
	public AudioSource laserHitSE { get; set; }
	public GameObject currentAttackTarget { get; set; }

	PKFxManager.Attribute laserLengthAtt;
	Unit lastAttackUnit;
	float buffPoint = 0;

	public override void Init() {
		base.Init();

		laserLengthAtt = attackParticle.GetAttribute("Length");
		laserLengthAtt.ValueFloat = 0;
	}

	public float length {
		get { return _length; }
		set {
			var s = transform.localScale;
			s.z = laserLengthAtt.ValueFloat = _length = value;
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
		buffPoint += bulletOwner.power * Time.deltaTime;
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
			Unit.Attack(bulletOwner.unitOwner, target, damage);
		});
	}

	//public override void OnHitEnter(Collider other) {
	//	isHit = true;
	//	currentAttackUnit = other.GetComponent<Unit>();
	//	if(currentAttackUnit) Debug.Log(currentAttackUnit.name);
	//}

	//public override void OnHitExit(Collider other) {
	//	isHit = false;
	//}

	public override void Update() {
		base.Update();

		if(!currentAttackTarget) return;

		//ヒットエフェクト再生
		if(!laserHitParticle) {
			laserHitParticle =
			ParticleManager.Spawn(GetBulletData<BulletLaserData>().particleNameHit, new Vector3(), Quaternion.identity, 0);
		}
		laserHitParticle.transform.SetPositionAndRotation(transform.position + transform.forward * length, transform.rotation);

		//SE再生
		if(!laserHitSE) {
			laserHitSE = AudioManager.PlaySE(bulletOwner.hitSound, autoDelete: false);
			laserHitSE.loop = true;
			laserHitSE.transform.SetParent(transform);
			laserHitSE.transform.localPosition = new Vector3(0, 0, 0.25f);
		}

		if(currentAttackTarget) Attack(currentAttackTarget.GetComponent<Unit>());
	}
}
