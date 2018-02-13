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
    public float searchDegree;
    public float searchDistance;
    public float attackRange;

    public override Unit Spawn(Vector3 position, Quaternion rot) {

		var enemy = SpawnUnit<Enemy>(position, rot);

		enemy.attackDuration = attackDuration;
		enemy.attackRestDuration = attackRestDuration;

        var ai = enemy.gameObject.AddComponent<EnemyAI>();
        ai.searchDegree = searchDegree;
        ai.searchDistance = searchDistance;
        ai.attackRange = attackRange;

        return enemy;
	}
}
