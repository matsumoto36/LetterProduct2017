using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特定のキャラクターから受けたダメージ量を保持する
/// </summary>
public struct DamageLog {
	public Unit attackUnit;
	public int damage;

	public DamageLog(Unit attackUnit, int damage) {
		this.attackUnit = attackUnit;
		this.damage = damage;
	}
}
