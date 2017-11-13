using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通常弾
/// </summary>
public class BulletNormal : Bullet {

	const float LIFETIME = 5.0f;

	public virtual void Start() {

		Destroy(gameObject, LIFETIME);
	}

	public virtual void Update() {
		transform.position += transform.forward * bData.speed * Time.deltaTime;
	}

	public override void OnTriggerEnter(Collider other) {

		if(other.tag == "Player") return;
		if(other.tag == "Bullet") return;

			Destroy(gameObject);
	}
}