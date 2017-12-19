using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 実弾が出る武器のクラス
/// </summary>
public class WeaponGun : WeaponRanged {

	/// <summary>
	/// 攻撃するときに呼ばれる
	/// </summary>
	public override void Attack() {

		//親クラスで攻撃ができるかの判断を行っている
		if(!canAction) return;

		//弾を発射
		bulletData.Create(this, shotAnchor.position, shotAnchor.rotation);

		//intervalの設定(最後に呼ぶこと)
		base.Attack();
	}
}
