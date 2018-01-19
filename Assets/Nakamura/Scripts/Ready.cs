using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GamepadInput;
public class Ready : MonoBehaviour {
    public SpliteCamera splitecamera;

    public Canvas readyPanel,GameUI;
    public Dropdown weponDropdown;
    public Text readytext, weponchoice;
    public Button gameStart;

    bool pushbutton = false;
    public bool flg = false;

    public int playerIndex;
    // Use this for initialization
    void Start ()
    {        
        //Readycanvasのみ表示
        readyPanel.gameObject.SetActive(true);
        GameUI.gameObject.SetActive(false);
        weponchoice.gameObject.SetActive(false);
        weponDropdown.gameObject.SetActive(false);
        gameStart.gameObject.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (pushbutton == false)
        {
            //Ready画面の操作
            if (InputManager.GetButtonDown(playerIndex, GamePad.Button.A) || Input.GetKeyDown(KeyCode.Space) )
            {
                readytext.text = "OK";
                flg = true;
                splitecamera.playercnt++;
                pushbutton = true;
            }
        }

        if (splitecamera.uiflg==true)
        {
            readytext.gameObject.SetActive(false);
            weponDropdown.Select();
            weponchoice.gameObject.SetActive(true);
            weponDropdown.gameObject.SetActive(true);
            gameStart.gameObject.SetActive(true);
        }


    }

    public void OnClickgamestart(Dropdown dropdown)
    {
        //ドロップボックスの要素を取得
        Debug.Log("dropdown.value = " + dropdown.value);   
        Debug.Log("text(options) = " + dropdown.options[dropdown.value].text); 
        Debug.Log("text(captionText) = " + dropdown.captionText.text); 

        readyPanel.gameObject.SetActive(false);   
    }
}
