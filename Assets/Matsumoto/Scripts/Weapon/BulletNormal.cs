using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通常弾
/// </summary>
public class BulletNormal : Bullet {

	const float LIFETIME = 5.0f;

	void Start() {


		Destroy(gameObject, LIFETIME);
	}

	void Update() {
		transform.position += transform.forward * speed * Time.deltaTime;
	}
}