using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Option : MonoBehaviour {

	const float VOLUME_CHANGE_VALUE = 10;

	public BGM BGMSlider;
	public SE SESlider;

	public ControlButton retryButton;
	public ControlButton backButton;

	ControlButton saveSelectedButton;

	void Awake() {

		BGMSlider.Init();
		SESlider.Init();

		retryButton.onSelect += () => {
			GameManager.instance.Retry();
		};

		backButton.onSelect += () => {
			UIManager.instance.OptionSwich(false);
		};

	}

	// Use this for initialization
	public void OnActive () {

		//ゲーム中でないときのオプション表示はリトライを表示しない
		retryButton.gameObject.SetActive(GameManager.instance.nowGamePlay);

		var controller = ControlButtonController.CreateController(-1);

		if(!controller) {
			controller = ControlButtonController.GetController(-1);

			//前の選択を保持しておく
			saveSelectedButton = controller.focusButton;
		}
		else {
			saveSelectedButton = null;
		}

		controller.Focus(BGMSlider.button);
	}
	
	public void OnHide() {

		//前に戻るボタンがある = コントローラーが既に存在していた
		if(saveSelectedButton) {
			var controller = ControlButtonController.GetController(-1);
			controller.Focus(saveSelectedButton);
		}
		else {
			ControlButtonController.DestroyController(-1);
		}
	}
}
