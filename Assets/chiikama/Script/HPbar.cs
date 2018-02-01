using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{

	public GameObject[] PlayerUI;
	public Image[] HPBarArray;
	public Image[] ExpBarArray;
	public Text[] LevelText;

	Player[] playerArray;

	public void Init()
	{
		GetPlayerArray();

		for (int i = 0; i < 4; i++)
		{
			PlayerUI[i].SetActive(playerArray[i]);
		}
	}

	void GetPlayerArray()
	{
		var playerList = Unit.unitList
			.Where(item => item)
			.Where(item => item is Player)
			.Select(item => (Player)item)
			.ToArray();

		foreach (var item in playerList)
		{
			playerArray[item.playerIndex] = item;
		}
	}
	void Update()
	{
		for (int i = 0; i < 4; i++)
		{
			HPBarArray[i].fillAmount = playerArray[i].HPRatio;
		//	ExpBarArray[i].fillAmount = playerArray[i].;
			LevelText[i].text = playerArray[i].level.ToString();
		}
	}
}
