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

		//エフェクト再生
		ParticleManager.Spawn("MuzzleFlash", shotAnchor.position, shotAnchor.rotation);
		//弾を発射
		bulletData.Create(this, shotAnchor.position, shotAnchor.rotation);
		//SE再生
		var se = AudioManager.PlaySE(useSound);
		se.transform.position = transform.position;

		//intervalの設定(最後に呼ぶこと)
		base.Attack();
	}
}
