﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInOut : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player1"))
        {
            FadeManager.Instance.LoadScene("Fade IN&OUT", 2.0f);
        }
    }
}
