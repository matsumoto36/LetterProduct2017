﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMoveScene : MonoBehaviour
{
    /// <summary>移動先のシーン名</summary>
    public string sceneName;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player1"))
        {
            FadeManager.instance.LoadScene(sceneName, 2.0f);
        }
    }
}
