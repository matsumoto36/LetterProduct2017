using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// PopcornFXのパーティクルを出したり消したりする
/// </summary>
public sealed class ParticleManager : SingletonMonoBehaviour<ParticleManager> {

	const string PARTICLE_BASE_PATH = "System/ParticleBase";
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
	Dictionary<string, ObjectPooler> poolTable;
	Dictionary<string, PKFxFX> particleTable;

	//外部からのnew禁止
	private ParticleManager() { }

	protected override void Init() {
		base.Init();
		Load();
	}

	/// <summary>
	/// エフェクトをすべてロード
	/// </summary>
	public static void Load() {

		instance.particleTable = new Dictionary<string, PKFxFX>();
		instance.poolTable = new Dictionary<string, ObjectPooler>();

		var data = Resources.LoadAll<PKFxFX>(PARTICLE_DATA_PATH);

		foreach(var item in data) {

			PKFxManager.PreLoadFxIFN(PARTICLE_DATA_PATH + "/" + item.name + ".pkfx");

			instance.particleTable.Add(item.name, item);

			var pool = ObjectPooler.GetObjectPool(item.gameObject);
			pool.maxCount = 200;
			pool.prepareCount = 100;
			pool.Generate(instance.transform);
			instance.poolTable.Add(item.name, pool);
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

		if(!instance.particleTable.ContainsKey(particleName)) {
			Debug.LogWarning(particleName + " is not found.");
			return null;
		}

		var pData = instance.particleTable[particleName];
		var pObj = instance.poolTable[particleName].GetInstance().GetComponent<PKFxFX>();
		pObj.transform.SetPositionAndRotation(position, rotation);
		pObj.StartEffect();

		//自動削除の時間が設定されている場合
		if(deleteTime > 0) {
			UtilityMethod.DelayExecution(() => StopParticle(pObj), deleteTime);
			//Destroy(pObj.gameObject, deleteTime);
		}

		return pObj;
	}

	/// <summary>
	/// エフェクトの再生を止めてプールに返す
	/// </summary>
	/// <param name="particle"></param>
	public static void StopParticle(PKFxFX particle) {
		//particle.m_IsPlaying = false;
		particle.TerminateEffect();
		ObjectPooler.ReleaseInstance(particle.gameObject);
	}
}
