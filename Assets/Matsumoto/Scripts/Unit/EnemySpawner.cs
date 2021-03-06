﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵を一体スポーンさせる。EnemySpawnTriggerと連携必須
/// </summary>
[SelectionBase]
[ExecuteInEditMode]
public class EnemySpawner : MonoBehaviour {

	public static List<EnemySpawner> spawnerList { get; private set; }

	[Header("スポーンする敵のデータを入力して下さい")]
	public EnemyData enemyData;

	[Header("ステージを始めてすぐスポーンするか")]
	public bool spawnAwake = true;

	[Header("ここから見やすくするためのガイド")]
	public bool guide = true;
	public Color arrowColor = Color.red;
	public float arrowLength = 1;
	public float arrowWidth = 0.5f;

	int content = 2;
	LineRenderer arrowR;
	MeshRenderer pointR;
	GameObject arrow;
	GameObject model;

	static EnemySpawner()
	{
		spawnerList = new List<EnemySpawner>();
	}

	public static void Clear() {

		for(int i = 0;i < spawnerList.Count;i++) {
			Destroy(spawnerList[i].gameObject);
		}

		spawnerList = new List<EnemySpawner>();
	}

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

		spawnerList.Add(this);
		if(spawnAwake) SpawnEnemy(true);
	}

	void Update() {

		if(Application.isPlaying) return;

		if(transform.childCount > content) {
			Awake();
		}

		ArrowUpdate();

		if(enemyData) ModelUpdate();
	}

	/// <summary>
	/// 表示用モデルを取得する
	/// </summary>
	bool GetModel() {

		if(model) DestroyImmediate(model);
		if(!enemyData) return false;
		if(!enemyData.model) return false;

		model = Instantiate(enemyData.model);
		model.name = enemyData.model.name + "(Preview)";
		model.transform.SetParent(transform);
		model.GetComponent<Collider>().enabled = false;

		return true;
	}

	/// <summary>
	/// 表示用モデルの位置を更新する
	/// </summary>
	void ModelUpdate() {

		if(!model || model.name != enemyData.model.name + "(Preview)") {
			if(!GetModel()) return;
		}

		model.transform.SetPositionAndRotation(transform.position, transform.rotation);
	}

	/// <summary>
	/// 向きを表示するための各オブジェクトを取得する
	/// </summary>
	void GetArrow() {

		if(arrow) DestroyImmediate(arrow);

		arrow = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		arrow.name = "[Arrow]";
		arrow.transform.SetParent(transform);

		arrowR = arrow.AddComponent<LineRenderer>();
		pointR = arrow.GetComponent<MeshRenderer>();

		arrowR.sharedMaterial = pointR.sharedMaterial = Resources.Load<Material>("Material/GuideArrow");
		arrowR.positionCount = 2;

	}

	/// <summary>
	/// 向きを表示するための矢印を更新する
	/// </summary>
	void ArrowUpdate() {

		while(!arrowR || !pointR) {
			GetArrow();
		}

		arrowR.gameObject.SetActive(guide);

		if(guide) {
			arrowR.SetPosition(0, transform.position);
			arrowR.SetPosition(1, transform.position + transform.forward * arrowLength);
			arrowR.startWidth = arrowWidth;
			arrowR.endWidth = 0;
			arrowR.startColor = arrowR.endColor = arrowColor;
			pointR.transform.SetPositionAndRotation(transform.position, transform.rotation);
			pointR.transform.localScale = Vector3.one * arrowWidth;
			pointR.sharedMaterial.color = arrowColor;
		}

	}

	/// <summary>
	/// 敵をスポーンする
	/// </summary>
	/// <returns></returns>
	public void SpawnEnemy(bool removeSpawner) {
		SpawnEnemy(transform.position, transform.rotation, removeSpawner);
	}

	/// <summary>
	/// 敵をスポーンする
	/// </summary>
	/// <param name="position"></param>
	/// <param name="rotation"></param>
	/// <param name="removeSpawner"></param>
	public void SpawnEnemy(Vector3 position, Quaternion rotation, bool removeSpawner) {

		var enemy = enemyData.Spawn(position, rotation);
		enemy.StartCoroutine(SpawnAnim(enemy));

		if(removeSpawner) Destroy();
	}

	/// <summary>
	/// スポナーを削除する
	/// </summary>
	public void Destroy() {
		spawnerList.Remove(this);
		Destroy(gameObject);
	}


	IEnumerator SpawnAnim(Unit enemy) {

		var _renderer = enemy.GetComponentsInChildren<Renderer>();
		foreach(var item in _renderer) {
			item.enabled = false;
		}

		var ai = enemy.GetComponent<EnemyAI>();
		if(ai) ai.enabled = false;
		else if(enemy is EnemyStructure) ((EnemyStructure)enemy).enabled = false;

		yield return null;

		ParticleManager.Spawn("EnemySpawn", enemy.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity, 2);

		yield return new WaitForSeconds(0.5f);

		foreach(var item in _renderer) {
			item.enabled = true;
		}

		if(ai) ai.enabled = true;
		else if(enemy is EnemyStructure) ((EnemyStructure)enemy).enabled = true;

	}
}
