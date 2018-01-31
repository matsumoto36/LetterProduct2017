using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// テスト用
/// 指定のパーティクルを出す
/// </summary>
public class TestParticleSpawn : MonoBehaviour {

	public string particleName;

	void Awake() {
		//GameManagerに移行してください
		ParticleManager.Load();
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.P)) {
			ParticleManager.Spawn(particleName, transform.position, transform.rotation);
		}

		if(Input.GetKeyDown(KeyCode.L)) {
			ParticleManager.isRenderingParticle = !ParticleManager.isRenderingParticle;
		}
		if(Input.GetKeyDown(KeyCode.B)) {
			Debug.Break();
		}
	}


}
