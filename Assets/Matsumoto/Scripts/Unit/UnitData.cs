using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// キャラクターの情報を保存しておく
/// </summary>
public class UnitData {

	public Type type { get; private set; }
	public string name { get; private set; }
	public string modelPath { get; private set; }
	public int hp { get; private set; }
	public int dropExp { get; private set; }
	public int nextLevelExp { get; private set; }
	public float moveSpeed { get; private set; }
	public float rotSpeed { get; private set; }
	public int[] weaponNum { get; private set; }

	public UnitData(string name, string modelPath, int hp, int dropExp, int nextLevelExp, float moveSpeed, float rotSpeed, int[] weaponNum) {
		this.name = name;
		this.modelPath = modelPath;
		this.hp = hp;
		this.dropExp = dropExp;
		this.nextLevelExp = nextLevelExp;
		this.moveSpeed = moveSpeed;
		this.rotSpeed = rotSpeed;
		this.weaponNum = weaponNum;
	}

	public void SetUnitType<T>() where T : Unit {
		type = typeof(T);
	}
}
