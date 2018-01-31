using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通常弾
/// </summary>
public class BulletNormal : Bullet {

	float length = 0;
	float maxLength;
	float speed;
	string hitParticleName;

	public override void Init() {
		base.Init();
		var data = GetBulletData<BulletNormalData>();
		speed = data.speed;
		maxLength = data.range;
		hitParticleName = data.particleNameHit;

	}

	void OnDestroy() {

		//再生中のパーティクルを止める
		attackParticle.transform.parent = null;
		attackParticle.TerminateEffect();
		Destroy(attackParticle.gameObject, 0.5f);   //少しずらさないと、なぜか進み続けてしまう

	}

	public virtual void FixedUpdate() {

		var spd = speed * Time.deltaTime;
		if((length += spd) > maxLength) Destroy(gameObject);

		transform.position += transform.forward * spd;
	}

	public override void OnHitEnter(Collider other) {
		Attack(other.GetComponent<Unit>());

		//ヒットパーティクルの再生
		ParticleManager.Spawn(hitParticleName, transform.position, Quaternion.identity);

		//SEの再生
		var se = AudioManager.PlaySE(bulletOwner.hitSound);
		se.transform.position = other.ClosestPointOnBounds(transform.position);

		Destroy(gameObject);
	}
}