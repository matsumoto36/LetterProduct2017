using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 敵を一体スポーンさせる。EnemySpawnTriggerと連携必須
/// </summary>
[SelectionBase]
[ExecuteInEditMode]
public class EnemySpawner : MonoBehaviour {

	[Header("スポーンする敵の番号を入力して下さい")]
	public int enemyNum;

	[Header("ここから見やすくするためのガイド")]
	public bool guide = true;
	public Color arrowColor = Color.red;
	public float arrowLength = 1;
	public float arrowWidth = 0.5f;

	int content = 2;
	int _enemyNum;
	LineRenderer arrowR;
	MeshRenderer pointR;
	GameObject arrow;
	GameObject model;

	void Start() {
		foreach(Transform item in transform) {
			if(Application.isPlaying) {
				Destroy(item.gameObject);
			}
			else {
				DestroyImmediate(item.gameObject);
			}
		}
	}

	void Update() {

		if(Application.isPlaying) return;

		if(transform.childCount > content) {
			Start();
		}

		if(enemyNum != _enemyNum) {
			if(model) DestroyImmediate(model.gameObject);
			_enemyNum = enemyNum;
		}

		ArrowUpdate();
		ModelUpdate();
	}

	/// <summary>
	/// 表示用モデルを取得する
	/// </summary>
	void GetModel() {

		if(model) DestroyImmediate(model);

		var num = enemyNum + 1;
		var data = CSVLoader.LoadData(UnitDataContainer.UNIT_DATA_PATH);

		if(data.Count <= num) return;

		model = Instantiate(Resources.Load<GameObject>(UnitManager.UNIT_MODEL_PATH + data[num][1]));
		model.name += "(Preview)";
		model.transform.SetParent(transform);
	}

	/// <summary>
	/// 表示用モデルの位置を更新する
	/// </summary>
	void ModelUpdate() {

		while(!model) {
			GetModel();
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
	public Enemy SpawnEnemy() {
		//0番はプレイヤーなので +1する
		return (Enemy)UnitManager.SpawnUnit(enemyNum + 1, transform.position, transform.rotation);
	}
}
