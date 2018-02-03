using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが全員集まると遷移する
/// </summary>
public class NextSceneArea : MonoBehaviour {

	public GameManager manager;

	bool[] enteringPlayer;

	void Start() {
		enteringPlayer = new bool[GameData.MAX_PLAYER_NUM];
	}

	bool CheckSceneMovable() {

		Debug.Log("Flg");

		var flg = true;
		for(int i = 0;i < enteringPlayer.Length;i++) {

			if(!GameData.instance.isEntryPlayer[i]) continue;

			flg &= !GameData.instance.isDeath[i]        //死んでいないか
				&& enteringPlayer[i];					//このエリアに入っているか
		}

		return flg;
	}

	void OnTriggerEnter(Collider other) {

		Player player;
		if(player = other.GetComponent<Player>()) {

			enteringPlayer[player.playerIndex] = true;
			if(CheckSceneMovable()) {
				FadeManager.instance.LoadScene(manager.nextSceneName, 1);
			}
		}
	}

	void OnTriggerExit(Collider other) {

		Player player;
		if(player = other.GetComponent<Player>()) {

			enteringPlayer[player.playerIndex] = false;
		}
	}

}
