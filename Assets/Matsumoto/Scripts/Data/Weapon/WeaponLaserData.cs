using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Create LaserWeapon Data")]
public class WeaponLaserData : WeaponRangedData {

	public override Weapon Create() {
		return CreateWeapon<WeaponLaser>((item) => {
			item.SetData(interval, power, bulletData);
		});
	}
}