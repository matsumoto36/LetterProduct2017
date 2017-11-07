using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離武器の親クラス
/// </summary>
public abstract class WeaponRanged : Weapon {

	const string SHOT_ANCHOR_NAME = "[ShotAnchor]";

	//!!DEBUG!!
	public BulletNormal _bullet;

	protected Bullet bullet;
	protected Transform shotAnchor;

	public override void Init() {
		base.Init();

		shotAnchor = transform.Find(SHOT_ANCHOR_NAME);
	}
}
