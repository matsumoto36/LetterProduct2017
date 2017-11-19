using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// マップに存在するキャラクターの親クラス
/// (タレット等の構造物も含む)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour {

	const int CAN_EQUIPPED_WEAPON_COUNT = 2;
	const string HAND_ANCHOR = "[HandAnchor]";
	const string WEAPON_STATUS_MOD_KEY = "WEAPON_MOD";

	[SerializeField]
	int _maxHP;
	[SerializeField]
	int _nowHP;
	[SerializeField]
	float _moveSpeed;
	[SerializeField]
	float _rotSpeed;
	[SerializeField]
	StatusModifier _statusMod = new StatusModifier(1);

	public int maxHP { get { return (int)(_maxHP * statusMod.mulHP); } private set { _maxHP = value; } }
	public int nowHP { get { return _nowHP; } protected set { _nowHP = value; } }
	public float moveSpeed { get { return _moveSpeed * statusMod.mulMoveSpeed; } set { _moveSpeed = value; } }
	public float rotSpeed { get { return _rotSpeed * statusMod.mulMoveSpeed; } set { _rotSpeed = value; } }
	public StatusModifier statusMod { get { return _statusMod; } private set { _statusMod = value; } }

	public Weapon[] equipWeapon { get; private set; }
	public bool isLockRotation { get; private set; }
	public bool isAttack { get; protected set; }
	public bool isDead { get; private set; }

	public bool canAttack { get; private set; }

	protected Transform body;
	protected Vector3 moveVec;
	protected Transform handAnchor;
	protected Rigidbody unitRig;

	Animator anim;
	int switchingWeaponNum = 0;
	Dictionary<string, StatusModifier> statusModStack;

	// Use this for initialization
	public virtual void Awake() {

		equipWeapon = new Weapon[CAN_EQUIPPED_WEAPON_COUNT];
		statusModStack = new Dictionary<string, StatusModifier>();
		canAttack = true;

		anim = GetComponent<Animator>();
		unitRig = GetComponent<Rigidbody>();

		body = transform.GetChild(0);
		foreach(Transform child in body.transform) {

			handAnchor = child.Find(HAND_ANCHOR);
			if(handAnchor) break;
		}

		nowHP = maxHP;
	}

	/// <summary>
	/// パッシブ効果を適用
	/// </summary>
	/// <param name="mod"></param>
	/// <param name="key"></param>
	/// <returns>適用できたか</returns>
	public bool ApplyModifier(StatusModifier mod, string key) {

		if(statusModStack
			.Where((item) => item.Key == key)
			.Count() > 0)
			return false;

		statusModStack.Add(key, mod);
		//再計算
		CalcSumStatusModfier();
		return true;
	}

	/// <summary>
	/// パッシブ効果を取り外す
	/// </summary>
	/// <param name="key"></param>
	/// <returns>取り外せたか</returns>
	public bool RemoveModifier(string key) {

		if(statusModStack
			.Where((item) => item.Key == key)
			.Count() == 0)
			return false;

		statusModStack.Remove(key);
		//再計算
		CalcSumStatusModfier();
		return true;
	}

	/// <summary>
	/// パッシブ効果の合計を計算する
	/// </summary>
	public void CalcSumStatusModfier() {

		var tempHP = maxHP;

		statusMod = new StatusModifier(1) + statusModStack
			.Select((item) => item.Value)
			.Aggregate((item1, item2) => item1 + item2);

		//HP上昇値の影響は受けるが、maxHPを超えず、nowHPを減らさないようにする
		nowHP = Mathf.Min(maxHP, Mathf.Max(nowHP, nowHP + maxHP - tempHP));
	}

	/// <summary>
	/// 指定の向き・回転で移動 
	/// </summary>
	public abstract void Move();

	/// <summary>
	/// 死亡時に呼ばれる
	/// </summary>
	public virtual void Death() {
		nowHP = 0;
	}

	/// <summary>
	/// 武器を装備する
	/// </summary>
	/// <param name="weapon"></param>
	/// <param name="slot"></param>
	public void EquipWeapon(Weapon weapon, int slot) {

		//すでに装備してたら外す
		if(equipWeapon[slot]) {
			RemoveWeapon(slot);
		}

		//位置合わせ
		weapon.transform.parent = handAnchor;
		weapon.transform.localPosition = new Vector3();
		weapon.transform.localRotation = Quaternion.identity;

		//所持する
		equipWeapon[slot] = weapon;
		equipWeapon[slot].unitOwner = this;

		//パッシブ効果を登録
		ApplyModifier(equipWeapon[slot].weaponMod, WEAPON_STATUS_MOD_KEY + slot);

		//0番以外の武器は無効化
		if(slot != 0) equipWeapon[slot].gameObject.SetActive(false);
	}

	/// <summary>
	/// 武器を外す
	/// </summary>
	/// <param name="slot"></param>
	public void RemoveWeapon(int slot) {

		if(equipWeapon[slot]) {
			Destroy(equipWeapon[slot].gameObject);
		}
		
		//パッシブ効果を削除
		RemoveModifier(WEAPON_STATUS_MOD_KEY + slot);

		equipWeapon[slot] = null;
	}

	/// <summary>
	/// 所持している武器を手持ち( = 0番)と入れ替える
	/// </summary>
	/// <param name="animTriggerName"></param>
	/// <param name="weaponNum"></param>
	/// <param name="onComplete">完了時に実行</param>
	public void SwitchWeapon(string animTriggerName, int weaponNum, Action onComplete) {

		if(weaponNum == 0) return;
		if(!equipWeapon[0]) return;
		if(!equipWeapon[weaponNum]) return;
		if(isLockRotation) return;

		//一定時間待つ
		StartCoroutine(SwitchWeaponWait(animTriggerName, weaponNum, onComplete));

	}

	/// <summary>
	/// 近接攻撃用のアニメーションを再生
	/// </summary>
	/// <param name="triggerName"></param>
	/// <param name="speed"></param>
	/// <param name="onComplete">完了時に実行</param>
	public void PlayMeleeAnimation(string triggerName, float speed, Action onComplete) {
		StartCoroutine(PlayMeleeAnimWait(triggerName, speed, onComplete));
	}

	/// <summary>
	/// 攻撃する
	/// </summary>
	/// <param name="from">攻撃するキャラクター</param>
	/// <param name="to">攻撃を受けるキャラクター</param>
	/// <param name="damage"></param>
	/// <returns>成功したかどうか</returns>
	public static bool Attack(Unit from, Unit to, int damage) {
		if(!from || !to) return false;
		Debug.Log("Attack " + from.name + " -> " + to.name);
		to.ApplyDamage(damage);
		return true;
	}

	/// <summary>
	/// ダメージを与える
	/// </summary>
	/// <param name="from">攻撃するキャラクター</param>
	/// <param name="damage"></param>
	protected virtual void ApplyDamage(int damage) {
		nowHP -= damage;
		if(nowHP <= 0) Death();
	}

	/// <summary>
	/// 武器が入れ替わる瞬間
	/// </summary>
	void OnSwitchWeaponModel() {

		//0番同士の交換はありえないので実行しない
		if(switchingWeaponNum == 0) return;

		//モデルの表示・非表示を切り替えるのみ
		equipWeapon[0].gameObject.SetActive(false);
		equipWeapon[switchingWeaponNum].gameObject.SetActive(true);
	}

	/// <summary>
	/// 近接攻撃の判定が始まったとき
	/// </summary>
	void OnMeleeAttackStart() {

		if(!equipWeapon[0]) return;

		var weaponMelee = equipWeapon[0].GetComponent<WeaponMelee>();
		if(!weaponMelee) return;
		weaponMelee.SetCollider(true);
	}

	/// <summary>
	/// 近接攻撃の判定が終わったとき
	/// </summary>
	void OnMeleeAttackEnd() {

		if(!equipWeapon[0]) return;

		var weaponMelee = equipWeapon[0].GetComponent<WeaponMelee>();
		if(!weaponMelee) return;
		weaponMelee.SetCollider(false);
	}

	IEnumerator PlayMeleeAnimWait(string triggerName, float speed, Action onComplete) {

		anim.SetTrigger(triggerName);
		anim.speed = speed;
		anim.Update(0);

		isLockRotation = true;
		//現状各アニメーション(Idleから)の遷移時間を0にしているので動いている。
		//0でない場合は遷移前の長さが出てくるはず
		Debug.Log(anim.GetCurrentAnimatorStateInfo(0).length);
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		isLockRotation = false;
		anim.speed = 1;

		//完了時に実行
		onComplete();
	}

	IEnumerator SwitchWeaponWait(string animTriggerName, int weaponNum, Action onComplete) {

		//攻撃キャンセル
		isAttack = false;

		anim.SetTrigger(animTriggerName);
		anim.Update(0);
		switchingWeaponNum = weaponNum;

		//交換中は攻撃できません
		canAttack = false;
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		canAttack = true;

		//内部的に交換
		var w = equipWeapon[0];
		equipWeapon[0] = equipWeapon[weaponNum];
		equipWeapon[weaponNum] = w;

		//完了後に実行
		onComplete();
	}
}
