using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり判定メソッドを親クラスに移譲するクラス
/// </summary>
public class BulletCollision : MonoBehaviour {

	public Bullet parent { get; set; }

	void OnTriggerEnter(Collider other) {
		parent.OnTriggerEnter(other);
	}
}
