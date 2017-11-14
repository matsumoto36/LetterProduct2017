using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重力に従って落ちるグレネード弾
/// </summary>
public class BulletGrenade : BulletNormal {

	public PhysicMaterial pMaterial;
	public float expTime = 2f;

	const float SPEED_MAG = 100f;
	const float DEC = 0.9f;
	const float NON_COL_TIME = 0.1f;

	Rigidbody rig;
	bool isWallCol = false;

	public override void Init() {
		base.Init();

		//消滅までの時間を上書き
		Destroy(gameObject, expTime);

		//当たり判定を追加
		Invoke("AddCollision", NON_COL_TIME);
		
		rig = body.GetComponent<Rigidbody>();
		rig.isKinematic = false;
		rig.useGravity = true;
		rig.AddForce(transform.forward * SPEED_MAG * bData.speed);
	}

	/// <summary>
	/// 当たり判定(実体)を追加
	/// </summary>
	void AddCollision() {
		var col = body.gameObject.AddComponent<SphereCollider>();
		var sc = body.transform.localScale;
		col.sharedMaterial = pMaterial;
		col.radius = Mathf.Max(sc.x, sc.y, sc.z) * 0.5f;
	}

	public override void Update() {

		//壁に触れるまで減衰
		if(!isWallCol) {
			var vel = rig.velocity;
			vel.x *= DEC;
			vel.z *= DEC;
			rig.velocity = vel;
		}
	}

	public override void OnHitEnter(Collider other) {

		//if(other.tag == "Player") return;
		//if(other.tag == "Bullet") return;

		isWallCol = true;
		if(other.tag != "Enemy") return;

		//爆発の処理
		Destroy(gameObject);
	}

	void OnDestroy() {
		Debug.Log("Explosion");
	}
}
