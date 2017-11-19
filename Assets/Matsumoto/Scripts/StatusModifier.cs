using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターのステータスに変更を加えるクラス
/// </summary>
[Serializable]
public class StatusModifier{

	public float mulHP;
	public float mulMoveSpeed;
	public float mulPow;
	public float mulAttackSpeed;

	public StatusModifier() {
		mulHP = 1;
		mulMoveSpeed = 1;
		mulPow = 1;
		mulAttackSpeed = 1;
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

	public new string ToString() {
		return string.Format("HP:{0:p} MoveSpeed:{1:p} Power:{2:p} ATKSpeed{3:p}", mulHP, mulMoveSpeed, mulPow, mulAttackSpeed);
	}
}
