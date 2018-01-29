using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(WeaponMeleeData))]
public class WeaponMeleeDataInspector : WeaponDataInspector {

	SerializedProperty particleName;
	SerializedProperty motionName;
	SerializedProperty motionSpeed;

	protected override void OnEnable() {
		base.OnEnable();

		particleName = serializedObject.FindProperty("particleName");
		motionName = serializedObject.FindProperty("motionName");
		motionSpeed = serializedObject.FindProperty("motionSpeed");
	}

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(icon, new GUIContent("アイコン"));
		EditorGUILayout.PropertyField(interval, new GUIContent("次の攻撃までの待ち時間"));
		EditorGUILayout.PropertyField(power, new GUIContent("1ヒット時のダメージ"));
		EditorGUILayout.PropertyField(mod, new GUIContent("パッシブ効果"), true);
		EditorGUILayout.PropertyField(particleName, new GUIContent("攻撃時に再生するエフェクト"));
		//EditorGUILayout.PropertyField(motionName, new GUIContent("再生するアニメーション"));
		EditorGUILayout.PropertyField(motionSpeed, new GUIContent("再生するアニメーションの再生速度"));

		serializedObject.ApplyModifiedProperties();
	}
}