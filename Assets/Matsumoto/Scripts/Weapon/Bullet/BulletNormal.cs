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

	public void Death() {

		//再生中のパーティクルを止める
		attackParticle.transform.parent = null;
		attackParticle.TerminateEffect();
		Destroy(attackParticle.gameObject, 0.5f);   //少しずらさないと、なぜか進み続けてしまう

		Destroy(gameObject);
	}

	public virtual void FixedUpdate() {

		var spd = speed * Time.deltaTime;
		if((length += spd) > maxLength) Death();

		transform.position += transform.forward * spd;
	}

	public override void OnHitEnter(Collider other) {
		Attack(other.GetComponent<Unit>());

		//ヒットパーティクルの再生
		ParticleManager.Spawn(hitParticleName, transform.position, Quaternion.identity);

		//SEの再生
		var se = AudioManager.PlaySE(bulletOwner.hitSound);
		if(se) se.transform.position = other.ClosestPointOnBounds(transform.position);

		Death();
	}
}