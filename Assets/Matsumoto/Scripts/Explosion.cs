using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 爆発を起こすクラス
/// </summary>
public class Explosion : MonoBehaviour {

	const string PREFAB_PATH = "System/Explosion";
	const float EXPLOSION_TIME = 0.1f;

	Weapon owner;
	int power;

	void Awake() {
		Destroy(gameObject, EXPLOSION_TIME);
	}

	public static void Create(Weapon owner, Vector3 position, int power, float radius) {

		var attackableUnit = Physics.OverlapSphere(position, radius)
			.Select((item) => item.GetComponent<Unit>())
			.Where((item) => item && owner.CheckHit(item.gameObject));

		foreach(var item in attackableUnit) {
			Unit.Attack(owner.unitOwner, item, power);
		}

		var exp = Instantiate(Resources.Load<Explosion>(PREFAB_PATH), position, Quaternion.identity);
		exp.transform.localScale = new Vector3(1, 1, 1) * radius * 2;
		exp.owner = owner;
		exp.power = power;
	}
}
