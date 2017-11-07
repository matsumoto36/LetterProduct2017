using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器から出る弾の親クラス
/// </summary>
public abstract class Bullet : MonoBehaviour {

	protected WeaponGun owner;
	public int power { get; set; }
	public float speed { get; set; }

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
}
