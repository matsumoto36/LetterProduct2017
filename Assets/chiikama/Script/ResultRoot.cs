using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultRoot : MonoBehaviour {

	public Text scoreText;
	public RectTransform[] playerIconText;
	public Text[] totalDamage;

	public ControlButton retryButton;
	public ControlButton titleButton;

	int score;

	void Awake() {

		retryButton.onFocus += () => {
			AudioManager.PlaySE("button");
		};
		retryButton.onSelect += () => {
			AudioManager.PlaySE("Button_select");

			GameManager.Retry();
			ControlButtonController.DestroyController(-1);
		};

		titleButton.onFocus += () => {
			AudioManager.PlaySE("button");
		};
		titleButton.onSelect += () => {
			AudioManager.PlaySE("Button_select");

			//シーン移動
			GameManager.GoToTitleScene();
			ControlButtonController.DestroyController(-1);
		};


	}

	public void OnActive() {
		var controller = ControlButtonController.CreateController(-1);
		controller.Focus(retryButton);

		//スコアの計算
		score = GameBalance.CalcScore(
			GameData.instance.maxCombo,
			GameData.instance.clearTime - GameData.instance.startTime,
			GameData.instance.clearTime - GameData.instance.bossStartTime);

		//スコアの表示
		StartCoroutine(NumberMoveAnim(5, 0, score, (value) => {
			scoreText.text = ((int)value).ToString();
		}));

		//総ダメージの表示
		for(int i = 0;i < GameData.MAX_PLAYER_NUM;i++) {

			if(!GameData.instance.isEntryPlayer[i]) {
				playerIconText[i].gameObject.SetActive(false);
				continue;
			}

			//ラムダ式では遅延評価されるため
			var num = i;
			StartCoroutine(NumberMoveAnim(5, 0, GameData.instance.sumDamage[num], (value) => {
				totalDamage[num].text = ((int)value).ToString();
			}));
		}

	}

	public void OnHide() {
		ControlButtonController.DestroyController(-1);
	}

	/// <summary>
	/// ババっと上がる
	/// </summary>
	/// <param name="Duration"></param>
	/// <param name="start"></param>
	/// <param name="end"></param>
	/// <returns></returns>
	IEnumerator NumberMoveAnim(float Duration, float start, float end, Action<float> execute) {

		float t = 0.0f;
		while((t += Time.deltaTime / Duration) < 1.0f) {
			execute(Mathf.Lerp(start, end, t));
			yield return null;
		}

		execute(end);
	}
}
