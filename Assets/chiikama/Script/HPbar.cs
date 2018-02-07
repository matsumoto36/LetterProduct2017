using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour {

	public UIPlayer[] PlayerUI;
	Player[] playerArray;

	public void Init() {

		GetPlayerArray();

		for(int i = 0;i < 4;i++) {
			PlayerUI[i].gameObject.SetActive(playerArray[i]);
		}
	}

	void GetPlayerArray() {

		playerArray = new Player[4];

		var playerList = Unit.unitList
			.Where(item => item)
			.Where(item => item is Player)
			.Select(item => (Player)item)
			.ToArray();

		foreach(var item in playerList) {
			playerArray[item.playerIndex] = item;
		}
	}

	void Update() {
		for(int i = 0;i < 4;i++) {
			if(playerArray[i] == false) continue;

			var ratio = playerArray[i].HPRatio;
			PlayerUI[i].SwitchWarningAnim(ratio < GameBalance.instance.data.duraEggCanUseRatio);

			PlayerUI[i].SetHPRatio(ratio);
			PlayerUI[i].SetEXPRatio(playerArray[i].EXPRatio);
			PlayerUI[i].SetLevel(playerArray[i].level);

			var weaponList = playerArray[i].equipWeapon;
			PlayerUI[i].SetWeaponIcon(weaponList[0].icon, weaponList[1].icon);
		}
	}

	public void OnHide() {

		foreach(var item in PlayerUI) {
			item.SwitchWarningAnim(false);
		}
	}
}