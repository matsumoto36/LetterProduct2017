using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameBalanceData))]
public class GameBalanceDataInspector : Editor {

	SerializedProperty nextExpMagnification;
	SerializedProperty levelUpMulHP;
	SerializedProperty levelUpMulPower;
	SerializedProperty healPointPerExp;
	SerializedProperty duraEggCanUseRatio;
	SerializedProperty duraEggChargeTime;
	SerializedProperty duraEggExitTime;
	SerializedProperty duraEggExitWeakTime;
	SerializedProperty duraEggExitDamageMag;
	SerializedProperty reviveTime;

	void OnEnable() {
		nextExpMagnification = serializedObject.FindProperty("nextExpMagnification");
		levelUpMulHP = serializedObject.FindProperty("levelUpMulHP");
		levelUpMulPower = serializedObject.FindProperty("levelUpMulPower");
		healPointPerExp = serializedObject.FindProperty("healPointPerExp");
		duraEggCanUseRatio = serializedObject.FindProperty("duraEggCanUseRatio");
		duraEggChargeTime = serializedObject.FindProperty("duraEggChargeTime");
		duraEggExitTime = serializedObject.FindProperty("duraEggExitTime");
		duraEggExitWeakTime = serializedObject.FindProperty("duraEggExitWeakTime");
		duraEggExitDamageMag = serializedObject.FindProperty("duraEggExitDamageMag");
		reviveTime = serializedObject.FindProperty("reviveTime");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(nextExpMagnification, new GUIContent("次のレベルの必要経験値倍率"));
		EditorGUILayout.PropertyField(levelUpMulHP, new GUIContent("レベルアップ時のHP増加倍率"));
		EditorGUILayout.PropertyField(levelUpMulPower, new GUIContent("レベルアップ時の攻撃力増加倍率"));
		EditorGUILayout.PropertyField(healPointPerExp, new GUIContent("1回復時の取得経験値量"), true);
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(duraEggCanUseRatio, new GUIContent("耐久卵を使えるHPの残量割合"));
		EditorGUILayout.PropertyField(duraEggChargeTime, new GUIContent("耐久卵を使うときの必要時間"));
		EditorGUILayout.PropertyField(duraEggExitTime, new GUIContent("耐久卵から出るときの必要時間"));
		EditorGUILayout.PropertyField(duraEggExitWeakTime, new GUIContent("耐久卵から出た時の弱くなる時間"));
		EditorGUILayout.PropertyField(duraEggExitDamageMag, new GUIContent("耐久卵で弱くなった時のダメージ倍率"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(reviveTime, new GUIContent("復活に必要な時間"));

		serializedObject.ApplyModifiedProperties();
	}
}