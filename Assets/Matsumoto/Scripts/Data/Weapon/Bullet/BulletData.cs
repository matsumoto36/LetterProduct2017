using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の情報全部入りクラス
/// </summary>
public abstract class BulletData : ScriptableObjectBase {

	public const string ASSET_PATH = "Resources/Data/Bullet";

	public GameObject model;
	public string particleName;

	public abstract Bullet Create(Weapon owner, Vector3 position, Quaternion quaternio);

	protected static T CreateBullet<T>(BulletData data, Weapon owner, Vector3 position, Quaternion quaternion)
		where T : Bullet {

		var bulletObj = Instantiate(data.model, position, quaternion).AddComponent<T>();
		bulletObj.SetBulletData(data);
		bulletObj.tag = "Bullet";
		bulletObj.bulletOwner = owner;

		//エフェクトの生成
		if(data.particleName != "") {
			var bulletParticle = ParticleManager.Spawn(data.particleName, bulletObj.transform.position, bulletObj.transform.rotation, 0);
			bulletParticle.transform.SetParent(bulletObj.transform);
		}

		bulletObj.Init();

		return bulletObj;
	}
}
