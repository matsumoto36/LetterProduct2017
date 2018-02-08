using UnityEngine;
using System.Collections;
using System.Linq;

/// <summary>
/// 継続的に敵をスポーンさせる
/// </summary>
[ExecuteInEditMode]
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
	public string _deathParticle;       //死んだときのパーティクル

	float activeTime = 0;
	public override void InitFinal() {
		base.InitFinal();

		//勢力のセット
		group = UnitGroup.Enemy;
	}

	public override void Attack() { }

	public override void Move() { }

	void Awake() {
		foreach(Transform item in transform) {
			if(Application.isPlaying) {
				if(item) Destroy(item.gameObject);
			}
			else {
				if(item) DestroyImmediate(item.gameObject);
			}
		}
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

		if(Application.isPlaying) {

			if(!CheckActive()) return;
			if(!((activeTime += Time.deltaTime) > interval)) return;

			activeTime = 0;
			Spawn();
		}
		else {

			ViewModelUpdate();
		}
	}

	void ViewModelUpdate() {

		if(transform.childCount > 1) {
			Awake();
		}

		if(spawnEnemy) ModelUpdate();
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

			var rad = Random.Range(0, Mathf.PI * 2);
			var position = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * spawnRange;

			spawnEnemy.Spawn(transform.position + position, transform.rotation);
		}
	}

	/// <summary>
	/// 表示用モデルを取得する
	/// </summary>
	bool GetModel() {

		if(transform.childCount != 0) DestroyImmediate(transform.GetChild(0));
		if(!spawnEnemy) return false;
		if(!spawnEnemy.model) return false;

		var model = Instantiate(spawnEnemy.model);
		model.name = spawnEnemy.model.name + "(Preview)";
		model.transform.SetParent(transform);

		return true;
	}

	/// <summary>
	/// 表示用モデルの位置を更新する
	/// </summary>
	void ModelUpdate() {

		if(transform.childCount == 0) {
			if(!GetModel()) return;
		}

		var model = transform.GetChild(0).gameObject;
		if(model.name != spawnEnemy.model.name + "(Preview)") {
			if(!GetModel()) return;
		}

		model.transform.SetPositionAndRotation(transform.position, transform.rotation);
	}

	/// <summary>
	/// 稼働してよいか調べる
	/// </summary>
	/// <returns></returns>
	bool CheckActive() {

		foreach(var item in GameData.instance.spawnedPlayer) {
			if(!item) continue;
			if((transform.position - item.transform.position).magnitude < activeRange)
				return true;
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
