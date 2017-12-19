using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通常弾
/// </summary>
public class BulletNormal : Bullet {

	const float LIFETIME = 5.0f;

	float speed;

	public override void Init() {
		base.Init();
		Destroy(gameObject, LIFETIME);

		speed = GetBulletData<BulletNormalData>().speed;
	}

	public virtual void Update() {
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	public override void OnHitEnter(Collider other) {
		Attack(other.GetComponent<Unit>());
		Destroy(gameObject);
	}
}