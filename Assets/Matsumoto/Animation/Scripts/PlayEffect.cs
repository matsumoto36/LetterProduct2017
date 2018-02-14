using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル用のエフェクトを指定のタイミングで再生するクラス
/// </summary>
public class PlayEffect : MonoBehaviour {

	public bool startEffect;
	PKFxFX effect;

	void Awake() {
		effect = GetComponent<PKFxFX>();
		StartCoroutine(WaitEffectStart());
	}

	IEnumerator WaitEffectStart() {

		while(!startEffect) {
			yield return null;
		}

		effect.StartEffect();
	}

}
