using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Bullet/Create NormalBullet Data")]
public class BulletNormalData : BulletData {

	public float speed;

	public override Bullet Create(Weapon owner, Vector3 position, Quaternion quaternion) {
		return CreateBullet<BulletNormal>(this, owner, position, quaternion);
	}

	[MenuItem("Bullet/Create NormalBullet Data")]
	static void CreateBulletData() {
		CreateAsset<BulletNormalData>(ASSET_PATH);
	}


}
