using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器全体の親クラス
/// </summary>
public abstract class Weapon : MonoBehaviour {

	public Unit owner { get; set; }
	public bool canFire { get; private set; }

	protected Transform body;

	protected int power;
	protected float interval;

	void Start() {
		Init();
	}

	/// <summary>
	/// 初期設定
	/// </summary>
	public virtual void Init() {
		StartCoroutine(WaitInterval());
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

		canFire = false;
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
		yield return new WaitForSeconds(interval);
		canFire = true;
	}
}
