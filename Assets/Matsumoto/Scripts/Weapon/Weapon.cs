﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 武器のタイプ
/// </summary>
public enum WeaponType {
	Ranged,
	Melee,
	Other,
}

/// <summary>
/// 武器全体の親クラス
/// </summary>
public abstract class Weapon : MonoBehaviour {

	[SerializeField]
	int _power;
	[SerializeField]
	float _interval;
	[SerializeField]
	StatusModifier _weaponMod = new StatusModifier();

	public int power { get { return _power;} private set { _power = value; } }
	public float interval { get { return _interval; } private set { _interval = value; } }
	public StatusModifier weaponMod { get { return _weaponMod; } set { _weaponMod = value; } }

	public Unit unitOwner { get; set; }
	public WeaponType weaponType { get; protected set; }
	public bool canAction { get; private set; }
	public int hitMask { get; private set; }

	protected Transform body;
	protected int basePower;
	protected float baseInterval;

	void Start() {
		Init();
	}

	/// <summary>
	/// 初期設定
	/// </summary>
	public virtual void Init() {

		weaponType = WeaponType.Other;
		StartCoroutine(WaitInterval());
		Debug.Log("WeaponBaseInitEnd");
	}

	/// <summary>
	/// ステータスを更新する
	/// </summary>
	public virtual void UpdateStatus() {
		power = (int)(basePower * unitOwner.statusMod.mulPow);
		interval = baseInterval / unitOwner.statusMod.mulAttackSpeed;
	}

	/// <summary>
	/// 当たらないマスクをセットする
	/// </summary>
	/// <param name="maskName"></param>
	public void SetHitMask(params string[] maskName) {

		hitMask = ~maskName
			.Select((item) => LayerMask.GetMask(item))
			.Sum();
	}

	/// <summary>
	/// 攻撃が始まった瞬間
	/// </summary>
	public virtual void AttackStart() {

	}

	/// <summary>
	/// 攻撃中
	/// </summary>
	public virtual void Attack() {

		canAction = false;
		StartCoroutine(WaitInterval());
	}

	/// <summary>
	/// 攻撃が終了した瞬間
	/// </summary>
	public virtual void AttackEnd() {

	}

	/// <summary>
	/// そのGameObjectに攻撃がヒットしたか
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public bool CheckHit(GameObject obj) {

		//自分の場合はヒットしない
		if(obj == unitOwner.gameObject) return false;

		//番号で渡されるため、マスクできるようにビット列にする
		int layer = obj.layer == 0 ? 0 : (int)Mathf.Pow(2, obj.layer);
		//レイヤーマスクを考慮してヒットしたかどうか
		return layer == 0 || (layer & hitMask) != 0;

	}

	/// <summary>
	/// 次の発射まで待つ
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitInterval() {
		yield return new WaitForSeconds(interval);
		canAction = true;
	}
}
