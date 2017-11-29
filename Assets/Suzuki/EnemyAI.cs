using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    //タイプ
    public enum Mode : int { BASIS, APPROACH, DUAL }//基本,接近,二刀流
    public Mode mode;
    
    private bool isRendered = false;    //画面内判定
    public float speed;         //移動速度(秒速)
    public float dashSpeed;     //急接近時の速度(秒速)
    public float moveLine;      //検知範囲
    public float stepLine;      //ステップ攻撃範囲
    public float attackLine;    //攻撃距離
    public float searchAngle;   //視野

    private Enemy enemySC;

    //武器(0:近接, weaponNum:遠距離)
    public int weaponNum;
    //public float[] weaponDistance = new float[2];   //射程距離
    //public float[] fillingTime = new float[2];      //充填時間

    //プレイヤー
    public GameObject[] player; //
    private Player[] playerCS;  //Playerスクリプト
    public int target;          //一番近いプレイヤー
    private float[] distance;   //距離

    void Awake()
    {
        //プレイ人数の取得
        player = GameObject.FindGameObjectsWithTag("Player");
        playerCS = new Player[player.Length];
        distance = new float[player.Length];

        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] != null)
            {
                //Playerスクリプトを人数分取得
                playerCS[i] = player[i].GetComponent<Player>();
            }
        }
        //Enemyスクリプトを取得
        enemySC = GetComponent<Enemy>();
    }

    void Update()
    {
        //画面に映っていたら行動を起こす
        if (isRendered)
        {
            for (int i = 0; i < player.Length; i++)
            {
                //player[i]が居ない,死亡なら処理をパス
                if (player[i] == null || playerCS[i].isDead)
                {
                    //初期化
                    distance[i] = moveLine * moveLine * moveLine;//とにかく大きい数値
                }
                else
                {
                    //距離を計算(2乗された値)
                    distance[i] = (transform.position - player[i].transform.position).sqrMagnitude;
                }
            }

            //距離比較
            target = 0;
            for (int i = 0; i < player.Length; i++)
            {
                for (int j = (i + 1); j < player.Length; j++)
                {
                    //比較対象が存在するか
                    if (player[i] == null || player[j] == null)
                    {
                        if (player[i] != null && distance[i] < distance[target])
                        {
                            target = i;
                        }
                        else if (player[j] != null && distance[j] < distance[target])
                        {
                            target = j;
                        }
                    }
                    //通常処理
                    else
                    {
                        if (distance[i] < distance[j])
                        {
                            target = i;
                        }
                        else if (distance[i] > distance[j])
                        {
                            target = j;
                        }
                    }
                }
            }

            //ルート化し正しき値へ
            distance[target] = Mathf.Sqrt(distance[target]);

            //switch (mode)
            //{
            //    case Mode.BASIS:
            //        Basis();
            //        break;
            //    case Mode.APPROACH:
            //        Approach();
            //        break;
            //    case Mode.DUAL:
            //        Dual();
            //        break;
            //}

            //確認用
            Debug.Log("ターゲット:" + target + " 距離:" + distance[target]);

            if (mode >= Mode.DUAL)
            {
                //方向転換
                DirctionChange();

                //プレイヤーの見えている正面からの角度
                float f = Vector3.Angle((player[target].transform.position - transform.position).normalized, transform.forward);
                Debug.Log(f+"°");

                if (enemySC.isAttack == false && f <= searchAngle && distance[target] >= attackLine)
                {
                    Debug.Log("ビーム");

                    //if(Weapon.weaponData)
                    enemySC.SwitchWeapon(weaponNum);  //武器交換

                    //遠距離攻撃
                    enemySC.Attack();
                }
            }
            if (distance[target] < attackLine)
            {
                //方向転換
                DirctionChange();

                if (enemySC.isAttack == false && mode >= Mode.BASIS)
                {
                    Debug.Log("剣");

                    //攻撃
                    enemySC.SwitchWeapon(1);  //武器交換
                    enemySC.Attack();
                }
            }
            else if (distance[target] < stepLine && mode >= Mode.APPROACH)
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
        isRendered = false;
    }

    //方向転換
    void DirctionChange()
    {
        transform.rotation = Quaternion.Slerp(
                                transform.rotation,
                                Quaternion.LookRotation(player[target].transform.position - transform.position),
                                Time.deltaTime * speed * 0.5f);
    }

    //移動処理
    void Move()
    {
        //方向転換
        DirctionChange();

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    //急接近処理
    void Dash()
    {
        //方向転換
        DirctionChange();

        transform.position += transform.forward * dashSpeed * Time.deltaTime;
    }

    //画面内判定
    //SkinnedMeshRendererなるものが必要？
    private void OnWillRenderObject()
    {
        isRendered = true;
    }

    ////基本型(保存用)
    //private void Basis()
    //{
    //    if (distance[target] < attackLine)
    //    {
    //        if (attackCan)
    //        {
    //            Debug.Log("剣");
    //            //攻撃


    //            //充填処理
    //            StartCoroutine(AttackTime(fillingTime[0]));
    //        }
    //    }
    //    else if (distance[target] < moveLine)
    //    {
    //        //移動
    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            Quaternion.LookRotation(player[target].transform.position - transform.position),
    //            Time.deltaTime * speed);
    //        transform.position += transform.forward * speed * Time.deltaTime;
    //    }
    //}

    //近接型(保存用)
    //private void Approach()
    //{
    //    if (distance[target] < attackLine)
    //    {
    //        if (attackCan)
    //        {
    //            //攻撃


    //            //充填処理
    //            StartCoroutine(AttackTime(fillingTime[0]));
    //        }
    //    }
    //    else if (distance[target] < stepLine)
    //    {
    //        //急接近
    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            Quaternion.LookRotation(player[target].transform.position - transform.position),
    //            Time.deltaTime * dashSpeed);
    //        transform.position += transform.forward * dashSpeed * Time.deltaTime;
    //    }
    //    else if (distance[target] < moveLine)
    //    {
    //        //移動
    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            Quaternion.LookRotation(player[target].transform.position - transform.position),
    //            Time.deltaTime * speed);
    //        transform.position += transform.forward * speed * Time.deltaTime;
    //    }
    //}

    //二刀流(保存用)
    //private void Dual()
    //{

    //    if (distance[target] >= attackLine && attackCan)
    //    {
    //        Debug.Log("ビーム");
    //        //遠距離攻撃


    //        //充填処理
    //        StartCoroutine(AttackTime(fillingTime[1]));
    //    }

    //    if (distance[target] < attackLine)
    //    {
    //        if (attackCan)
    //        {
    //            Debug.Log("剣");
    //            //攻撃


    //            //充填処理
    //            StartCoroutine(AttackTime(fillingTime[0]));
    //        }
    //    }
    //    else if (distance[target] < stepLine)
    //    {
    //        //急接近
    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            Quaternion.LookRotation(player[target].transform.position - transform.position),
    //            Time.deltaTime * dashSpeed);
    //        transform.position += transform.forward * dashSpeed * Time.deltaTime;
    //    }
    //    else if (distance[target] < moveLine)
    //    {
    //        //移動
    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            Quaternion.LookRotation(player[target].transform.position - transform.position),
    //            Time.deltaTime * speed);
    //        transform.position += transform.forward * speed * Time.deltaTime;
    //    }
    //}

    //充填時間カウントコルーチン
    //private IEnumerator AttackTime(int i)
    //{
    //    //0:近接 1:遠距離
    //    attackCan[i] = false;
    //    yield return new WaitForSeconds(fillingTime[i]);
    //    attackCan[i] = true;
    //}
}
