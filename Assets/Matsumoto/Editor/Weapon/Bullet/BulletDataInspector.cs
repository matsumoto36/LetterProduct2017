using UnityEngine;
using UnityEditor;

public abstract class BulletDataInspector : Editor {

	protected SerializedProperty model;
	protected SerializedProperty particleNameAttack;
	protected SerializedProperty particleNameHit;

	protected virtual void OnEnable() {

		model = serializedObject.FindProperty("model");
		particleNameAttack = serializedObject.FindProperty("particleNameAttack");
		particleNameHit = serializedObject.FindProperty("particleNameHit");
	}
}