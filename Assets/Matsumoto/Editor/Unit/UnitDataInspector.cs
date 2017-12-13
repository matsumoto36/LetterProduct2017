using UnityEngine;
using UnityEditor;

public abstract class UnitDataInspector : Editor {

	protected SerializedProperty model;
	protected SerializedProperty hp;
	protected SerializedProperty dropExp;
	protected SerializedProperty nextLevelExp;
	protected SerializedProperty moveSpeed;
	protected SerializedProperty rotSpeed;
	 
	protected SerializedProperty weaponData;

	protected virtual void OnEnable() {

		model = serializedObject.FindProperty("model");
		hp = serializedObject.FindProperty("hp");
		dropExp = serializedObject.FindProperty("dropExp");
		nextLevelExp = serializedObject.FindProperty("nextLevelExp");
		moveSpeed = serializedObject.FindProperty("moveSpeed");
		rotSpeed = serializedObject.FindProperty("rotSpeed");

		weaponData = serializedObject.FindProperty("weaponData");
	}
}