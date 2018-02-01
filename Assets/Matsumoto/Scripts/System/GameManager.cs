using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public bool checkFlag;
	public PlayerSpawner spawner;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < InputManager.MAX_PAYER_NUM; i++)
		{
			if (GameData.instance.isEntryPlayer[i]) spawner.SpawnPlayer(i);
		}
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

