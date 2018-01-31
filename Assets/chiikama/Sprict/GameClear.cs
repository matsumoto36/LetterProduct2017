using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour {
	public Unit boss;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(boss && boss.nowHP <= 0)
		{
			Debug.Log("GameClear");
		}
	}
}
