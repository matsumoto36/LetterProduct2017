using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public bool canAction { get; private set; }

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

		StartCoroutine(WaitInterval());
		Debug.Log("WeaponBaseInit");
	}

	public virtual void UpdateStatus() {
		Debug.Log("UpdateStatus");
		power = (int)(basePower * unitOwner.statusMod.mulPow);
		interval = baseInterval / unitOwner.statusMod.mulAttackSpeed;
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
	/// 次の発射まで待つ
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitInterval() {
		yield return new WaitForSeconds(interval / unitOwner.statusMod.mulAttackSpeed);
		canAction = true;
	}
}
