using UnityEngine;
using System.Collections;
using System.Linq;

public class door : MonoBehaviour
{
	public GameObject thedoor;
	public bool isOpen;

	public int enemyCountView;
	public int enemySpawnCountView;

	void Update()
	{
		if (isOpen) return;

        int enemyCount = enemyCountView = Unit.unitList
                .Where(unit => unit.group != UnitGroup.Player)
                .Count();

		int enemySpawnCount = enemySpawnCountView = EnemySpawner.spawnerList
			.Count();

        if (enemyCount + enemySpawnCount <= 0)
        {
			isOpen = true;
            thedoor.GetComponent<Animation>().Play("open");
        }
    }
}
