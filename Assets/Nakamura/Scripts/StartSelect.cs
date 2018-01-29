using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamepadInput;

public class StartSelect : MonoBehaviour {

	const string NEXT_SCENE_NAME = "Main_Select";
	public ControlButton initSelectButton;

	public  int playercnt = 0;
    public bool flg = false;

    public Button startButton;

    public Text player1Ready;
    public Text player2Ready;
    public Text player3Ready;
    public Text player4Ready;

    public int playerIndex;

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
