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

		//重いので呼ぶタイミングを制限する
		if(Time.frameCount % 2 == 0) UpdateUI();

	}

	void UpdateUI() {

		if(target) {
			vec = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);
			image.rectTransform.position = vec;
		}

		//UIに重なっているか調べるためにあらかじめ計算しておく
		var screenPos = Unit.unitList
			.Where(item => item)
			.Where(item => item is Player || (item.GetComponent<EnemyAI>() && item.GetComponent<EnemyAI>().LookPlayer()))
			.Select(item => RectTransformUtility.WorldToScreenPoint(Camera.main, item.transform.position))
			.ToList();

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

			//重なりそうだったら半透明になる
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