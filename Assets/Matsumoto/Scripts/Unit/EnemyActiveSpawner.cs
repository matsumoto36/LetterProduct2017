using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 継続的に敵をスポーンさせる
/// </summary>
public class EnemyActiveSpawner : Enemy {

	public int hp;
	public int dropEXP;

	public EnemyData spawnEnemy;		//湧かせるキャラクター
	public float interval;				//湧き頻度(秒)
	public RangeInteger spawnCount;		//湧く数の範囲
	public int spawnMax;				//湧く範囲に居れるキャラクターの最大数

	public float activeRange;			//稼働範囲
	public float spawnRange;			//湧く範囲

	public string _deathSE;				//死んだときのSE
	public string _deathParticle;		//死んだときのパーティクル

	public override void InitFinal() {
		base.InitFinal();

		//勢力のセット
		group = UnitGroup.Enemy;
	}

	void Start() {
		InitFirst();
		SetInitData(hp, dropEXP, 0, 0, 0);
		InitFinal();
	}

	public override void Attack() { }

	public override void Move() { }

	void OnDrawGizmos() {

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, spawnRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, spawnRange);

	}

}
