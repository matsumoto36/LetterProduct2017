using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selectplayer : MonoBehaviour {
    public Dropdown playerdropdown;
	// Use this for initialization
	void Start ()
    {
        //プレイヤー人数のリスト作成
        if (playerdropdown)
        {
            playerdropdown.ClearOptions();   
            List<string> list = new List<string>();
            list.Add("1");
            list.Add("2");
            list.Add("3");
            list.Add("4");
            playerdropdown.AddOptions(list);  
            playerdropdown.value = 0;         
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}   
}
