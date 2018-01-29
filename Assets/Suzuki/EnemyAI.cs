using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// 攻撃タイプ
    /// </summary>
    public enum Mode : int { BASIS, APPROACH, DUAL }//基本,接近,二刀流
    public Mode mode;
    
    //private bool isRendered = false;    //画面内判定
    public float speed = 10;            //移動速度(秒速)
    public float dashSpeed = 15;        //急接近時の速度(秒速)
    public float moveLine = 20;         //検知範囲
    public float stepLine = 10;         //ステップ攻撃範囲
    public float attackLine = 3;        //攻撃距離
    public float searchAngle = 7;       //視野(度)

    // Enemyスクリプト
    private Enemy enemySC;

    //プレイヤー関連
    [SerializeField]
    private List<GameObject> playerList;//Playerリスト
    private Player[] playerCS;          //Playerスクリプト
    [SerializeField]
    private int target;                 //一番近いプレイヤー
    [SerializeField]
    private float[] distance;           //距離
    //[SerializeField]
    //private GameObject[] player;        //Playerオブジェクト

    /// <summary>
    /// 初期設定
    /// </summary>
    void Start()
    {
        ////プレイ人数の取得
        //player = GameObject.FindGameObjectsWithTag("Player");
        //playerCS = new Player[player.Length];
        //distance = new float[player.Length];

        ////Player参照
        //for (int i = 0; i < player.Length; i++)
        //{
        //    player[i] = Unit.unitList[0].gameObject;
        //    if (player[i] != null)
        //    {
        //        //Playerスクリプトを人数分取得
        //        playerCS[i] = player[i].GetComponent<Player>();
        //    }
        //}

        ////リストからプレイ人数の取得
        //player = new GameObject[1];
        //int playerCount = 0;
        //for (int i = 0; i < Unit.unitList.Count; i++)
        //{
        //    Debug.Log("ループ数 : " + i);
        //    if (Unit.unitList[i].gameObject.tag == "Player")
        //    {
        //        //playerを増量し登録
        //        if (playerCount != 0)
        //        {
        //            GameObject[] copyBox = player;
        //            player = new GameObject[playerCount + 1];
        //            for (int j = 0; j < copyBox.Length; j++)
        //            {
        //                player[j] = copyBox[j];
        //            }
        //        }
        //        Debug.Log(player.Length);
        //        player[playerCount] = Unit.unitList[i].gameObject;
        //        playerCount++;
        //    }
        //}
        //playerCS = new Player[player.Length];
        //distance = new float[player.Length];

        //リストからプレイ人数の取得
        playerList = new List<GameObject>();
        int playerCount = 0;
        for (int i = 0; i < Unit.unitList.Count; i++)
        {
            if (Unit.unitList[i].gameObject.tag == "Player")
            {
                //playerListに登録
                playerList.Add(Unit.unitList[i].gameObject);
                playerCount++;
            }
        }
        playerCS = new Player[playerList.Count];
        distance = new float[playerList.Count];

        //Player参照
        for (int i = 0; i < playerList.Count; i++)
        {
            //Playerスクリプトを人数分取得
            playerCS[i] = playerList[i].GetComponent<Player>();
        }

        //Enemyスクリプトを取得
        enemySC = GetComponent<Enemy>();
	}

	void Update()
    {
		//画面に映っていたら行動を起こす(廃止)
		if (true)//isRendered
        {
            int passCount = 0;
            for (int i = 0; i < playerList.Count; i++)
            {
                //player[i]が死亡なら処理をパス
                if (playerCS[i].isDead)
                {
                    passCount++;
                }
                else
                {
                    //距離を計算(2乗された値)
                    distance[i] = ((transform.position - playerList[i].transform.position) * 10000 / 10000).sqrMagnitude;
                }
            }

            //全滅したので以下の処理をしない
            if (passCount == playerList.Count)
            {
                return;
            }

            //距離比較
            target = 0;
            for (int i = 1; i < playerList.Count; i++)
            {
                //比較対象が存在,生存しているか
                if (playerCS[target].isDead)
                {
                    target = i;
                }
                else if (playerCS[i].isDead)
                {

                }

                //通常処理
                else
                {
                    if (distance[i] < distance[target])
                    {
                        target = i;
                    }
                }
            }

            //ルート化し正しき値へ
            distance[target] = Mathf.Sqrt(distance[target]);

            if (mode >= Mode.DUAL)
            {
                //方向転換
                DirctionChange();

                //プレイヤーの見えている正面からの角度(正規化)
                Vector3 vec3 = (playerList[target].transform.position - transform.position).normalized;
                float f = Vector3.Angle(vec3, transform.forward);

                if (enemySC.isAttack == false && f <= searchAngle && distance[target] >= stepLine)
                {
                    if (enemySC.equipWeapon[0].weaponType != WeaponType.Ranged)
                    {
                        //遠距離用の武器に交換
                        enemySC.SwitchWeapon(1);
                        Debug.Log("武器変更");
                    }

                    //遠距離攻撃
                    enemySC.Attack();
                    Debug.Log("ビーム");
                }
            }
            if (distance[target] < attackLine)
            {
                DirctionChange();

                if (enemySC.isAttack == false)
                {
                    if (enemySC.equipWeapon[0].weaponType != WeaponType.Melee)
                    {
                        //近接用の武器に交換
                        enemySC.SwitchWeapon(1);
                        Debug.Log("武器変更");
                    }

                    //攻撃
                    enemySC.Attack();
                    Debug.Log("剣");
                }
            }
            else if (enemySC.isAttack == false)//移動処理
            {
                if (distance[target] < stepLine && mode >= Mode.APPROACH)
                {
                    //急接近
                    Dash();
                }
                else if (distance[target] < moveLine && mode >= Mode.BASIS)
                {
                    //移動
                    Move();
                }
            }
        }
        //isRendered = false;
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    private void DirctionChange()
    {
        float f = 0.5f;

        if (enemySC.isAttack)
        {
            f /= 2;
        }

        transform.rotation = Quaternion.Slerp(
                                transform.rotation,
                                Quaternion.LookRotation(playerList[target].transform.position - transform.position),
                                Time.deltaTime * speed * f);
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        //方向転換
        DirctionChange();

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// 急接近処理
    /// </summary>
    private void Dash()
    {
        //方向転換
        DirctionChange();

        transform.position += transform.forward * dashSpeed * Time.deltaTime;
    }

    /// <summary>
    /// ターゲット変更時間
    /// </summary>
    /// <returns></returns>
    private IEnumerator TargetChange()
    {
        yield return new WaitForSeconds(5.0f);
    }

    //SkinnedMeshRendererなるものが必要？
    /// <summary>
    /// 画面内判定
    /// </summary>
    //private void OnWillRenderObject()
    //{
    //    isRendered = true;
    //}
}
