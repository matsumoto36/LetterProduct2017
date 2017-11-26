using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の情報全部入りクラス
/// </summary>
public class BulletData {

	public string name { get; private set; }
	public Bullet bullet { get; private set; }
	public Weapon bulletOwner { get; private set; }
	public GameObject model { get; private set; }
	public float speed { get; private set; }
	public float expRadius { get; private set; }
	public int expPow { get; private set; }
	public float maxLength { get; private set; }

	public BulletData(string name, Bullet bullet, GameObject model, float speed, int expPow, float expRadius, float maxLength) {
		this.name = name;
		this.bullet = bullet;
		this.model = model;
		this.speed = speed;
		this.expPow = expPow;
		this.expRadius = expRadius;
		this.maxLength = maxLength;
	}

	public void SetOwner(Weapon bulletOwner) {
		this.bulletOwner = bulletOwner;
	}
}
