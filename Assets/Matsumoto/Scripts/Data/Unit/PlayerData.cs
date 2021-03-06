﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Unit/Create Player Data")]
public class PlayerData : UnitData {

	public Unit Spawn(Vector3 position, Quaternion rot, WeaponData[] weaponData) {
		//武器情報をオーバーライドする
		this.weaponData = weaponData;
		return SpawnUnit<Player>(position, rot);
	}

	public override Unit Spawn(Vector3 position, Quaternion rot) {
		return SpawnUnit<Player>(position, rot);
	}
}
