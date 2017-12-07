using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Create LaserWeapon Data")]
public class WeaponLaserData : WeaponRangedData {

	public override Weapon Create() {
		return CreateWeapon<WeaponLaser>((item) => {
			item.SetData(interval, power, bulletData);
		});
	}

	[MenuItem("Weapon/Create LaserWeapon Data")]
	static void CreateData() {
		CreateAsset<WeaponLaserData>(ASSET_PATH);
	}
}