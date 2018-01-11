using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(EnemyData))]
public class EnemyDataInspector : UnitDataInspector {

	protected SerializedProperty attackDuration;
	protected SerializedProperty attackRestDuration;

	SerializedProperty enemyAIType;
	SerializedProperty dashSpeed;
	SerializedProperty moveLine;
	SerializedProperty stepLine;
	SerializedProperty attackLine;
	SerializedProperty searchAngle;

	protected override void OnEnable() {
		base.OnEnable();

		attackDuration = serializedObject.FindProperty("attackDuration");
		attackRestDuration = serializedObject.FindProperty("attackRestDuration");

		enemyAIType = serializedObject.FindProperty("enemyAIType");
		dashSpeed = serializedObject.FindProperty("dashSpeed");
		moveLine = serializedObject.FindProperty("moveLine");
		stepLine = serializedObject.FindProperty("stepLine");
		attackLine = serializedObject.FindProperty("attackLine");
		searchAngle = serializedObject.FindProperty("searchAngle");
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
		EditorGUILayout.PropertyField(enemyAIType, new GUIContent("敵AIの種類"));
		EditorGUILayout.PropertyField(searchAngle, new GUIContent("視野"));
		EditorGUILayout.PropertyField(moveLine, new GUIContent("検知範囲"));
		EditorGUILayout.PropertyField(dashSpeed, new GUIContent("急接近時の速度"));
		EditorGUILayout.PropertyField(attackLine, new GUIContent("攻撃距離"));

		switch((EnemyAI.Mode)enemyAIType.enumValueIndex) {
			case EnemyAI.Mode.BASIS:
				break;
			case EnemyAI.Mode.APPROACH:
				EditorGUILayout.PropertyField(stepLine, new GUIContent("ステップ攻撃範囲"));
				break;
			case EnemyAI.Mode.DUAL:
				EditorGUILayout.PropertyField(stepLine, new GUIContent("ステップ攻撃範囲"));
				break;
			default:
				break;
		}
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(weaponData.GetArrayElementAtIndex(0), new GUIContent("メイン武器"));
		EditorGUILayout.PropertyField(weaponData.GetArrayElementAtIndex(1), new GUIContent("サブ武器"));

		serializedObject.ApplyModifiedProperties();
	}
}