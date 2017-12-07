using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet/Create LaserBullet Data")]
public class BulletLaserData : BulletData {

	public float maxLength;
	public bool isHeal;

	public override Bullet Create(Weapon owner, Vector3 position, Quaternion quaternion) {
		return CreateBullet<BulletLaser>(this, owner, position, quaternion);
	}

	[MenuItem("Bullet/Create LaserBullet Data")]
	static void CreateBulletData() {
		CreateAsset<BulletLaserData>(ASSET_PATH);
	}
}
