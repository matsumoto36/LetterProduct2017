using UnityEngine;
using UnityEditor;

public abstract class BulletDataInspector : Editor {

	protected SerializedProperty model;
	protected SerializedProperty particleName;

	protected virtual void OnEnable() {

		model = serializedObject.FindProperty("model");
		particleName = serializedObject.FindProperty("particleName");
	}
}