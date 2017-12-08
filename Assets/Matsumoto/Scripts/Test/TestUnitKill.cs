using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUnitKill : MonoBehaviour {

	public Unit attacker;
	public Unit target;

	void Update() {
		if(Input.GetKeyDown(KeyCode.B))
			Unit.Attack(attacker, target, target.nowHP);
	}
}
