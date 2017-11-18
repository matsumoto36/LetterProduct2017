using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾の情報全部入りクラス
/// </summary>
public class BulletData {

	public Bullet bullet { get; private set; }
	public Weapon bulletOwner { get; private set; }
	public GameObject model { get; private set; }
	public int power { get; private set; }
	public float speed { get; private set; }
	public float expRadius { get; private set; }
	public int expPow { get; private set; }
	public float maxLength { get; private set; }

	public BulletData(Bullet bullet, Weapon bulletOwner, GameObject model) {
		this.bullet = bullet;
		this.bulletOwner = bulletOwner;
		this.model = model;
	}

	public void SetBulletDataNormal(int power, float speed) {
		this.power = power;
		this.speed = speed;
	}

	public void SetBulletDataLaser(int power, int maxLength) {
		this.power = power;
		this.maxLength = maxLength;
	}

	public void SetBulletDataGrenade(int power, float speed, int expPow, float expRadius) {
		this.power = power;
		this.speed = speed;
		this.expPow = expPow;
		this.expRadius = expRadius;
	}
}
