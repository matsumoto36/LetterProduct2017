using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public Text player1;
    public Text player2;
    public Text player3;
    public Text player4;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void ShowDamage(int damage1,int damage2, int damage3, int damage4)
    {
        player1.text = damage1 + "ダメージ".ToString();
        player2.text = damage2 + "ダメージ".ToString();
        player3.text = damage3 + "ダメージ".ToString();
        player4.text = damage4 + "ダメージ".ToString();
    }
}
