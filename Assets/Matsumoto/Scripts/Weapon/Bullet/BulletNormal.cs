using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通常弾
/// </summary>
public class BulletNormal : Bullet {

	const float LIFETIME = 5.0f;

	public override void Init() {
		base.Init();
		Destroy(gameObject, LIFETIME);

	}

	public virtual void Update() {
		transform.position += transform.forward * bData.speed * Time.deltaTime;
	}

	public override void OnHitEnter(Collider other) {
		Destroy(gameObject);
	}
}