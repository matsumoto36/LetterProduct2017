using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(BulletNormalData))]
public class BulletNormalDataInspector : BulletDataInspector {

	protected SerializedProperty speed;
	protected SerializedProperty range;

	protected override void OnEnable() {
		base.OnEnable();

		speed = serializedObject.FindProperty("speed");
		range = serializedObject.FindProperty("range");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(particleNameAttack, new GUIContent("発射時に再生されるパーティクルの名前"));
		EditorGUILayout.PropertyField(particleNameHit, new GUIContent("ヒット時に再生するパーティクルの名前"));
		EditorGUILayout.PropertyField(speed, new GUIContent("弾の速さ"));
		EditorGUILayout.PropertyField(range, new GUIContent("射程"));

		serializedObject.ApplyModifiedProperties();
	}
}