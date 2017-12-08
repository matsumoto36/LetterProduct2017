using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内で改変される可能性のある計算式全部入り
/// </summary>
public sealed class GameBalance : SingletonMonoBehaviour<GameBalance> {

	public GameBalanceData data { get; private set; }

	void Awake() {
		//データをロード
		data = Resources.Load<GameBalanceData>(GameBalanceData.ASSET_PATH);
		if(!data) Debug.LogError("Failed load GameBalanceData.");
	}

	/// <summary>
	/// 次のレベルに必要な経験値を取得
	/// </summary>
	/// <param name="baseEXP">1から2に上げるための経験値</param>
	/// <param name="level">現在のレベル</param>
	/// <returns></returns>
	public static int CalcNextLevelExp(int baseNextLevel, int level) {
		if(level == 0) return 0;
		if(level == 1) return baseNextLevel;
		return (int)(baseNextLevel * Mathf.Pow(instance.data.nextExpMagnification, level));
	}

	/// <summary>
	/// 次のレベルのステータスを適用
	/// </summary>
	/// <param name="mod"></param>
	public static void ApplyNextLevelStatus(StatusModifier mod, int level) {
		if(level == 0) return;
		mod.mulHP = Mathf.Pow(instance.data.levelUpMulHP, level-1);
		mod.mulPow = Mathf.Pow(instance.data.levelUpMulPower, level-1);
	}

	/// <summary>
	/// 回復量から経験値量を計算する
	/// </summary>
	/// <param name="healPoint"></param>
	/// <returns></returns>
	public static float CalcHealExp(int healPoint) {
		return healPoint * instance.data.healPointPerExp;
	}
}
