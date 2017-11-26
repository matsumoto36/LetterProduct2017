using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 武器から出る弾の親クラス
/// </summary>
public abstract class Bullet : MonoBehaviour {

	const string BODY_ANCHOR = "Body";

	public BulletData bData { get; private set; }

	protected Transform body;

	void Start() {
		Init();
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public virtual void Init() {

		//デフォルトで突き抜けるレイヤー
		bData.bulletOwner.SetHitMask("PlayerLayer", "BulletLayer");
	}

	/// <summary>
	/// 攻撃する
	/// </summary>
	/// <param name="target"></param>
	protected virtual void Attack(Unit target) {
		if(!target) return;
		Unit.Attack(bData.bulletOwner.unitOwner, target, bData.bulletOwner.power);
	}

	/// <summary>
	/// 回復する
	/// </summary>
	/// <param name="target"></param>
	protected virtual void Heal(Unit target) {
		Unit.Heal(bData.bulletOwner.unitOwner, target, bData.bulletOwner.power);
	}

	/// <summary>
	/// 弾が当たった瞬間
	/// </summary>
	/// <param name="other"></param>
	public virtual void OnHitEnter(Collider other) { }
	/// <summary>
	/// 弾が当たっているとき
	/// </summary>
	/// <param name="other"></param>
	public virtual void OnHitting(Collider other) { }
	/// <summary>
	/// 弾が当たった後の瞬間
	/// </summary>
	/// <param name="other"></param>
	public virtual void OnHitExit(Collider other) { }

	/// <summary>
	/// 弾を生成する
	/// </summary>
	/// <param name="bData"></param>
	/// <param name="transform"></param>
	/// <returns></returns>
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

}
