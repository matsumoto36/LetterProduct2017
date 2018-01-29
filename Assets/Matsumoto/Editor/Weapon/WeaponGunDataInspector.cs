using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(WeaponGunData))]
public class WeaponGunDataInspector : WeaponRangedDataInspector {

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(icon, new GUIContent("アイコン"));
		EditorGUILayout.PropertyField(interval, new GUIContent("次の発射までの待ち時間"));
		EditorGUILayout.PropertyField(power, new GUIContent("一発のダメージ"));
		EditorGUILayout.PropertyField(mod, new GUIContent("パッシブ効果"), true);
		EditorGUILayout.PropertyField(bulletData, new GUIContent("発射する弾のデータ"));

		serializedObject.ApplyModifiedProperties();
	}
}