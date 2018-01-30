using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重力に従って落ちるグレネード弾
/// </summary>
public class BulletGrenade : BulletNormal {

	public PhysicMaterial pMaterial;

	const float EXPLOSION_TIME = 2f;
	const float SPEED_MAG = 100f;
	const float DEC = 0.9f;
	const float NON_COL_TIME = 0.1f;

	Rigidbody rig;
	bool isWallCol = false;

	public override void Init() {
		base.Init();

		//消滅までの時間を上書き
		Destroy(gameObject, EXPLOSION_TIME);

		//当たり判定を追加
		Invoke("AddCollision", NON_COL_TIME);

		var bulletData = GetBulletData<BulletGrenadeData>();

		rig = GetComponent<Rigidbody>();
		rig.isKinematic = false;
		rig.useGravity = true;
		rig.AddForce(transform.forward * SPEED_MAG * bulletData.speed);
	}

	/// <summary>
	/// 当たり判定(実体)を追加
	/// </summary>
	void AddCollision() {
		var col = gameObject.AddComponent<SphereCollider>();
		var sc = transform.localScale;
		col.sharedMaterial = pMaterial;
		col.radius = Mathf.Max(sc.x, sc.y, sc.z) * 0.5f;
	}

	public override void FixedUpdate() {

		//壁に触れるまで減衰
		if(!isWallCol) {
			var vel = rig.velocity;
			vel.x *= DEC;
			vel.z *= DEC;
			rig.velocity = vel;
		}
	}

	public override void OnHitEnter(Collider other) {

		isWallCol = true;
		if(other.tag != "Enemy") return;

		//攻撃
		Unit.Attack(bulletOwner.unitOwner, other.GetComponent<Unit>(), bulletOwner.power);

		//爆発の処理
		Destroy(gameObject);
	}

	void OnDestroy() {

		var data = GetBulletData<BulletGrenadeData>();

		//エフェクトの再生
		Debug.Log(data.particleNameHit);
		ParticleManager.Spawn(data.particleNameHit, transform.position, transform.rotation);

		//SEの再生
		var se = AudioManager.PlaySE(bulletOwner.hitSound);
		se.transform.position = transform.position;

		//爆発
		Explosion.Create(bulletOwner, transform.position, data.expPow, data.expRadius);
	}
}