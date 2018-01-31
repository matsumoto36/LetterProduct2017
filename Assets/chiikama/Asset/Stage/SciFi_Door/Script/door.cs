using UnityEngine;
using System.Collections;
using System.Linq;

public class door : MonoBehaviour
{
	public GameObject thedoor;



	void Start()
	{
        AudioManager.FadeIn(2, "stage_1");

        int enemyCount = Unit.unitList
                .Where(unit => unit.group != UnitGroup.Player)
                .Count();
        if (enemyCount <= 0)
        {
            thedoor.GetComponent<Animation>().Play("open");
        }
    }
}
