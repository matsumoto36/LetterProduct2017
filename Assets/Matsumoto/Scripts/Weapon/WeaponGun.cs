using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 実弾が出る武器のクラス
/// </summary>
public class WeaponGun : WeaponRanged {

	float bulletSpeed;

	/// <summary>
	/// 必要なデータをセットする
	/// </summary>
	/// <param name="power"></param>
	/// <param name="interval"></param>
	/// <param name="bullet"></param>
	/// <param name="bulletSpeed"></param>
	public void SetData(int power, float interval, BulletNormal bullet, float bulletSpeed) {
		this.power = power;
		this.interval = interval;
		this.bullet = bullet;
		this.bulletSpeed = bulletSpeed;
	}

	/// <summary>
	/// 攻撃するときに呼ばれる
	/// </summary>
	public override void Attack() {

		//親クラスで攻撃ができるかの判断を行っている
		if(!canFire) return;

		//弾を発射
		var b = Instantiate(bullet, shotAnchor.position, shotAnchor.rotation);
		b.power = power;
		b.speed = bulletSpeed;

		//intervalの設定(最後に呼ぶこと)
		base.Attack();
	}
}
