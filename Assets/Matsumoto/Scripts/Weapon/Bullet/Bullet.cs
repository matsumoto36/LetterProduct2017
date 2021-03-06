﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 武器から出る弾の親クラス
/// </summary>
public abstract class Bullet : MonoBehaviour {

	public Weapon bulletOwner { get; set; }
	public PKFxFX attackParticle { get; set; }
	object bulletData { get; set; }

	/// <summary>
	/// 初期化
	/// </summary>
	public virtual void Init() {

		//相手の勢力に当たるように設定
		var maskList = UnitGroupMatrix.GetAttackableGroup(bulletOwner.unitOwner.group, true)
			.Select((item) => item.ToString() + "Layer")
			.ToList();

		maskList.Add("BulletLayer");
		maskList.Add("IgnoreHit");

		bulletOwner.SetHitMask(maskList.ToArray());

		//ポーズ用処理追加
		gameObject.AddComponent<Pause>();
	}

	/// <summary>
	/// 弾のデータをセットする
	/// </summary>
	/// <param name="data"></param>
	public void SetBulletData(BulletData data) {
		bulletData = data;
	}

	public void Death() {

		//再生中のパーティクルを止める
		ParticleManager.StopParticle(attackParticle);

		//削除
		BulletData.DestroyBullet(this);
	}

	/// <summary>
	/// 弾のデータを取得する
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T GetBulletData<T>() where T : BulletData {
		return (T)bulletData;
	}

	/// <summary>
	/// 攻撃する
	/// </summary>
	/// <param name="target"></param>
	protected virtual void Attack(Unit target) {
		if(!target) return;
		Unit.Attack(bulletOwner.unitOwner, target, bulletOwner.power);
	}

	/// <summary>
	/// 回復する
	/// </summary>
	/// <param name="target"></param>
	protected virtual void Heal(Unit target) {
		Unit.Heal(bulletOwner.unitOwner, target, bulletOwner.power);
	}

	/// <summary>
	/// 弾が当たった瞬間
	/// </summary>
	/// <param name="other"></param>
	public virtual void OnHitEnter(Collider other) { }
	/// <summary>
	/// 弾が当たった後の瞬間
	/// </summary>
	/// <param name="other"></param>
	public virtual void OnHitExit(Collider other) { }

	public virtual void Update() {
		if(attackParticle) attackParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
	}

	void OnTriggerEnter(Collider other) {
		//ヒットしない定義のものだったらスキップ
		if(bulletOwner.CheckHit(other.gameObject)) OnHitEnter(other);
	}
	void OnTriggerExit(Collider other) {
		if(bulletOwner.CheckHit(other.gameObject)) OnHitExit(other);
	}
}
