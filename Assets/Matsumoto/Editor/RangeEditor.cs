using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RangeInteger))]
public class RangeIntEditor : RangeEditor {
	protected override void ApplyValue(SerializedProperty min, SerializedProperty max) {
		// 小さい数値を基準にして、大きい数値が小さい数値より小さくならないようにしてみよう。
		if(max.intValue < min.intValue) {
			max.intValue = min.intValue;
		}
	}

} // class RangeIntEditor

[CustomPropertyDrawer(typeof(RangeFloat))]
public class RangeFloatEditor : RangeEditor {
	protected override void ApplyValue(SerializedProperty min, SerializedProperty max) {
		// 小さい数値を基準にして、大きい数値が小さい数値より小さくならないようにしてみよう。
		if(max.floatValue < min.floatValue) {
			max.floatValue = min.floatValue;
		}
	}

} // class RangeFloatEditor

public class RangeEditor : PropertyDrawer {
	private static readonly GUIContent MIN_LABEL = new GUIContent("Min");
	private static readonly GUIContent MAX_LABEL = new GUIContent("Max");

	public override void OnGUI(Rect rect, SerializedProperty prop, GUIContent label) {
		var minProperty = prop.FindPropertyRelative("min");
		var maxProperty = prop.FindPropertyRelative("max");

		ApplyValue(minProperty, maxProperty);

		label = EditorGUI.BeginProperty(rect, label, prop);

		// プロパティの名前部分を描画。
		Rect contentPosition = EditorGUI.PrefixLabel(rect, label);

		// MinとMaxの2つのプロパティを表示するので、残りのフィールドを半分こ。
		contentPosition.width /= 2.0f;

		// Rangeを配列でもっている際は、その分インデントが深くなっている。揃えたいので0に。
		EditorGUI.indentLevel = 0;

		// 3文字なら、これぐらいの幅があればいいんじゃないかな。
		EditorGUIUtility.labelWidth = 30f;


		EditorGUI.PropertyField(contentPosition, minProperty, MIN_LABEL);

		contentPosition.x += contentPosition.width;

		EditorGUI.PropertyField(contentPosition, maxProperty, MAX_LABEL);


		EditorGUI.EndProperty();
	}

	protected virtual void ApplyValue(SerializedProperty min, SerializedProperty max) {

	}

	public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
		return EditorGUIUtility.singleLineHeight;
	}

} // class RangeEditor