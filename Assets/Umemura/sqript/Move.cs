using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 移動方向を保存する変数 pos
        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
        float speed = 5.0f;
        // カーソルキーによって移動方向を決定
        if (Input.GetKey(KeyCode.RightArrow) == true)
        {
            pos.x = speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) == true)
        {
            pos.x = -speed;
        }
        if (Input.GetKey(KeyCode.UpArrow) == true)
        {
            pos.z = speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow) == true)
        {
            pos.z = -speed;
        }
        // 変数値をconsoleウィンドウに表示
        Debug.LogFormat("pos=({0},{1},{2})", pos.x, pos.y, pos.z);
        // 現在の座標に移動量を加算する
        transform.position = transform.position + pos * Time.deltaTime;
    }
}