using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(WeaponLaserData))]
public class WeaponLaserDataInspector : WeaponRangedDataInspector {

	SerializedProperty chargeSound;

	protected override void OnEnable() {
		base.OnEnable();

		chargeSound = serializedObject.FindProperty("chargeSound");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(icon, new GUIContent("アイコン"));
		EditorGUILayout.PropertyField(interval, new GUIContent("チャージ時間(秒)"));
		EditorGUILayout.PropertyField(power, new GUIContent("一秒に与えるダメージ"));
		EditorGUILayout.PropertyField(mod, new GUIContent("パッシブ効果"), true);
		EditorGUILayout.PropertyField(bulletData, new GUIContent("照射する弾のデータ"));

		EditorGUILayout.PropertyField(equipSound, new GUIContent("持ち替えた時のSE"));
		EditorGUILayout.PropertyField(useSound, new GUIContent("発射したときのSE"));
		EditorGUILayout.PropertyField(hitSound, new GUIContent("当たったときのSE"));
		EditorGUILayout.PropertyField(chargeSound, new GUIContent("チャージしているときのSE"));

		serializedObject.ApplyModifiedProperties();
	}
}