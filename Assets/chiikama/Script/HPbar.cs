using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour {

	public UIPlayer[] PlayerUI;
	Player[] playerArray;

	public GameObject target;
	public Image image;
	public Vector2 vec;

	public void Init() {

		GetPlayerArray();

		for(int i = 0;i < 4;i++) {
			PlayerUI[i].gameObject.SetActive(playerArray[i]);
			PlayerUI[i].Init();
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

		if(target) {
			vec = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
			image.rectTransform.position = vec;
		}


		List<Vector2> screenPos = new List<Vector2>();
		foreach(var item in Unit.unitList) {
			if(item == false) continue;
			screenPos.Add(RectTransformUtility.WorldToScreenPoint(Camera.main, item.transform.position));
		}

		for(int i = 0;i < 4;i++) {
			if(playerArray[i] == false) continue;

			//ステータスの表示
			var ratio = playerArray[i].HPRatio;
			PlayerUI[i].SwitchWarningAnim(ratio < GameBalance.instance.data.duraEggCanUseRatio);

			PlayerUI[i].SetHPRatio(ratio);
			PlayerUI[i].SetEXPRatio(playerArray[i].EXPRatio);
			PlayerUI[i].SetLevel(playerArray[i].level);

			//武器の表示
			var weaponList = playerArray[i].equipWeapon;
			PlayerUI[i].SetWeaponIcon(weaponList[0].icon, weaponList[1].icon);

			//プレイヤーが重なりそうだったら半透明になる
			float alpha = 1;
			foreach(var item in screenPos) {
				if(PlayerUI[i].CheckInsidePosition(item)) {
					alpha = 0.25f;
				}
			}

			PlayerUI[i].Alpha = alpha;

		}
	}

	public void OnHide() {

		foreach(var item in PlayerUI) {
			item.SwitchWarningAnim(false);
		}
	}
}