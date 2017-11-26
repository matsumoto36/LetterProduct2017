using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり判定メソッドを親クラスに移譲するクラス
/// </summary>
public class BulletCollision : MonoBehaviour {

	public Bullet parent { get; set; }

	void OnTriggerEnter(Collider other) {
		//ヒットしない定義のものだったらスキップ
		if(parent.bData.bulletOwner.CheckHit(other.gameObject)) parent.OnHitEnter(other);
	}
	void OnTriggerStay(Collider other) {
		if(parent.bData.bulletOwner.CheckHit(other.gameObject)) parent.OnHitting(other);
	}
	void OnTriggerExit(Collider other) {
		if(parent.bData.bulletOwner.CheckHit(other.gameObject)) parent.OnHitExit(other);
	}
}
