using UnityEngine;
using UnityEditor;

public abstract class WeaponDataInspector : Editor {

	protected SerializedProperty model;
	protected SerializedProperty icon;
	protected SerializedProperty interval;
	protected SerializedProperty power;
	protected SerializedProperty mod;

	protected SerializedProperty equipSound;
	protected SerializedProperty useSound;
	protected SerializedProperty hitSound;

	protected virtual void OnEnable() {

		model = serializedObject.FindProperty("model");
		icon = serializedObject.FindProperty("icon");
		interval = serializedObject.FindProperty("interval");
		power = serializedObject.FindProperty("power");
		mod = serializedObject.FindProperty("mod");

		equipSound = serializedObject.FindProperty("equipSound");
		useSound = serializedObject.FindProperty("useSound");
		hitSound = serializedObject.FindProperty("hitSound");
	}
}