using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// 武器のデータを保持しておく
/// </summary>
public class WeaponData {

	Type type;
	public string modelPath { get; private set; }
	public object setDataMethod { get; private set; }
	public StatusModifier mod { get; private set; }

	string name;

	public WeaponData(string name, string modelPath, StatusModifier mod) {
		this.name = name;
		this.modelPath = modelPath;
		this.mod = mod;
	}

	/// <summary>
	/// 武器のデータをセットする
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="setDataMethod"></param>
	/// <returns></returns>
	public WeaponData SetWeaponSetData<T>(Action<Weapon> setDataMethod) where T : Weapon {
		type = typeof(T);
		this.setDataMethod = setDataMethod;
		return this;
	}

	/// <summary>
	/// 武器を生成
	/// </summary>
	/// <returns></returns>
	public Weapon CreateWeapon() {

		//武器の生成
		var weapon = (Weapon)(UnityEngine.Object.Instantiate(Resources.Load<GameObject>(modelPath)).AddComponent(type));
		weapon.name = name;
		//武器のデータ設定
		((Action<Weapon>)setDataMethod)(weapon);
		weapon.weaponMod = mod;
		return weapon;
	}
}