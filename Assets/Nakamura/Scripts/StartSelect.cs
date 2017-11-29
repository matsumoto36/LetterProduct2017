using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSelect : MonoBehaviour {

    public static int playercnt;
    public void OnClick(Dropdown dropdown)
    {
        playercnt = dropdown.value;
        Debug.Log("dropdown.value = " + dropdown.value);    
        Debug.Log("text(options) = " + dropdown.options[dropdown.value].text); 
        Debug.Log("text(captionText) = " + dropdown.captionText.text); 
        //シーン遷移
        SceneManager.LoadScene("Main");
    }
    // Use this for initialization
    void Start ()
    {

		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnGoSelect()
    {
        SceneManager.LoadScene("Select");
    }
}
