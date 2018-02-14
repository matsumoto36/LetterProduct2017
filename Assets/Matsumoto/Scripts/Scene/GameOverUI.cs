using UnityEngine;
using System.Collections;

public class GameOverUI : MonoBehaviour {

	public ControlButton retryButton;
	public ControlButton selectButton;

	void Awake() {

		retryButton.onFocus += () => {
			AudioManager.PlaySE("button");
		};
		retryButton.onSelect += () => {
			AudioManager.PlaySE("Button_select");

			GameManager.Retry();
			ControlButtonController.DestroyController(-1);
		};

		selectButton.onFocus += () => {
			AudioManager.PlaySE("button");
		};
		selectButton.onSelect += () => {
			AudioManager.PlaySE("Button_select");

			//シーン移動
			GameManager.GoToSelectScene();
			ControlButtonController.DestroyController(-1);
		};
	}

	public void OnActive() {
		var controller = ControlButtonController.CreateController(-1);
		controller.Focus(retryButton);
	}

	public void OnHide() {
		ControlButtonController.DestroyController(-1);
	}
}
