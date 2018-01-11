using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet/Create GrenadeBullet Data")]
public class BulletGrenadeData : BulletNormalData {

	public float expRadius;
	public int expPow;

	public override Bullet Create(Weapon owner, Vector3 position, Quaternion quaternion) {
		return CreateBullet<BulletGrenade>(this, owner, position, quaternion);
	}
}
