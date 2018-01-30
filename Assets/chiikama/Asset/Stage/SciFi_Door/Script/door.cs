using UnityEngine;
using System.Collections;
using System.Linq;

public class door : MonoBehaviour
{
	public GameObject thedoor;
	public bool isOpen;


	void Update()
	{
<<<<<<< HEAD

		if (isOpen) return;
=======
        AudioManager.FadeIn(2, "stage_1");
>>>>>>> 2dcabc76d57a85becc21014c65d5a9b4b43b2b3e

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
