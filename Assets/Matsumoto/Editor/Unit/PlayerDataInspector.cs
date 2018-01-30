using UnityEngine;
using UnityEditor;

[CanEditMultipleObjects, CustomEditor(typeof(PlayerData))]
public class PlayerDataInspector : UnitDataInspector {

	public override void OnInspectorGUI() {

		serializedObject.Update();

		EditorGUILayout.PropertyField(model, new GUIContent("モデルデータ"));
		EditorGUILayout.PropertyField(hp, new GUIContent("体力"));
		EditorGUILayout.PropertyField(dropExp, new GUIContent("落とす経験値量(総量)"));
		EditorGUILayout.PropertyField(nextLevelExp, new GUIContent("レベル1から2にあげるための経験値量"));
		EditorGUILayout.PropertyField(moveSpeed, new GUIContent("移動速度"));
		EditorGUILayout.PropertyField(rotSpeed, new GUIContent("回転速度"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(weaponData.GetArrayElementAtIndex(0), new GUIContent("メイン武器"));
		EditorGUILayout.PropertyField(weaponData.GetArrayElementAtIndex(1), new GUIContent("サブ武器"));
		EditorGUILayout.Separator();
		EditorGUILayout.PropertyField(deathSE, new GUIContent("死んだときのSE"));
		EditorGUILayout.PropertyField(deathParticle, new GUIContent("死んだときのパーティクル"));

		serializedObject.ApplyModifiedProperties();
	}
}