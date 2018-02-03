using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 便利メソッドたち
/// </summary>
public class UtilityMethod : MonoBehaviour {


	/// <summary>
	/// 一定時間時間待ってから実行する
	/// </summary>
	/// <param name="delayExecution"></param>
	/// <param name="delay"></param>
	/// <returns></returns>
	public static Coroutine DelayExecution(Action delayExecution, float delay) {
		var g = new GameObject("[DelayExecution]").AddComponent<UtilityMethod>();
		DontDestroyOnLoad(g.gameObject);
		return g.StartCoroutine(g.DelayWait(delayExecution, delay));
	}

	IEnumerator DelayWait(Action delayExecution, float delay) {
		yield return new WaitForSeconds(delay);
		delayExecution();
		Destroy(gameObject);
	}
}
