using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamepadInput;

public class StartSelect : MonoBehaviour {

	const string NEXT_SCENE_NAME = "Main_Select";
	const float cameraRotationSpeed = 5;

	public Transform cameraParent;
	public Image fadeImage;
	public ControlButton initSelectButton;

    // Use this for initialization
    void Start ()
    {
		var controller = ControlButtonController.CreateController(-1);
		controller.Focus(initSelectButton);

		var titleButton = ControlButton.GetButton(ButtonName.Title_Start);
		titleButton.onFocus += () => {
			AudioManager.PlaySE("button");
		};
		titleButton.onSelect += () => {
			AudioManager.PlaySE("Button_select");
			AudioManager.FadeOut(1);

			//シーン移動
			FadeManager.instance.LoadScene(NEXT_SCENE_NAME, 1);
			ControlButtonController.DestroyController(-1);
		};

		var selectButton = ControlButton.GetButton(ButtonName.Title_Option);
		selectButton.onFocus += () => {
			AudioManager.PlaySE("button");
		};
		selectButton.onSelect += () => {
			AudioManager.PlaySE("Button_select");
			//オプション画面を表示
			UIManager.instance.OptionSwich(true);
			return;
		};

		var exitButton = ControlButton.GetButton(ButtonName.Title_Exit);
		exitButton.onFocus += () => {
			AudioManager.PlaySE("button");
		};
		exitButton.onSelect += () => {
			AudioManager.PlaySE("Button_select");
			//終了
			Application.Quit();
			return;
		};

		//BGM再生
		AudioManager.FadeIn(2, "Title");

		//エフェクト撮影待ち用フェード
		StartCoroutine(FadeIn());
	}

    // Update is called once per frame
    void Update ()
    {
		//カメラの回転
		cameraParent.rotation *= Quaternion.AngleAxis(cameraRotationSpeed * Time.deltaTime, Vector3.up);
	}

    public void OnGoSelect()
    {
        SceneManager.LoadScene("Select");
    }

	IEnumerator FadeIn() {

		//エフェクト撮影用待ち
		yield return new WaitForSeconds(1.5f);

		float t = 0.0f;
		while((t += Time.deltaTime) < 1.0f) {
			fadeImage.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), t);
			yield return null;
		}

		fadeImage.color = new Color(0, 0, 0, 0);
	}
}
