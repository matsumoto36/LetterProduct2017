using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 爆発を起こすクラス
/// </summary>
public class Explosion : MonoBehaviour {

	public static void Create(Weapon owner, Vector3 position, int power, float radius) {

		var attackableUnit = Physics.OverlapSphere(position, radius)
			.Select((item) => item.GetComponent<Unit>())
			.Where((item) => item && owner.CheckHit(item.gameObject));

		foreach(var item in attackableUnit) {
			Unit.Attack(owner.unitOwner, item, power);
		}

		//SEの再生
		AudioManager.PlaySE("grenade_Launcher_explosion");

	}
}
