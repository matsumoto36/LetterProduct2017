using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル用のエフェクトを指定のタイミングで止めるクラス
/// </summary>
public class CameraTimelineControl : MonoBehaviour {

	PKFxRenderingPlugin cameraFx;

	float sizeMax;
	public bool isStop;
	bool _isStop;

	void Awake() {
		GetRenderer();
		StartCoroutine(CameraUpdate());
	}

	void GetRenderer() {
		cameraFx = GetComponent<PKFxRenderingPlugin>();
	}

	IEnumerator CameraUpdate() {

		while(true) {

			if(isStop != _isStop) {
				cameraFx.m_TimeMultiplier = isStop ? 0 : 1;
				_isStop = isStop;
			}

			yield return null;
		}

	}
}
