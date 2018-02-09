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
	public string _deathParticle;		//死んだときのパーティクル

	public Transform modelParent;		//プレビュー機能で表示するオブジェクトの親

	float activeTime = 0;
	public override void InitFinal() {
		base.InitFinal();

		//勢力のセット
		group = UnitGroup.Enemy;
	}

	public override void Attack() { }

	public override void Move() { }

	void Awake() {

		foreach(Transform item in modelParent) {

			if(Application.isPlaying) {
				Destroy(item.gameObject);
			}
			else {
				DestroyImmediate(item.gameObject);
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
			if(!modelParent) return;
			ViewModelUpdate();
		}
	}

	void ViewModelUpdate() {

		if(modelParent.childCount != 1) {
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
			var randCircle = Random.insideUnitCircle * spawnRange;
			var position = new Vector3(randCircle.x, 0, randCircle.y);
			spawnEnemy.Spawn(transform.position + position, transform.rotation);
		}
	}

	/// <summary>
	/// 表示用モデルを取得する
	/// </summary>
	bool GetModel() {

		if(modelParent.childCount != 0) Awake();
		if(!spawnEnemy) return false;
		if(!spawnEnemy.model) return false;
		
		var model = Instantiate(spawnEnemy.model);
		model.name = spawnEnemy.model.name + "(Preview)";
		model.transform.SetParent(modelParent);

		return true;
	}

	/// <summary>
	/// 表示用モデルの位置を更新する
	/// </summary>
	void ModelUpdate() {

		if(modelParent.childCount == 0) {
			if(!GetModel()) return;
		}

		var model = modelParent.GetChild(0).gameObject;
		if(model.name != spawnEnemy.model.name + "(Preview)") {
			if(!GetModel()) return;
		}
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
