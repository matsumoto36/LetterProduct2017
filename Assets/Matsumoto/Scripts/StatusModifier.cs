using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステータスに変更を加えるクラス
/// </summary>
[Serializable]
public struct StatusModifier {

	public float mulHP;
	public float mulMoveSpeed;
	public float mulPow;
	public float mulAttackSpeed;

	/// <summary>
	/// 全てのパラメータをparで初期化
	/// </summary>
	/// <param name="par"></param>
	public StatusModifier(float par) {
		mulHP = par;
		mulMoveSpeed = par;
		mulPow = par;
		mulAttackSpeed = par;
	}

	public StatusModifier(float mulHP, float mulMoveSpeed, float mulPow, float mulAttackSpeed) {
		this.mulHP = mulHP;
		this.mulMoveSpeed = mulMoveSpeed;
		this.mulPow = mulPow;
		this.mulAttackSpeed = mulAttackSpeed;
	}

	public static StatusModifier operator +(StatusModifier par1, StatusModifier par2) {
		return new StatusModifier(
			par1.mulHP + par2.mulHP - 1,
			par1.mulMoveSpeed + par2.mulMoveSpeed - 1,
			par1.mulPow + par2.mulPow - 1,
			par1.mulAttackSpeed + par2.mulAttackSpeed - 1);
	}
}
