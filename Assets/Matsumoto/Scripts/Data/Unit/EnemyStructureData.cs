using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Unit/Create EnemyStructure Data")]
public class EnemyStructureData : EnemyData {

	public float detectAngle;		//視野角(度)
	public float detectRadius;		//視認できる範囲(半径)
	public bool isDetectAttacked;	//範囲内で攻撃されたときに振り向くか

	public override Unit Spawn(Vector3 position, Quaternion rot) {

		var enemy = SpawnUnit<EnemyStructure>(position, rot);

		enemy.attackDuration = attackDuration;
		enemy.attackRestDuration = attackRestDuration;

		enemy.detectAngle = detectAngle;
		enemy.detectRadius = detectRadius;
		enemy.isDetectAttacked = isDetectAttacked;

		return enemy;
	}
}
