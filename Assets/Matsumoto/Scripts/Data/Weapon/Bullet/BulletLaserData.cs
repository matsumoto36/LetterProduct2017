using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet/Create LaserBullet Data")]
public class BulletLaserData : BulletData {

	public float maxLength;
	public bool isHeal;

	public override Bullet Create(Weapon owner, Vector3 position, Quaternion quaternion) {

		if(isHeal) return CreateBullet<BulletLaserHeal>(this, owner, position, quaternion);

		return CreateBullet<BulletLaser>(this, owner, position, quaternion);
	}
}
