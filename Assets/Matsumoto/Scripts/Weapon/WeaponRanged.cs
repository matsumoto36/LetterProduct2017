using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離武器の親クラス
/// </summary>
public abstract class WeaponRanged : Weapon {

	const string SHOT_ANCHOR = "[ShotAnchor]";

	protected BulletData bulletData;
	protected Transform shotAnchor;

	public override void Init() {
		base.Init();

		weaponType = WeaponType.Ranged;
		shotAnchor = transform.Find(SHOT_ANCHOR);
	}

	/// <summary>
	/// 必要なデータをセットする
	/// </summary>
	/// <param name="interval"></param>
	/// <param name="bulletData"></param>
	public virtual void SetData(float interval, int power, BulletData bulletData) {
		baseInterval = interval;
		basePower = power;
		this.bulletData = bulletData;
	}
}
