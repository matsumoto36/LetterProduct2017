using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲーム内で改変される可能性のある計算式全部入り
/// </summary>
public sealed class GameBalance : SingletonMonoBehaviour<GameBalance> {

	public GameBalanceData data { get; private set; }

	protected override void Init() {
		base.Init();

		//データをロード
		data = Resources.Load<GameBalanceData>(GameBalanceData.ASSET_PATH);
		if(!data) Debug.LogError("Failed load GameBalanceData.");
	}

	/// <summary>
	/// プレイヤー人数で変わる敵のHP倍率を計算
	/// </summary>
	/// <param name="playerCount"></param>
	/// <returns></returns>
	public static float CalcEnemyHPEnrich(int playerCount) {
		if(playerCount == 1) return 1;
		return instance.data.enemyHPMul / (3 / (playerCount - 1)) + 1;
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

	/// <summary>
	/// コンボに対して効果を得る
	/// </summary>
	/// <param name="mod"></param>
	/// <param name="combo"></param>
	public static void ApplyNextComboStatus(StatusModifier mod, int combo) {
		if(combo == 0) return;
		mod.mulPow = Mathf.Pow(instance.data.comboMulPower, combo / 10.0f + 1);
	}

	public static int CalcScore(int combo, float stageClearTime, float bossClearTime) {

		var comboScore = (1 + (float)combo / 10) * 10;
		var bossScore = 1000 + Mathf.Max(200 - bossClearTime, 0);
		var stageScore = Mathf.Max(600 - stageClearTime, 0);

		return (int)((comboScore + bossScore + stageScore) * 10);
	}

	/// <summary>
	/// コンボの持続を求める
	/// </summary>
	/// <param name="combo"></param>
	/// <returns></returns>
	public static float CalcNextComboDuration(int combo) {
		return instance.data.comboStartDuration / (1 + combo / 20.0f);
	}
}
