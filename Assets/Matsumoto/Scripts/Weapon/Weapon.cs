using System.Collections;
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
	public int power { get { return _power;} private set { _power = value; } }

	[SerializeField]
	float _interval;
	public float interval { get { return _interval; } private set { _interval = value; } }

	[SerializeField]
	StatusModifier _weaponMod = new StatusModifier();
	public StatusModifier weaponMod { get { return _weaponMod; } set { _weaponMod = value; } }

	public Sprite icon { get; set; }
	public Unit unitOwner { get; set; }
	public string equipSound { get; set; }
	public string useSound { get; set; }
	public string hitSound { get; set; }
	public WeaponType weaponType { get; protected set; }
	public bool canAction { get; private set; }
	public int hitMask { get; private set; }

	protected Transform body;
	protected int basePower;
	protected float baseInterval;

	/// <summary>
	/// 初期設定
	/// </summary>
	public virtual void Init() {

		weaponType = WeaponType.Other;
		canAction = true;
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
		UtilityMethod.DelayExecution(() => canAction = true, interval);
		//StartCoroutine(WaitInterval());
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
		if(unitOwner && obj == unitOwner.gameObject) return false;

		//番号で渡されるため、マスクできるようにビット列にする
		int layer = obj.layer == 0 ? 0 : (int)Mathf.Pow(2, obj.layer);
		//レイヤーマスクを考慮してヒットしたかどうか
		return layer == 0 || (layer & hitMask) != 0;

	}

	public void OnSwitchActive() {
		canAction = false;
		UtilityMethod.DelayExecution(() => canAction = true, interval);
		//StartCoroutine(WaitInterval());
	}

	///// <summary>
	///// 次の発射まで待つ
	///// </summary>
	///// <returns></returns>
	//IEnumerator WaitInterval() {
	//	yield return new WaitForSeconds(interval);
	//	canAction = true;
	//}
}
