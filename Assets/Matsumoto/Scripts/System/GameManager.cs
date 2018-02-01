using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public bool checkFlag;

	// Use this for initialization
	void Start ()
	{
		checkFlag = false;
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
