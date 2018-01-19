using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamepadInput;
public class OptionButton : MonoBehaviour {
    public Canvas OptionMenuCanvas;

    public int playerIndex;

    // Use this for initialization
    void Start ()
    {
        OptionMenuCanvas.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (InputManager.GetButtonDown(playerIndex, GamePad.Button.B))
        {
            OnOptionBackClicked();
        }
    }

    public  void OnOptionClicked()
    {
        //pause画面表示
        OptionMenuCanvas.enabled = true;
    }
    public void OnOptionBackClicked()
    {
        //pause画面非表示
        OptionMenuCanvas.enabled = false;
    }
}
