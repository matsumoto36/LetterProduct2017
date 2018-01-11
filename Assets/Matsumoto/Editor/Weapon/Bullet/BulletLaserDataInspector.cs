using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(BulletLaserData))]
public class BulletLaserDataInspector : BulletDataInspector {

	SerializedProperty maxLength;
	SerializedProperty isHeal;

	protected override void OnEnable() {
		base.OnEnable();

		maxLength = serializedObject.FindProperty("maxLength");
		isHeal = serializedObject.FindProperty("isHeal");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(particleName, new GUIContent("再生するパーティクルの名前"));
		EditorGUILayout.PropertyField(maxLength, new GUIContent("最大の照射する長さ"));
		EditorGUILayout.PropertyField(isHeal, new GUIContent("回復弾の場合はチェック"));

		serializedObject.ApplyModifiedProperties();
	}
}