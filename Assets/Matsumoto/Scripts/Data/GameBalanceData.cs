using UnityEngine;

public class GameBalanceData : ScriptableObject {

	public const string ASSET_PATH = "Data/GameBalance";

	public float nextExpMagnification = 1.1f;	//次のレベルの必要経験値倍率
	public float levelUpMulHP = 1.01f;			//レベルアップ時のHP増加倍率
	public float levelUpMulPower = 1.10f;		//レベルアップ時の攻撃力増加倍率
	public float healPointPerExp = 0.25f;		//1回復時の取得経験値量
	public float duraEggCanUseRatio = 0.25f;	//耐久卵を使えるようになるHPの残量割合
	public float duraEggChargeTime = 0.1f;		//耐久卵を使うときの必要時間
	public float duraEggExitTime = 0.1f;		//耐久卵から出るときの必要時間
	public float duraEggExitWeakTime = 2f;		//耐久卵から出た時の弱くなる時間
	public float reviveTime = 4f;				//復活に必要な時間

}