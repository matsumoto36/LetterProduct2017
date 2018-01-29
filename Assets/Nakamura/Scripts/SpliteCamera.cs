using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamepadInput;

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

    public int playercnt = 4;
    

    public int playerIndex;


    bool flg = false;
    public  bool uiflg = false;
    public Ready ready;
    // Use this for initialization
    void Start ()
    {
        /*
        player1Camera.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
        player2Camera.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
        player3Camera.rect = new Rect(0f, 0f, 0.5f, 0.5f);
        player4Camera.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
        */

        mode = SpriteCameraMode.quartet;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (ready.flg == true)
        {
            if (InputManager.GetButtonDown(playerIndex, GamePad.Button.Start) || Input.GetKeyDown(KeyCode.A))
            {
                flg = true;

                switch (playercnt)
                {
                    case 1:
                        mode = SpriteCameraMode.solo;
                        uiflg = true;
                        break;
                    case 2:
                        mode = SpriteCameraMode.duo;
                        uiflg = true;
                        break;
                    case 3:
                        mode = SpriteCameraMode.trio;
                        uiflg = true;
                        break;
                    case 4:
                        mode = SpriteCameraMode.quartet;
                        uiflg = true;
                        break;
                }

            }
        }

        //プレイヤーに応じて画面を分割する 
        switch (mode)
        {
            //１プレイヤー
            case SpriteCameraMode.solo:
                player1Camera.rect = new Rect(0f, 0f, 1f, 1f);
                player2Camera.gameObject.SetActive(false);
                player3Camera.gameObject.SetActive(false);
                player4Camera.gameObject.SetActive(false);
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
}
