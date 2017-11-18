using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// マップに存在するキャラクターの親クラス
/// (タレット等の構造物も含む)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour {

	const int CAN_EQUIPPED_WEAPON_COUNT = 2;
	const string HAND_ANCHOR = "[HandAnchor]";

	[SerializeField]
	private int _maxHP;
	[SerializeField]
	private int _nowHP;
	[SerializeField]
	private float _moveSpeed;
	[SerializeField]
	private float _rotSpeed;

	public Weapon[] equipWeapon { get; private set; }

	public int maxHP { get { return _maxHP; } private set { _maxHP = value; } }
	public int nowHP { get { return _nowHP; } private set { _nowHP = value; } }
	public float moveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
	public float rotSpeed { get { return _rotSpeed; } set { _rotSpeed = value; } }

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

	// Use this for initialization
	public virtual void Awake() {

		equipWeapon = new Weapon[CAN_EQUIPPED_WEAPON_COUNT];
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
	/// 指定の向き・回転で移動 
	/// </summary>
	public abstract void Move();



	/// <summary>
	/// ダメージを与える
	/// </summary>
	/// <param name="damage"></param>
	public virtual void ApplyDamage(int damage) {

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

		//0番以外の武器は無効化
		if(slot != 0) equipWeapon[slot].gameObject.SetActive(false);
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
	public void PlayMeleeAnimation(string triggerName, Action onComplete) {
		StartCoroutine(PlayMeleeAnimWait(triggerName, onComplete));
	}

	/// <summary>
	/// 武器を外す
	/// </summary>
	/// <param name="slot"></param>
	public void RemoveWeapon(int slot) {

		if(equipWeapon[slot]) {
			Destroy(equipWeapon[slot].gameObject);
		}

		equipWeapon[slot] = null;
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

	IEnumerator PlayMeleeAnimWait(string triggerName, Action onComplete) {

		anim.SetTrigger(triggerName);
		anim.Update(0);

		isLockRotation = true;
		//現状各アニメーション(Idleから)の遷移時間を0にしているので動いている。
		//0でない場合は遷移前の長さが出てくるはず
		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		isLockRotation = false;

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
