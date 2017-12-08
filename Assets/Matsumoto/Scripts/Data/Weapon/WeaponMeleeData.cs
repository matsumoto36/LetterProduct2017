using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Create MeleeWeapon Data")]
public class WeaponMeleeData : WeaponData {

	public string motionName;
	public float motionSpeed = 1;

	/// <summary>
	/// 武器を生成
	/// </summary>
	/// <returns></returns>
	public override Weapon Create() {
		return CreateWeapon<WeaponMelee>((item) => {
			item.SetData(interval, power, motionName, motionSpeed);
		});
	}
}