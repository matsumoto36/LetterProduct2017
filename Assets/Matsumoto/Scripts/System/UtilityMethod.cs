using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 便利メソッドたち
/// </summary>
public sealed class UtilityMethod : SingletonMonoBehaviour<UtilityMethod> {

	//外部からのnew禁止
	private UtilityMethod() { }

	/// <summary>
	/// 一定時間時間待ってから実行する
	/// </summary>
	/// <param name="delayExecution"></param>
	/// <param name="delay"></param>
	/// <returns></returns>
	public static Coroutine DelayExecution(Action delayExecution, float delay) {
		return instance.StartCoroutine(instance.DelayWait(delayExecution, delay));
	}

	IEnumerator DelayWait(Action delayExecution, float delay) {
		yield return new WaitForSeconds(delay);
		delayExecution();
	}
}
