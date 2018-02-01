using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

	public Player attaker;
	public Player[] player;
	public void Clickbutton()
	{
		Unit.Attack(attaker, player[0], 10);//ダメージ
		Unit.Attack(attaker, player[1], 10);
		Unit.Attack(attaker, player[2], 10);
		Unit.Attack(attaker, player[3], 10);
		Debug.Log("damage");
	}
	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	
}

