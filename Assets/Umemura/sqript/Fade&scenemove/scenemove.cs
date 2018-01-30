using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemove : MonoBehaviour
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

    public void OnClick()
    {
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
