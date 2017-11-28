using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 武器の定義済みデータが入っている
/// </summary>
public sealed class WeaponDataContainer : SingletonMonoBehaviour<WeaponDataContainer> {

	const string WEAPON_DATA_PATH = "Data/WeaponData";

	List<WeaponData> weaponDataList;
	
	//外部からのnew禁止
	private WeaponDataContainer() { }

	/// <summary>
	/// 武器のデータをロードする
	/// </summary>
	public static void Load() {

		//弾のデータをロードする
		BulletDataContainter.Load();

		//CSVデータから武器リストに変換
		instance.weaponDataList = CSVLoader.LoadData(WEAPON_DATA_PATH)
			.Select(item => new {
				name = item[0],
				modelName = item[1],
				weaponType = item[2],
				interval = float.Parse(item[3]),
				power = int.Parse(item[4]),
				mod = new StatusModifier(float.Parse(item[5]), float.Parse(item[6]), float.Parse(item[7]), float.Parse(item[8])),
				exData1 = item[9],
				exData2 = item[10]
			})
			.Select((item) => {

				var weaponData = new WeaponData(item.name, item.modelName, item.mod);

				switch(item.weaponType) {
					case "WeaponGun":
						return weaponData.SetWeaponSetData<WeaponGun>(
							(weapon) => {
								((WeaponGun)weapon).SetData(item.interval, item.power, new BulletData(BulletDataContainter.data[int.Parse(item.exData1)]));
							});
					case "WeaponLaser":
						return weaponData.SetWeaponSetData<WeaponLaser>(
							(weapon) => {
								//弾のデータを生成
								((WeaponLaser)weapon).SetData(item.interval, item.power, new BulletData(BulletDataContainter.data[int.Parse(item.exData1)]));
							});
					case "WeaponMelee":
						return weaponData.SetWeaponSetData<WeaponMelee>(
							(weapon) => {
								((WeaponMelee)weapon).SetData(item.interval, item.power, item.exData1, int.Parse(item.exData2));
							});
					default:
						return null;
				}
			})
			.Where((item) => item != null)
			.ToList();
	}

	/// <summary>
	/// 武器の生成
	/// </summary>
	/// <param name="num"></param>
	/// <returns></returns>
	public static Weapon CreateWeapon(int num) {
		if(num < 0 || num >= instance.weaponDataList.Count) return null;
		return instance.weaponDataList[num].CreateWeapon();
	}
}
