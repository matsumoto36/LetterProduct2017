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

	public int power { get { return (int)(_power * unitOwner.statusMod.mulPow); } protected set { _power = value; } }
	public float interval { get { return _interval / unitOwner.statusMod.mulAttackSpeed; } protected set { _interval = value; } }

	public StatusModifier weaponMod { get; set; }
	public Unit unitOwner { get; set; }

	public bool canAction { get; private set; }

	protected Transform body;

	void Start() {
		Init();
	}

	/// <summary>
	/// 初期設定
	/// </summary>
	public virtual void Init() {
		weaponMod = new StatusModifier(1);
		StartCoroutine(WaitInterval());
		Debug.Log("Init");
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
