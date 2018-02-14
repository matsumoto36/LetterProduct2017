using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトル用の攻撃エフェクトを指定のタイミングで実行するクラス
/// </summary>
public class MeleeAttackControl : MonoBehaviour {

	PKFxFX meleeAttack;
	PKFxManager.Attribute sizeMaxAttribute;

	float sizeMax;
	public bool isEmit;
	bool _isEmit;

	void Awake() {
		GetEffect();
		sizeMaxAttribute.ValueFloat = isEmit ? sizeMax : 0;

		StartCoroutine(EffectUpdate());
	}

	void GetEffect() {
		meleeAttack = GetComponent<PKFxFX>();
		sizeMaxAttribute = meleeAttack.GetAttribute("SizeMax");
		sizeMax = sizeMaxAttribute.ValueFloat;
	}

	IEnumerator EffectUpdate() {

		while(true) {

			if(isEmit != _isEmit) {
				if(sizeMaxAttribute == null) GetEffect();
				sizeMaxAttribute.ValueFloat = isEmit ? sizeMax : 0;
				_isEmit = isEmit;
			}

			yield return null;
		}

	}
}
