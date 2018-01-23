using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menu : MonoBehaviour {
    public Button startButton;
    public Dropdown playerSelect;
	// Use this for initialization
	void Start ()
    {
        playerSelect.Select();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
