using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(EnemyStructureData))]
public class EnemyStructureDataInspector : EnemyDataInspector {

	SerializedProperty detectAngle;
	SerializedProperty detectRadius;
	SerializedProperty isDetectAttacked;

	protected override void OnEnable() {
		base.OnEnable();

		detectAngle = serializedObject.FindProperty("detectAngle");
		detectRadius = serializedObject.FindProperty("detectRadius");
		isDetectAttacked = serializedObject.FindProperty("isDetectAttacked");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(hp, new GUIContent("体力"));
		EditorGUILayout.PropertyField(dropExp, new GUIContent("落とす経験値量(総量)"));
		EditorGUILayout.PropertyField(nextLevelExp, new GUIContent("レベル1から2にあげるための経験値量"));
		EditorGUILayout.PropertyField(moveSpeed, new GUIContent("移動速度"));
		EditorGUILayout.PropertyField(rotSpeed, new GUIContent("回転速度"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(attackDuration, new GUIContent("攻撃時間"));
		EditorGUILayout.PropertyField(attackRestDuration, new GUIContent("攻撃休止時間"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(detectAngle, new GUIContent("視野角(度)"));
		EditorGUILayout.PropertyField(detectRadius, new GUIContent("視認できる範囲(半径)"));
		EditorGUILayout.PropertyField(isDetectAttacked, new GUIContent("攻撃されたときに振り向くか"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(weaponData.GetArrayElementAtIndex(0), new GUIContent("メイン武器"));

		serializedObject.ApplyModifiedProperties();
	}
}