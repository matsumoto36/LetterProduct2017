using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(EnemyActiveSpawner))]
public class EnemySpawnerInspector : Editor {

	SerializedProperty hp;
	SerializedProperty dropExp;

	SerializedProperty spawnEnemy;
	SerializedProperty interval;
	SerializedProperty spawnCount;
	SerializedProperty spawnMax;

	SerializedProperty activeRange;
	SerializedProperty spawnRange;

	SerializedProperty deathSE;
	SerializedProperty deathParticle;

	protected virtual void OnEnable() {

		hp = serializedObject.FindProperty("hp");
		dropExp = serializedObject.FindProperty("dropExp");

		spawnEnemy = serializedObject.FindProperty("spawnEnemy");
		interval = serializedObject.FindProperty("interval");
		spawnCount = serializedObject.FindProperty("spawnCount");
		spawnMax = serializedObject.FindProperty("spawnMax");

		activeRange = serializedObject.FindProperty("activeRange");
		spawnRange = serializedObject.FindProperty("spawnRange");

		deathSE = serializedObject.FindProperty("_deathSE");
		deathParticle = serializedObject.FindProperty("_deathParticle");
	}

	public override void OnInspectorGUI() {

		EditorGUILayout.PropertyField(hp, new GUIContent("体力"));
		EditorGUILayout.PropertyField(dropExp, new GUIContent("落とす経験値"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(spawnEnemy, new GUIContent("スポーンする敵のデータ"));
		EditorGUILayout.PropertyField(interval, new GUIContent("スポーンするインターバル"));
		EditorGUILayout.PropertyField(spawnCount, new GUIContent("一回にスポーンする数"));
		EditorGUILayout.PropertyField(spawnMax, new GUIContent("湧き範囲にいる最大の敵数"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(activeRange, new GUIContent("可動範囲(半径)"));
		EditorGUILayout.PropertyField(spawnRange, new GUIContent("湧き範囲(半径)"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(deathSE, new GUIContent("死んだときのSE"));
		EditorGUILayout.PropertyField(deathParticle, new GUIContent("死んだときのパーティクル"));
	}
}