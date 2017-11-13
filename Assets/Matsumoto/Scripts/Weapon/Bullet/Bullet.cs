using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器から出る弾の親クラス
/// </summary>
public abstract class Bullet : MonoBehaviour {

	const string BODY_ANCHOR = "Body";
	public BulletData bData { get; private set; }

	protected Transform body;

	public static Bullet CreateBullet(BulletData bData, Transform transform) {

		//親の生成
		var bulletObj = Instantiate(bData.bullet, transform.position, transform.rotation);
		bulletObj.bData = bData;

		//本体の生成
		bulletObj.body = Instantiate(bData.model, transform).transform;
		bulletObj.body.tag = "Bullet";
		bulletObj.body.parent = bulletObj.transform;
		bulletObj.body.localPosition = new Vector3();
		bulletObj.body.transform.localRotation = Quaternion.identity;
		bulletObj.body.GetComponent<BulletCollision>().parent = bulletObj;

		return bulletObj;
	}

	public abstract void OnTriggerEnter(Collider other);
}
