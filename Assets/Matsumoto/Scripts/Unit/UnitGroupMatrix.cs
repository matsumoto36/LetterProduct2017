using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 各グループの敵対状態を定義
/// </summary>
public class UnitGroupMatrix {

	static List<List<bool>> matrix;

	static UnitGroupMatrix() {

		//ここで敵対状態を定義。横で見る。
		matrix = new List<List<bool>> {
			new List<bool> {false, true, true},	//player
			new List<bool> {true, false, true},	//enemy
			new List<bool> {true, true, true},	//other
		};
	}

	/// <summary>
	/// 攻撃が可能なグループを取得
	/// </summary>
	/// <param name="group"></param>
	/// <returns></returns>
	public static UnitGroup[] GetAttackableGroup(UnitGroup group) {
		return GetAttackableGroup(group, false);
	}

	/// <summary>
	/// 攻撃が可能なグループを取得(反転可能)
	/// </summary>
	/// <param name="group"></param>
	/// <param name="isInvert"></param>
	/// <returns></returns>
	public static UnitGroup[] GetAttackableGroup(UnitGroup group, bool isInvert) {

		return matrix[(int)group]
			.Where((item) => isInvert ? !item : item)
			.Select((item, index) => ((UnitGroup)index))
			.ToArray();
	}
}
