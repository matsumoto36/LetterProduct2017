using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通常弾
/// </summary>
public class BulletNormal : Bullet {

	const float LIFETIME = 5.0f;

	float speed;
	string hitParticleName;

	public override void Init() {
		base.Init();
		Destroy(gameObject, LIFETIME);

		var data = GetBulletData<BulletNormalData>();
		speed = data.speed;
		hitParticleName = data.particleNameHit;
	}

	public virtual void FixedUpdate() {
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	public override void OnHitEnter(Collider other) {
		Attack(other.GetComponent<Unit>());

		//再生中のパーティクルを止める
		attackParticle.transform.parent = null;
		Destroy(attackParticle.gameObject, 0.1f);	//少しずらさないと、なぜか進み続けてしまう

		//ヒットパーティクルの再生
		ParticleManager.Spawn(hitParticleName, transform.position, Quaternion.identity);
		Destroy(gameObject);

		//SEの再生
		var se = AudioManager.PlaySE(bulletOwner.hitSound);
		se.transform.position = other.ClosestPointOnBounds(transform.position);
	}
}