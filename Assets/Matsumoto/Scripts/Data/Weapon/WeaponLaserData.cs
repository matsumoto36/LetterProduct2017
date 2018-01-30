using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Create LaserWeapon Data")]
public class WeaponLaserData : WeaponRangedData {

	public float shootingMoveMul;
	public string chargeSound;

	public override Weapon Create() {
		return CreateWeapon<WeaponLaser>((item) => {
			item.chargeSound = chargeSound;
			item.shootingMoveMul = shootingMoveMul;
			item.SetData(interval, power, bulletData);
		});
	}
}