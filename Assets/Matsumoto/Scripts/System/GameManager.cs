using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public bool checkFlag;
	public PlayerSpawner spawner;

    public bool useCustomSpawn;
    public bool[] customSpawnFlg;

    // Use this for initialization
    void Start ()
	{

        if (useCustomSpawn) GameData.instance.isEntryPlayer = customSpawnFlg;
        spawner.SpawnPlayer();

        GameData.instance.isSpawnedPlayer = true;

		checkFlag = false;
		UIManager.instance.HPvarSwich(true);//HP表示
	}

	// Update is called once per frame
	void Update()
	{
		
		foreach (var item in GameData.instance.isDeath)
		{
			if (item == false)
			{
				checkFlag = true;
			}
	    }
		if (checkFlag == false)
		{
			UIManager.instance.GameOverSwich(true);
		}
	}
}

