﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CreateAssetMenu(menuName = "Unit/Create Enemy Data")]
public class EnemyData : UnitData {

	public EnemyAI.Mode enemyAIType;
	public float dashSpeed;
	public float moveLine;
	public float stepLine;
	public float attackLine;
	public float searchAngle;

	public override Unit Spawn(Vector3 position, Quaternion rot) {

		var enemy = SpawnUnit<Enemy>(position, rot);
		var ai = enemy.gameObject.AddComponent<EnemyAI>();
		ai.mode = enemyAIType;
		ai.speed = enemy.moveSpeed;
		ai.dashSpeed = dashSpeed;
		ai.moveLine = moveLine;
		ai.stepLine = stepLine;
		ai.attackLine = attackLine;
		ai.searchAngle = searchAngle;

		return enemy;
	}

	[MenuItem("Unit/Create Enemy Data")]
	static void CreateData() {
		CreateAsset<EnemyData>(ASSET_PATH);
	}
}
