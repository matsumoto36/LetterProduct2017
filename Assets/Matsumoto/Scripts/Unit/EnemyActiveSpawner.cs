using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// 継続的に敵をスポーンさせる
/// </summary>
//[ExecuteInEditMode]
public class EnemyActiveSpawner : Enemy {

	public int hp;
	public int dropEXP;

	public float interval;				//湧き頻度(秒)
	public RangeInteger spawnCount;		//湧く数の範囲
	public int spawnMax;				//湧く範囲に居れるキャラクターの最大数

	public float activeRange;			//稼働範囲
	public float spawnRange;			//湧く範囲

	public string _deathSE;				//死んだときのSE
	public string _deathParticle;		//死んだときのパーティクル

	public Transform modelParent;       //プレビュー機能で表示するオブジェクトの親

	Player activatePlayer;

	EnemySpawner spawner;
	float activeTime = 0;

	public override void InitFinal() {
		base.InitFinal();

		//勢力のセット
		group = UnitGroup.Enemy;

		spawner = GetComponentInChildren<EnemySpawner>();
	}

	public override void Attack() { }

	public override void Move() { }

	public override void Death() {

		//消される前にスポナーを消す
		spawner.Destroy();

		base.Death();
	}

	void Start() {

		if(!Application.isPlaying) return;

		InitFirst();
		deathSE = _deathSE;
		deathParticle = _deathParticle;
		SetInitData(hp, dropEXP, 0, 0, 0);
		InitFinal();
	}

	void Update() {

		if(!CheckActive()) return;
		if(!((activeTime += Time.deltaTime) > interval)) return;

		activeTime = 0;
		Spawn();
	}

	/// <summary>
	/// 敵をスポーンさせる
	/// </summary>
	void Spawn() {

		var insideEnemyCount = unitList
			.Where(item => item)
			.Where(item => (item.transform.position - transform.position).magnitude < spawnRange)
			.Count();

		var count = Mathf.Min(spawnCount.RandomValue, spawnMax - insideEnemyCount);

		for(int i = 0;i < count;i++) {
			var randCircle = Random.insideUnitCircle * spawnRange;
			var position = new Vector3(randCircle.x, 0, randCircle.y) + transform.position;
			var diff = activatePlayer.transform.position - position;
			diff.y = 0;
			spawner.SpawnEnemy(position, Quaternion.LookRotation(diff, Vector3.up), false);
		}
	}

	/// <summary>
	/// 稼働してよいか調べる
	/// </summary>
	/// <returns></returns>
	bool CheckActive() {

		foreach(var item in GameData.instance.spawnedPlayer) {
			if(!item) continue;
			if((transform.position - item.transform.position).magnitude < activeRange) {
				activatePlayer = item;
				return true;
			}
		}

		return false;
	}

	void OnDrawGizmos() {

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, activeRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, spawnRange);

	}

}
