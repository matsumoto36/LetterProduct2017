using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// キャラクターの情報を保存しておく
/// </summary>
public abstract class UnitData : ScriptableObjectBase {

	public const string ASSET_PATH = "Assets/Resources/Data/Unit";

	public GameObject model;
	public int hp;
	public int dropExp;
	public int nextLevelExp;
	public float moveSpeed;
	public float rotSpeed;
	public WeaponData[] weaponData;
	public bool isDrawWeapon = true;

	public UnitData() {
		weaponData = new WeaponData[2];
	}

	/// <summary>
	/// スポーン処理
	/// </summary>
	public abstract Unit Spawn(Vector3 position, Quaternion rot);

	/// <summary>
	/// 生成する
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="position"></param>
	/// <param name="rot"></param>
	/// <returns></returns>
	protected T SpawnUnit<T>(Vector3 position, Quaternion rot) where T : Unit{

		var unit = Instantiate(model, position, rot)
			.AddComponent<T>();

		unit.name = name;

		//初期化処理1
		unit.InitFirst();

		//データをセット
		unit.SetInitData(hp, dropExp, nextLevelExp, moveSpeed, rotSpeed);

		//武器を装備
		if(weaponData[0]) {
			var weapon = weaponData[0].Create();

			//表示しない場合は描画を無効化
			if(!isDrawWeapon)
			foreach(var item in weapon.GetComponentsInChildren<Renderer>()) {
					item.enabled = false;
			}

			unit.EquipWeapon(weapon, 0);
		}
		if(weaponData[1]) {
			var weapon = weaponData[1].Create();

			//表示しない場合は描画を無効化
			if(!isDrawWeapon)
				foreach(var item in weapon.GetComponentsInChildren<Renderer>()) {
					item.enabled = false;
				}

			unit.EquipWeapon(weapon, 1);
		}

		//初期化処理2
		unit.InitFinal();

		return unit;
	}
}
