using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{

	public GameObject[] PlayerUI;
	public Image[] warningImage;
	public Image[] HPBarArray;
	public Image[] ExpBarArray;
	public Text[] LevelText;

	Player[] playerArray;
	Coroutine[] warningAnim;

	public void Init()
	{
		playerArray = new Player[4];
		warningAnim = new Coroutine[4];

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
			if (playerArray[i] == false) continue;

			var ratio = playerArray[i].HPRatio;
			if(ratio < GameBalance.instance.data.duraEggCanUseRatio) {
				if(warningAnim[i] == null) warningAnim[i] = StartCoroutine(WarningAnim(warningImage[i]));
			}
			else {
				if(warningAnim[i] != null) {
					StopCoroutine(warningAnim[i]);
					warningImage[i].color = new Color(1, 1, 1, 0);
				}
			}

			HPBarArray[i].fillAmount = playerArray[i].HPRatio;
			ExpBarArray[i].fillAmount = playerArray[i].EXPRatio;
			LevelText[i].text = playerArray[i].level.ToString();
		}
	}

	public void OnHide() {

		StopAllCoroutines();

		foreach(var item in warningImage) {
			item.color = new Color(1, 1, 1, 0);
		}
	}

	IEnumerator WarningAnim(Image image) {

		var t = 0.0f;
		while(true) {

			t += Time.deltaTime;
			var col = image.color;
			col.a = Mathf.Abs(Mathf.Sin(t * 8));
			image.color = col;

			yield return null;
		}

	}
}
