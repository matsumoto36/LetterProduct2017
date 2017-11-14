using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり判定メソッドを親クラスに移譲するクラス
/// </summary>
public class BulletCollision : MonoBehaviour {

	public Bullet parent { get; set; }

	bool CheckHit(GameObject obj) {

		//自分の場合はヒットしない
		if(ReferenceEquals(obj, parent.bData.owner.owner.gameObject)) return false;

		//番号で渡されるため、マスクできるようにビット列にする
		int layer = obj.layer == 0 ? 0 : (int)Mathf.Pow(2, obj.layer);
		//レイヤーマスクを考慮してヒットしたかどうか
		return layer == 0 || (layer & parent.hitMask) != 0;

	}

	void OnTriggerEnter(Collider other) {
		//ヒットしない定義のものだったらスキップ
		if(CheckHit(other.gameObject)) parent.OnHitEnter(other);
	}
	void OnTriggerStay(Collider other) {
		if(CheckHit(other.gameObject)) parent.OnHitting(other);
	}
	void OnTriggerExit(Collider other) {
		if(CheckHit(other.gameObject)) parent.OnHitExit(other);
	}
}
