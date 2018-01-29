using UnityEngine;
using System.Collections;
using System.Linq;

public class door : MonoBehaviour
{
	GameObject thedoor;

	void Update()
	{
		int enemyCount = Unit.unitList
			.Where(unit => unit.group != UnitGroup.Player)
			.Count();
		if (enemyCount <= 0)
		{
			thedoor.GetComponent<Animation>().Play("open");
		}
	}
}
