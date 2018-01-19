using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(BulletGrenadeData))]
public class BulletGrenadeDataInspector : BulletNormalDataInspector {

	SerializedProperty expRadius;
	SerializedProperty expPow;

	protected override void OnEnable() {
		base.OnEnable();

		expRadius = serializedObject.FindProperty("expRadius");
		expPow = serializedObject.FindProperty("expPow");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(particleNameAttack, new GUIContent("発射時に再生されるパーティクルの名前"));
		EditorGUILayout.PropertyField(particleNameHit, new GUIContent("爆発時に再生するパーティクルの名前"));
		EditorGUILayout.PropertyField(speed, new GUIContent("弾の速さ"));
		EditorGUILayout.PropertyField(expRadius, new GUIContent("爆発の半径"));
		EditorGUILayout.PropertyField(expPow, new GUIContent("爆発の威力"));

		serializedObject.ApplyModifiedProperties();
	}
}