using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// マップに存在するキャラクターの親クラス
/// (タレット等の構造物も含む)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour {

	const string HAND_ANCHOR_L = "[HandAnchorL]";
	const string HAND_ANCHOR_R = "[HandAnchorR]";

	public float moveSpeed { get; set; }
	public float rotSpeed { get; set; }
	public Weapon[] equipWeapon { get; private set; }
	public bool isPlayAnim { get; private set; }

	protected Transform body;
	protected Vector3 moveVec;
	protected Transform handAnchorL;
	protected Transform handAnchorR;
	protected Rigidbody unitRig;

	Animator anim;

	// Use this for initialization
	public virtual void Awake() {


		equipWeapon = new Weapon[2];

		body = transform.GetChild(0);

		anim = GetComponent<Animator>();

		foreach(Transform child in body.transform) {

			handAnchorL = child.Find(HAND_ANCHOR_L);
			handAnchorR = child.Find(HAND_ANCHOR_R);

			if(handAnchorL && handAnchorR) break;
		}

		unitRig = GetComponent<Rigidbody>();
	}

	/// <summary>
	/// 指定の向き・回転で移動 
	/// </summary>
	public abstract void Move();

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
		weapon.transform.parent = slot == 0 ? handAnchorL : handAnchorR;
		weapon.transform.localPosition = new Vector3();
		weapon.transform.localRotation = Quaternion.identity;

		//所持する
		equipWeapon[slot] = weapon;
		equipWeapon[slot].unitOwner = this;
	}

	public void PlayAnimation(string triggerName) {
		StartCoroutine(PlayAnimWait(triggerName));
	}

	IEnumerator PlayAnimWait(string triggerName) {
		anim.SetTrigger(triggerName);
		anim.Update(0);
		isPlayAnim = true;

		yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		isPlayAnim = false;
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

}
