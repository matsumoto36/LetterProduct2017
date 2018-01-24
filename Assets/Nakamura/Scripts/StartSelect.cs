using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamepadInput;

public class StartSelect : MonoBehaviour {
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
		//GetComponent<Button>().Select();

		ControlButton.GetButton(ButtonName.Title_Start).onSelect += () => {
			//シーン移動
			FadeManager.Instance.LoadScene("Main_Select", 2);
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
