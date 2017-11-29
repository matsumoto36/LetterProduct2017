using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectWepon : MonoBehaviour {
    public Dropdown weponDropdown;

	// Use this for initialization
	void Start ()
    {
        //武器選択のリスト作成
        if (weponDropdown)
        {
            weponDropdown.ClearOptions();
            List<string> list = new List<string>();
            list.Add("Wepon1");
            list.Add("Wepon2");
            list.Add("Wepon3");
            list.Add("Wepon4");
            weponDropdown.AddOptions(list); 
            weponDropdown.value = 0;        
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
