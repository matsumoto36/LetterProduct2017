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
	public bool isBoss;

	//共通のパラメータ
	public float attackLine = 3;            //近接攻撃可能距離
	public float searchAngle = 7;           //視野(度)

	//普通の敵の場合のパラメータ
	public EnemyAI.Mode enemyAIType;
	public float dashSpeed;
	public float moveLine;
	public float stepLine;

	//ボスの場合のパラメータ
	public float speed = 10;				//移動速度(秒速)
	public float targetChangeTime = 5.0f;	//ターゲット変更時間(秒)
	public int rollingMax = 2;				//何回転するか
	public float spAttackTime = 2;			//攻撃持続時間(秒)

	public override Unit Spawn(Vector3 position, Quaternion rot) {

		var enemy = SpawnUnit<Enemy>(position, rot);

		enemy.attackDuration = attackDuration;
		enemy.attackRestDuration = attackRestDuration;

		if(isBoss) {
			var ai = enemy.gameObject.AddComponent<BossAI>();
			ai.speed = enemy.moveSpeed;
			ai.attackLine = attackLine;
			ai.searchAngle = searchAngle;
			ai.targetChangeTime = targetChangeTime;
			ai.rollingMax = rollingMax;
			ai.spAttackTime = spAttackTime;
		}
		else {
			var ai = enemy.gameObject.AddComponent<EnemyAI>();
			ai.mode = enemyAIType;
			ai.speed = enemy.moveSpeed;
			ai.dashSpeed = dashSpeed;
			ai.moveLine = moveLine;
			ai.stepLine = stepLine;
			ai.attackLine = attackLine;
			ai.searchAngle = searchAngle;
		}


		return enemy;
	}
}
