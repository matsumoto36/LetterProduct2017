using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マップに存在するキャラクターの親クラス
/// (タレット等の構造物も含む)
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Unit : MonoBehaviour {

	const string HAND_ANCHOR = "[HandAnchor]";

	public float moveSpeed { get; set; }
	public float rotSpeed { get; set; }
	public Weapon[] equipWeapon { get; private set; }

	protected Transform body;
	protected Vector3 moveVec;
	protected Transform handAnchor;
	protected Rigidbody unitRig;

	// Use this for initialization
	public virtual void Awake() {

		equipWeapon = new Weapon[2];

		body = transform.GetChild(0);
		handAnchor = body.Find(HAND_ANCHOR);
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
		weapon.transform.parent = handAnchor;
		weapon.transform.localPosition = new Vector3();
		weapon.transform.localRotation = Quaternion.identity;

		//所持する
		equipWeapon[slot] = weapon;
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
}
