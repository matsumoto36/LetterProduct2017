using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Create LaserWeapon Data")]
public class WeaponLaserData : WeaponRangedData {

	public string chargeSound;

	public override Weapon Create() {
		return CreateWeapon<WeaponLaser>((item) => {
			item.chargeSound = chargeSound;
			item.SetData(interval, power, bulletData);
		});
	}
}