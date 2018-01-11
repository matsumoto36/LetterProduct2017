using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ready : MonoBehaviour {
    public Canvas readyPanel;
    public Dropdown weponDropdown;
    public Text readytext, weponchoice;
    public Button gameStart;

    // Use this for initialization
    void Start ()
    {        
        //Readycanvasのみ表示
        readyPanel.gameObject.SetActive(true);
        weponchoice.gameObject.SetActive(false);
        weponDropdown.gameObject.SetActive(false);
        gameStart.gameObject.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Ready画面の操作
        if (Input.GetKey(KeyCode.Space))
        {
            readytext.enabled = false;
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
