using UnityEngine;
using System.Collections;

public sealed class UnitManager : SingletonMonoBehaviour<UnitManager> {

	public const string UNIT_MODEL_PATH = "Model/Unit/";

	//外部からのnew禁止
	private UnitManager() { }

	/// <summary>
	/// コンテナからキャラクタを生成
	/// </summary>
	/// <param name="unitNum"></param>
	/// <param name="position"></param>
	/// <param name="rot"></param>
	/// <returns></returns>
	public static Unit SpawnUnit(int unitNum, Vector3 position, Quaternion rot) {

		if(unitNum < 0 || unitNum >= UnitDataContainer.data.Count) return null;

		//初期装備を装備
		var unitData = UnitDataContainer.data[unitNum];
		return SpawnUnit(unitNum, position, rot, unitData.weaponNum);
	}

	/// <summary>
	/// コンテナからキャラクタを生成(所持武器変更可)
	/// </summary>
	/// <param name="unitNum"></param>
	/// <param name="group"></param>
	/// <param name="position"></param>
	/// <param name="rot"></param>
	/// <param name="weaponNum"></param>
	/// <returns></returns>
	public static Unit SpawnUnit(int unitNum, Vector3 position, Quaternion rot, int[] weaponNum) {

		if(unitNum < 0 || UnitDataContainer.data.Count <= unitNum) return null;

		var unitData = UnitDataContainer.data[unitNum];

		var unit = (Unit)Instantiate(Resources.Load<GameObject>(UNIT_MODEL_PATH + unitData.modelPath), position, rot)
			.AddComponent(unitData.type);

		unit.name = unitData.name;

		//初期化処理1
		unit.InitFirst();

		//データをセット
		unit.SetInitData(unitData.hp, unitData.dropExp, unitData.nextLevelExp, unitData.moveSpeed, unitData.rotSpeed);

		//武器を装備
		unit.EquipWeapon(WeaponDataContainer.CreateWeapon(weaponNum[0]), 0);
		unit.EquipWeapon(WeaponDataContainer.CreateWeapon(weaponNum[1]), 1);

		//初期化処理2
		unit.InitFinal();

		return unit;
	}
}
