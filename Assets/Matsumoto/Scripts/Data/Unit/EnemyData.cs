using UnityEngine;
using System.Collections;

/// <summary>
/// 敵のデータ
/// </summary>
[CreateAssetMenu(menuName = "Unit/Create Enemy Data")]
public class EnemyData : UnitData {

	//攻撃間隔
	public float attackDuration;
	public float attackRestDuration;

	//敵AIの設定
	public EnemyAI.Mode enemyAIType;
	public float dashSpeed;
	public float moveLine;
	public float stepLine;
	public float attackLine;
	public float searchAngle;

	public override Unit Spawn(Vector3 position, Quaternion rot) {

		var enemy = SpawnUnit<Enemy>(position, rot);

		enemy.attackDuration = attackDuration;
		enemy.attackRestDuration = attackRestDuration;

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
}
