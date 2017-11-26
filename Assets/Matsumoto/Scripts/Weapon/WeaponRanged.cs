using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離武器の親クラス
/// </summary>
public abstract class WeaponRanged : Weapon {

	const string SHOT_ANCHOR = "[ShotAnchor]";

	protected BulletData bData;
	protected Transform shotAnchor;

	public override void Init() {
		base.Init();

		shotAnchor = transform.Find(SHOT_ANCHOR);
	}

	/// <summary>
	/// 必要なデータをセットする
	/// </summary>
	/// <param name="interval"></param>
	/// <param name="bData"></param>
	public virtual void SetData(float interval, int power, BulletData bData) {
		baseInterval = interval;
		basePower = power;
		bData.SetOwner(this);
		this.bData = bData;

	}
}
