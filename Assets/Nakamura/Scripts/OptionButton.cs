using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OptionButton : MonoBehaviour {
    public Canvas OptionMenuCanvas;


    // Use this for initialization
    void Start ()
    {
        OptionMenuCanvas.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
       

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
