using UnityEngine;
using System.Collections;
using System.Linq;

public class door : MonoBehaviour
{
	public GameObject thedoor;
	public bool isOpen;


	void Update()
	{

		if (isOpen) return;

        int enemyCount = Unit.unitList
                .Where(unit => unit.group != UnitGroup.Player)
                .Count();

		int enemySpawnCount = EnemySpawner.spawnerList
			.Count();

        if (enemyCount + enemySpawnCount <= 0)
        {
			isOpen = true;
            thedoor.GetComponent<Animation>().Play("open");
        }
    }
}
