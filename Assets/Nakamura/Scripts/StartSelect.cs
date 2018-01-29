using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamepadInput;

public class StartSelect : MonoBehaviour {

	const string NEXT_SCENE_NAME = "Main_Select";
	public ControlButton initSelectButton;

    // Use this for initialization
    void Start ()
    {
		var controller = ControlButtonController.CreateController(-1);
		controller.Focus(initSelectButton);

		ControlButton.GetButton(ButtonName.Title_Start).onSelect += () => {
			//シーン移動
			FadeManager.Instance.LoadScene(NEXT_SCENE_NAME, 1);
			ControlButtonController.DestroyController(-1);
		};
		ControlButton.GetButton(ButtonName.Option).onSelect += () => {
			//オプション画面を表示
			return;
		};
	}

    // Update is called once per frame
    void Update ()
    {
       
            //if (InputManager.GetButtonDown(playerIndex, GamePad.Button.A) )
            //{
            //    OnGoSelect();
            //}
      
       
    }

    public void OnGoSelect()
    {
        SceneManager.LoadScene("Select");
    }
}
