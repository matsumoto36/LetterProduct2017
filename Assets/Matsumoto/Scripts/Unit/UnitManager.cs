using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// ドローコール対策
/// </summary>
public sealed class UnitManager : SingletonMonoBehaviour<UnitManager> {

	const string UNIT_PARENT = "[Unit]";

	Transform unitParent;

	//外部からのnew禁止
	private UnitManager() { }

	public static void AddParent (Unit unit) {
		var parent = instance.unitParent ? instance.unitParent : GameObject.Find(UNIT_PARENT).transform;

		unit.transform.SetParent(parent);

		// 子階層のGameObject取得
		var list = new List<Transform>();
		foreach(Transform item in parent.transform) {
			list.Add(item);
		}

		// オブジェクトを名前で昇順ソート
		list.Sort((obj1, obj2) => string.Compare(obj1.name, obj2.name));

		// ソート結果順にGameObjectの順序を反映
		foreach(var obj in list) {
			obj.SetSiblingIndex(list.Count - 1);
		}
	}
}
