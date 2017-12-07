using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(WeaponLaserData))]
public class WeaponLaserDataInspector : WeaponRangedDataInspector {

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(interval, new GUIContent("チャージ時間(秒)"));
		EditorGUILayout.PropertyField(power, new GUIContent("一秒に与えるダメージ"));
		EditorGUILayout.PropertyField(mod, new GUIContent("パッシブ効果"), true);
		EditorGUILayout.PropertyField(bulletData, new GUIContent("照射する弾のデータ"));

		serializedObject.ApplyModifiedProperties();
	}
}