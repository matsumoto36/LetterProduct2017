using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScoreOther : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ShowScore(int score)
    {
        GetComponent<Text>().text = score + "ポイント".ToString();
    }
}
