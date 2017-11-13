using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離武器の親クラス
/// </summary>
public abstract class WeaponRanged : Weapon {

	const string SHOT_ANCHOR_NAME = "[ShotAnchor]";

	protected BulletData bData;

	protected Transform shotAnchor;

	public override void Init() {
		base.Init();

		shotAnchor = transform.Find(SHOT_ANCHOR_NAME);
	}

	public virtual void SetData(float interval, BulletData bData) {
		this.interval = interval;
		this.bData = bData;
	}
}
