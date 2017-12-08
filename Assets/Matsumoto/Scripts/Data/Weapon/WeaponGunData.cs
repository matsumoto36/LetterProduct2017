using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Create NormalWeapon Data")]
public class WeaponGunData : WeaponRangedData {

	public override Weapon Create() {
		return CreateWeapon<WeaponGun>((item) => {
			item.SetData(interval, power, bulletData);
		});
	}
}