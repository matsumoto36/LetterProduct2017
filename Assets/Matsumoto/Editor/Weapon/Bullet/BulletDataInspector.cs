using UnityEngine;
using UnityEditor;

public abstract class BulletDataInspector : Editor {

	protected SerializedProperty model;

	protected virtual void OnEnable() {

		model = serializedObject.FindProperty("model");
	}
}