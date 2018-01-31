using UnityEngine;

public class GameBalanceData : ScriptableObject {

	public const string ASSET_PATH = "Data/GameBalance";

	public float enemyHPMul = 2;				//プレイヤー4人のときの敵の体力の倍率
	public float nextExpMagnification = 1.1f;	//次のレベルの必要経験値倍率
	public float levelUpMulHP = 1.01f;			//レベルアップ時のHP増加倍率
	public float levelUpMulPower = 1.10f;		//レベルアップ時の攻撃力増加倍率
	public float healPointPerExp = 0.25f;		//1回復時の取得経験値量
	public float duraEggCanUseRatio = 0.25f;	//耐久卵を使えるHPの残量割合
	public float duraEggChargeTime = 0.1f;		//耐久卵を使うときの必要時間
	public float duraEggExitTime = 0.1f;		//耐久卵から出るときの必要時間
	public float duraEggExitWeakTime = 2f;		//耐久卵から出た時の弱くなる時間
	public float duraEggExitDamageMag = 2f;		//耐久卵で弱くなった時のダメージ倍率
	public float revivableRadius = 2f;			//復活可能な距離
	public float reviveTime = 4f;				//復活に必要な時間
	public float comboStartDuration = 2.0f;		//1コンボ目で待ってくれる時間
	public float comboMulPower = 1.001f;		//コンボで増える攻撃力増加倍率

}