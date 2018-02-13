using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(EnemyData))]
public class EnemyDataInspector : UnitDataInspector {

	protected SerializedProperty attackDuration;
	protected SerializedProperty attackRestDuration;

    SerializedProperty searchDegree;
    SerializedProperty searchDistance;
    SerializedProperty attackRange;

    protected override void OnEnable() {
		base.OnEnable();

		attackDuration = serializedObject.FindProperty("attackDuration");
		attackRestDuration = serializedObject.FindProperty("attackRestDuration");

        searchDegree = serializedObject.FindProperty("searchDegree");
        searchDistance = serializedObject.FindProperty("searchDistance");
        attackRange = serializedObject.FindProperty("attackRange");
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
        EditorGUILayout.PropertyField(searchDegree, new GUIContent("視野角(度)"));
        EditorGUILayout.PropertyField(searchDistance, new GUIContent("視認距離"));
        EditorGUILayout.PropertyField(attackRange, new GUIContent("射程距離"));
		EditorGUILayout.Separator();
        EditorGUILayout.PropertyField(weaponData.GetArrayElementAtIndex(0), new GUIContent("武器"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(deathSE, new GUIContent("死んだときのSE"));
		EditorGUILayout.PropertyField(deathParticle, new GUIContent("死んだときのパーティクル"));

		serializedObject.ApplyModifiedProperties();
	}
}