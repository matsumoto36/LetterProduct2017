using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 実弾が出る武器のクラス
/// </summary>
public class WeaponGun : WeaponRanged {

	protected float bulletSpeed;

	public void SetData(int power, float interval, Bullet bullet, float bulletSpeed) {
		this.power = power;
		this.interval = interval;
		this.bullet = bullet;
		this.bulletSpeed = bulletSpeed;
	}

	public override void Attack() {

		if(!canFire) return;

		//弾を発射
		var b = Instantiate(bullet, shotAnchor.position, shotAnchor.rotation);
		b.power = power;
		b.speed = bulletSpeed;

		//intervalの設定(最後に呼ぶこと)
		base.Attack();
	}
}
