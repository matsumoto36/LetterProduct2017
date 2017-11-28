using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/// <summary>
/// 弾のデータを格納しておく
/// </summary>
public class UnitDataContainer : SingletonMonoBehaviour<UnitDataContainer> {

	const string UNIT_DATA_PATH = "Data/UnitData";

	public static List<UnitData> data {
		get { return instance.unitDataList; }
	}

	List<UnitData> unitDataList;

	//外部からのnew禁止
	private UnitDataContainer() { }

	public static void Load() {

		//CSVから読み込む
		instance.unitDataList = CSVLoader.LoadData(UNIT_DATA_PATH)
			.Select((item) => new {
				name = item[0],
				modelPath = item[1],
				classType = item[2],
				hp = int.Parse(item[3]),
				nextLevelExp = int.Parse(item[4]),
				moveSpeed = float.Parse(item[5]),
				rotSpeed = float.Parse(item[6]),
				weaponNum = new int[2] { int.Parse(item[7]), int.Parse(item[8]) },
				ex1 = item[9]
			})
			.Select((item) => {
				var data = new UnitData(
						item.name,
						item.modelPath,
						item.hp,
						item.nextLevelExp,
						item.moveSpeed,
						item.rotSpeed,
						item.weaponNum
					);

				switch(item.classType) {
					case "Player":
						data.SetUnitType<Player>();
						break;
					case "Enemy":
						data.SetUnitType<Enemy>();
						break;
					default:
						return null;
				}

				return data;
			})
			.ToList();
	}
}
