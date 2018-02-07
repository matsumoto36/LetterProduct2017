using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	[Header("各移動速度")]
	public float moveRatio = 0.1f;
	[Header("ズームの範囲")]
	public float maxCamLength;
	public float minCamLength;
	[Header("各種オフセット")]
	public Vector3 trackOffset;
	public float lengthOffset;

	Vector3 camPosition;
	float camLength;

	Camera cam;

	void Awake() {
		cam = GetComponent<Camera>();
	}

	void FixedUpdate() {
		
		//カメラの目標位置を計算
		CalcCameraPosition();
		//移動
		Move();
	}

	/// <summary>
	/// カメラの位置を求める
	/// </summary>
	void CalcCameraPosition() {

		if(Unit.unitList.Count == 0) return;

		//収めるUnitを抽出
		var unitList = Unit.unitList
			.Where((item) => item)
			.ToArray();
		if(unitList.Length == 0) return;

		//Unit達の中心を求める
		var trackPos = unitList
			.Select((item) => item.transform.position)
			.Aggregate((from, to) => from + to) / unitList.Length;

		//一番遠いUnitを抽出
		var diffMax = unitList
			.Select((item) => {
				var diff = item.transform.position - trackPos;
				return Mathf.Max(
					//スクリーンの比で合わせる
					Mathf.Abs(diff.x) * ((float)Screen.height / Screen.width),
					Mathf.Abs(diff.y),
					Mathf.Abs(diff.z)
					);
			})
			.Max();

		//目標の長さを求める
		camLength = Mathf.Abs(diffMax * Mathf.Tan(Mathf.Deg2Rad * (90 - cam.fieldOfView / 2))) + lengthOffset;

		//長さを制限
		camLength = Mathf.Clamp(camLength, minCamLength, maxCamLength);

		//カメラの位置を調整
		camPosition = trackPos - transform.forward * camLength + trackOffset;
	}

	/// <summary>
	/// 移動
	/// </summary>
	void Move() {
		//補間で移動する
		transform.position = Vector3.Lerp(transform.position, camPosition, moveRatio);
	}
}