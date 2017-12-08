using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// 攻撃のタイプ
    /// </summary>
    public enum Mode : int { BASIS, APPROACH, DUAL }//基本,接近,二刀流
    public Mode mode;
    
    private bool isRendered = false;    //画面内判定
    public float speed = 10;            //移動速度(秒速)
    public float dashSpeed = 15;        //急接近時の速度(秒速)
    public float moveLine = 20;         //検知範囲
    public float stepLine = 10;         //ステップ攻撃範囲
    public float attackLine = 3;        //攻撃距離
    public float searchAngle = 7;       //視野

    // Enemyスクリプト
    private Enemy enemySC;

    //public float[] weaponDistance = new float[2];   //射程距離
    //public float[] fillingTime = new float[2];      //充填時間

    //プレイヤー関連
    [SerializeField]
    private GameObject[] player;        //Playerオブジェクト
    private Player[] playerCS;          //Playerスクリプト
    [SerializeField]
    private int target;                 //一番近いプレイヤー
    [SerializeField]
    private float[] distance;           //距離

    /// <summary>
    /// 初期設定
    /// </summary>
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
                    distance[i] = 1000000f;//とにかく大きい数値
                }
                else
                {
                    //距離を計算(2乗された値)
                    distance[i] = ((transform.position - player[i].transform.position) * 100 / 100).sqrMagnitude;
                }
            }

            //距離比較
            target = 0;
            for (int i = 1; i < player.Length; i++)
            {
                //比較対象が存在,生存しているか
                if (player[target] == null || player[i] == null || playerCS[target].isDead || playerCS[i].isDead)
                {
                    continue;
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

            if (mode >= Mode.DUAL)
            {
                //プレイヤーの見えている正面からの角度(正規化)
                float f = Vector3.Angle((player[target].transform.position - transform.position).normalized, transform.forward);

                if (enemySC.isAttack == false && f <= searchAngle && distance[target] >= attackLine)
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
                
                if (distance[target] >= moveLine)
                {
                    //方向転換
                    DirctionChange();
                }
            }
            if (distance[target] < attackLine)
            {
                DirctionChange();

                if (enemySC.isAttack == false && mode >= Mode.BASIS)
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
        isRendered = false;
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    private void DirctionChange()
    {
        transform.rotation = Quaternion.Slerp(
                                transform.rotation,
                                Quaternion.LookRotation(player[target].transform.position - transform.position),
                                Time.deltaTime * speed * 0.5f);
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

    //SkinnedMeshRendererなるものが必要？
    /// <summary>
    /// 画面内判定
    /// </summary>
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
