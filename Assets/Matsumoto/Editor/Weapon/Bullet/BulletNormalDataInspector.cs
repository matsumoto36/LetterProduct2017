using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(BulletNormalData))]
public class BulletNormalDataInspector : BulletDataInspector {

	protected SerializedProperty speed;

	protected override void OnEnable() {
		base.OnEnable();

		speed = serializedObject.FindProperty("speed");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(particleName, new GUIContent("再生するパーティクルの名前"));
		EditorGUILayout.PropertyField(speed, new GUIContent("弾の速さ"));

		serializedObject.ApplyModifiedProperties();
	}
}