using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器全体の親クラス
/// </summary>
public abstract class Weapon : MonoBehaviour {

	public Unit owner { get; protected set; }
	public bool canFire { get; private set; }

	protected Transform body;

	protected int power;
	protected float interval;

	void Start() {
		Init();
	}

	public virtual void Init() {
		StartCoroutine(WaitInterval());
	}

	public virtual void Attack() {

		canFire = false;
		StartCoroutine(WaitInterval());

	}

	IEnumerator WaitInterval() {
		yield return new WaitForSeconds(interval);
		canFire = true;
	}
}
