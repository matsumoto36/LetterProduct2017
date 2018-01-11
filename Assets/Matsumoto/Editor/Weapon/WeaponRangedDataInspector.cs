using UnityEngine;
using UnityEditor;

public abstract class WeaponRangedDataInspector : WeaponDataInspector {

	protected SerializedProperty bulletData;

	protected override void OnEnable() {
		base.OnEnable();

		bulletData = serializedObject.FindProperty("bulletData");
	}
}