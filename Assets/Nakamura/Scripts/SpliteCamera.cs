using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpliteCamera : MonoBehaviour
{
    public enum SpriteCameraMode
    {
        solo,
        duo,
        trio,
        quartet,
    };

    SpriteCameraMode mode;

    

    //画面分割カメラ
    public Camera player1Camera;
    public Camera player2Camera;
    public Camera player3Camera;
    public Camera player4Camera;

    // Use this for initialization
    void Start ()
    {
        switch (StartSelect.playercnt)
        {
            case 0:
                mode = SpriteCameraMode.solo;
                break;
            case 1:
                mode = SpriteCameraMode.duo;
                break;
            case 2:
                mode = SpriteCameraMode.trio;
                break;
            case 3:
                mode = SpriteCameraMode.quartet;
                break;
        }
        
        //プレイヤーに応じて画面を分割する 
        switch (mode)
        {
            //１プレイヤー
            case SpriteCameraMode.solo:
                break;

            //２プレイヤー  
            case SpriteCameraMode.duo:
                //３，４カメラは無効
                player3Camera.gameObject.SetActive(false);
                player4Camera.gameObject.SetActive(false);

                player1Camera.rect = new Rect(0f, 0f, 0.5f, 1f);
                player2Camera.rect = new Rect(0.5f, 0f, 0.5f, 1f);
                break;

            //３プレイヤー
            case SpriteCameraMode.trio:
                //４カメラのみ無効
                player4Camera.gameObject.SetActive(false);

                player1Camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                player2Camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                player3Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                break;

            //４プレイヤー
            case SpriteCameraMode.quartet:
                player1Camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                player2Camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                player3Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                player4Camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                break;
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
