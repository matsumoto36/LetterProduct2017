using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// PopcornFXのパーティクルを出したり消したりする
/// </summary>
public sealed class ParticleManager : SingletonMonoBehaviour<ParticleManager> {

	const string PARTICLE_DATA_PATH = "Particle";

	public static bool isRenderingParticle {
		get {
			if(!instance._renderer)
				instance._renderer = Camera.main.GetComponent<PKFxRenderingPlugin>();
			
			return instance._renderer.enabled;
		}
		set {
			if(!instance._renderer)
				instance._renderer = Camera.main.GetComponent<PKFxRenderingPlugin>();

			instance._renderer.enabled = value;

			if(!value) {
				//パーティクルの描画を消す
				PKFxManager.Reset();
			}
		}
	}

	PKFxRenderingPlugin _renderer;
	Dictionary<string, PKFxFX> particleTable;

	//外部からのnew禁止
	private ParticleManager() { }

	/// <summary>
	/// エフェクトをすべてロード
	/// </summary>
	public static void Load() {

		instance.particleTable = new Dictionary<string, PKFxFX>();

		var data = Resources.LoadAll<PKFxFX>(PARTICLE_DATA_PATH);

		foreach(var item in data) {
			instance.particleTable.Add(item.name, item);
		}
	}

	/// <summary>
	/// パーティクルを生成する(自動削除)
	/// </summary>
	/// <param name="particleName"></param>
	/// <param name="position"></param>
	/// <param name="rotation"></param>
	/// <returns></returns>
	public static PKFxFX Spawn(string particleName, Vector3 position, Quaternion rotation) {

		return Spawn(particleName, position, rotation, .1f);
	}

	/// <summary>
	/// パーティクルを生成する
	/// deleteTimeに0を設定すると削除されない
	/// </summary>
	/// <param name="particleName"></param>
	/// <param name="position"></param>
	/// <param name="rotation"></param>
	/// <returns></returns>
	public static PKFxFX Spawn(string particleName, Vector3 position, Quaternion rotation, float deleteTime) {

		var pData = instance.particleTable[particleName];

		if(pData == null) {
			Debug.LogError("particle is not found.");
			return null;
		}

		Debug.Log("Spawn:" + particleName);

		var pObj = Instantiate(pData, position, rotation);
		pObj.StartEffect();

		//自動削除の時間が設定されている場合
		if(deleteTime > 0) {
			Destroy(pObj.gameObject, deleteTime);
		}

		return pObj;
	}
}
