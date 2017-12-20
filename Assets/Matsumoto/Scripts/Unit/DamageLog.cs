using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特定のキャラクターから受けたダメージ量、時間を保持する
/// </summary>
public struct DamageLog {
	public Unit attackUnit;
	public int damage;
	public float time;

	public DamageLog(Unit attackUnit, int damage, float time) {
		this.attackUnit = attackUnit;
		this.damage = damage;
		this.time = time;
	}

	public new string ToString() {
		return string.Format("{0} : {1}", attackUnit.name, damage);
	}
}
