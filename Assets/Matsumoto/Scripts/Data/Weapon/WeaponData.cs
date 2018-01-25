using UnityEngine;
using System;

/// <summary>
/// 武器のデータを保持しておく
/// </summary>
public abstract class WeaponData : ScriptableObjectBase {

	public const string ASSET_PATH = "Assets/Resources/Data/Weapon";

	public GameObject model;
	public Sprite icon;

	public float interval;
	public int power;
	public StatusModifier mod;

	/// <summary>
	/// 武器を生成
	/// </summary>
	/// <returns></returns>
	public abstract Weapon Create();

	/// <summary>
	/// 武器を生成
	/// </summary>
	/// <returns></returns>
	protected T CreateWeapon<T>(Action<T> setDataMethod) where T : Weapon {

		var weapon = Instantiate(model).AddComponent<T>();
		weapon.name = name;
		weapon.icon = icon;
		weapon.weaponMod = mod;
		
		setDataMethod(weapon);

		return weapon;
	}
}