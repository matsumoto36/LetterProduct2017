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
	public string particleNameAttack;
	public string particleNameHit;

	public abstract Bullet Create(Weapon owner, Vector3 position, Quaternion quaternio);

	protected static T CreateBullet<T>(BulletData data, Weapon owner, Vector3 position, Quaternion quaternion)
		where T : Bullet {

		var bulletObj = BulletObjectPool.GetInstance(data.model.name).AddComponent<T>();
		bulletObj.name = data.name;
		bulletObj.transform.SetPositionAndRotation(position, quaternion);
		bulletObj.SetBulletData(data);
		bulletObj.tag = "Bullet";
		bulletObj.bulletOwner = owner;

		//エフェクトの生成
		if(data.particleNameAttack != "") {
			bulletObj.attackParticle = ParticleManager.Spawn(data.particleNameAttack, bulletObj.transform.position, bulletObj.transform.rotation, 0);
		}

		bulletObj.Init();

		return bulletObj;
	}

	public static void DestroyBullet<T>(T bullet) where T : Bullet {

		var g = bullet.gameObject;
		Destroy(bullet);

		ObjectPooler.ReleaseInstance(g);
	}
}
