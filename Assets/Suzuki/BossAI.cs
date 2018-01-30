using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAI : MonoBehaviour
{
    //private bool isRendered = false;    //画面内判定

    //基本
    public float speed = 10;                //移動速度(秒速)
    public float attackLine = 3;            //近接攻撃可能距離
    public float searchAngle = 7;           //視野(度)
    public float targetChangeTime = 5.0f;   //ターゲット変更時間(秒)

    //必殺関連
    private bool spAttackFlg;
    private int rollingCount = 0;           //回転した回数
    public int rollingMax = 2;             //何回転するか
    private float rollingSpeed = 0;         //回転速度
    [SerializeField]
    private float acceleration;             //加速度
    [SerializeField]
    private float frameParSecond;
    private float spChargeTime = 60.0f;     //チャージ時間(秒)
    public float spAttackTime = 2;         //攻撃持続時間(秒)
    private float rotationNum = 0;          //回った角度(度)


    // Enemyスクリプト
    private Enemy enemySC;

    //プレイヤー関連
    [SerializeField]
    private List<GameObject> playerList;    //Playerリスト
    private Player[] playerCS;              //Playerスクリプト
    [SerializeField]
    private int target;                     //狙うプレイヤー
    int lastAttackUnit;                     //最後に攻撃したPlayer
    [SerializeField]
    private float distance;                 //ターゲットとの距離
    [SerializeField]
    private int[] damage;                   //各々に負わされたダメージ
    [SerializeField]
    private int[] beforeTotalDamage;
    //[SerializeField]
    //private GameObject[] player;            //Playerオブジェクト

    //確認用変数
    [SerializeField]
    private float spCountDown;//spChargeTimeの値

    void Start()
    {
        //リストからプレイ人数の取得
        playerList = new List<GameObject>();
        int playerCount = 0;
        for (int i = 0; i < Unit.unitList.Count; i++)
        {
            if (Unit.unitList[i].gameObject.tag == "Player")
            {
                //playerList登録
                playerList.Add(Unit.unitList[i].gameObject);
                playerCount++;
            }
        }
        playerCS = new Player[playerList.Count];
        damage = new int[playerList.Count];
        beforeTotalDamage = new int[playerList.Count];

        //Player参照
        for (int i = 0; i < playerList.Count; i++)
        {
            //Playerスクリプトを人数分取得
            playerCS[i] = playerList[i].GetComponent<Player>();
        }

        //Enemyスクリプトを取得
        enemySC = GetComponent<Enemy>();

        //初期化
        spAttackFlg = false;
        target = 0;
        lastAttackUnit = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            damage[i] = 0;
            beforeTotalDamage[i] = 0;
        }
        frameParSecond = 1 / Time.deltaTime;
        while (frameParSecond > 60)
        {
            frameParSecond = 1 / Time.deltaTime;
        }

        //攻撃されたときの通知に登録
        enemySC.OnAttacked += OnAttacked;

        //カウントコルーチン始動
        StartCoroutine(TargetChange());
        StartCoroutine(SpecialAttackCounter());

        //確認用
        spCountDown = spChargeTime;
    }
    
    void Update()
    {
        //ターゲットとの距離を計算(2乗された値)
        distance = ((transform.position - playerList[target].transform.position) * 10000 / 10000).sqrMagnitude;

        ////HP判定
        if (enemySC.nowHP <= enemySC.maxHP * 0.3f)
        {
            //HPが30%以下なら必殺技を使う
            spAttackFlg = true;
        }

        //行動処理
        if (spAttackFlg)
        {
            //必殺技
            SpecialAttack();
        }
        else if (distance < Mathf.Pow(attackLine, 2))
        {
            if (!enemySC.isAttack)
            {
                DirctionChange();
            }

            if (enemySC.equipWeapon[0].weaponType != WeaponType.Melee)
            {
                //近接用の武器に交換
                enemySC.SwitchWeapon(1);
            }

            //近接攻撃
            enemySC.Attack();
        }
        else
        {
            Move();

            if (enemySC.equipWeapon[0].weaponType != WeaponType.Ranged && distance > Mathf.Pow(attackLine, 2) + 1)
            {
                //遠距離用の武器に交換
                enemySC.SwitchWeapon(1);
            }

            //遠距離攻撃
            enemySC.Attack();
        }

        //確認用
        spCountDown -= Time.deltaTime;
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    private void DirctionChange()
    {
        float f = 0.5f;

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
        DirctionChange();

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// 必殺技
    /// </summary>
    public void SpecialAttack()
    {
        if (enemySC.equipWeapon[0].weaponType != WeaponType.Ranged)
        {
            //遠距離用の武器に交換
            enemySC.SwitchWeapon(1);
        }

        enemySC.Attack();

        ////案1 回るが自然でない
        //float f = 360 * rollingMax * Time.deltaTime / spAttackTime;
        //rotationNum += f;
        //transform.Rotate(new Vector3(0, f, 0));

        ////案2 微妙に良くなった?　やってることが変わらない
        //float f = 360 * rollingMax * Time.deltaTime / spAttackTime;
        //rotationNum += f;
        //var v = Quaternion.Slerp
        //    (Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z),
        //    Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z),
        //    Time.deltaTime);
        //transform.Rotate(v.eulerAngles * spAttackTime);

        //案3 加速して回る 時々速度が速い
        CalculateRollingSpeed();
        if (rotationNum <= 360 * rollingMax / 2)
        {
            rollingSpeed += acceleration;
        }
        else
        {
            rollingSpeed += -acceleration;
        }

        if (rollingSpeed >= 0)
        {
            //通常
            transform.Rotate(new Vector3(0, rollingSpeed, 0));
            rotationNum += rollingSpeed;
        }
        else
        {
            //逆回転防止
            rollingCount++;
        }

        if (rotationNum >= 360 * (rollingCount + 1))
        {
            //回転数カウント
            rollingCount++;
        }

        //指定回数回った
        if (rollingCount >= rollingMax)
        {
            //終了
            rollingCount = 0;
            rotationNum = 0;
            rollingSpeed = 0;
            spAttackFlg = false;
        }
    }

    void CalculateRollingSpeed()
    {
        int i = 1000;

        ////誤差が出るためあらかじめにTime.deltaTimeを計算
        //float f2 = Mathf.Round(Mathf.Pow(Time.deltaTime, 2) * Mathf.Pow(i, 2)) / Mathf.Pow(i, 2);
        //float f = 360 * rollingMax / Mathf.Pow(spAttackTime / 2, 2) * f2;
        
        float f = 360 * rollingMax / Mathf.Pow(spAttackTime / 2, 2) / Mathf.Pow(frameParSecond, 2);

        acceleration = Mathf.Round(f * i) / i;//小数点第3位未満
    }


    /// <summary>
    /// 攻撃されたときに実行される
    /// </summary>
    /// <param name="from">攻撃したキャラクター</param>
    void OnAttacked(Unit from)
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] == from.gameObject)
            {
                for (int j = 0; j < enemySC.attackedUnitList.Count; j++)
                {
                    if (enemySC.attackedUnitList[j].attackUnit == from)
                    {
                        damage[i] = enemySC.attackedUnitList[j].damage - beforeTotalDamage[i];
                        break;
                    }
                }
                break;
            }
        }

        //ログの取得
        //enemySC.attackedUnitList
        //何秒前に攻撃されたか求める
        //Time.time - enemySC.attackedUnitList[0].time
        //配列の.length = Listの .Count
    }

    /// <summary>
    /// ターゲット変更
    /// </summary>
    /// <returns></returns>
    IEnumerator TargetChange()
    {
        //targetChangeTime秒待つ(5秒)
        yield return new WaitForSeconds(targetChangeTime);

        //まだ一度も攻撃されていない
        if (enemySC.attackedUnitList.Count < 1)
        {
            target = 0;
        }
        else
        {
            //最後に攻撃したPlayerを算出
            for (int i = 0; i < enemySC.attackedUnitList.Count; i++)
            {
                if (i == 0)
                {
                    lastAttackUnit = 0;
                }
                else if (enemySC.attackedUnitList[lastAttackUnit].time > enemySC.attackedUnitList[i].time)
                {
                    lastAttackUnit = i;
                }
            }

            //targetChangeTime秒間に攻撃を受けた
            if (Time.time - enemySC.attackedUnitList[lastAttackUnit].time <= targetChangeTime)
            {
                List<int> maxDamage = new List<int>();

                //ダメージ比較
                for (int i = 0; i < playerList.Count; i++)
                {
                    if (target == i)
                    {
                        //比較対象がすでにターゲット状態
                    }
                    //比較対象が生存しているか
                    else if (playerCS[target].isDead)
                    {
                        target = i;
                    }
                    else if (playerCS[i].isDead)
                    {
                        //処理無し
                    }
                    //通常処理
                    else
                    {
                        if (damage[i] > damage[target])
                        {
                            maxDamage.Clear();
                            maxDamage.Add(i);
                        }
                        else if (damage[i] == damage[target])
                        {
                            maxDamage.Add(i);
                        }
                    }
                }
                //乱数でターゲット変更
                int j = Random.Range(0, maxDamage.Count);
                target = maxDamage[j];
            }
            //攻撃を受けなかった
            else
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                    if (playerList[i] == enemySC.attackedUnitList[lastAttackUnit].attackUnit)
                    {
                        target = i;
                    }
                }
            }

            //初期化
            for (int i = 0; i < damage.Length; i++)
            {
                damage[i] = 0;
            }
            for (int i = 0; i < beforeTotalDamage.Length; i++)
            {
                for (int j = 0; j < enemySC.attackedUnitList.Count; j++)
                {
                    if(playerList[i] == enemySC.attackedUnitList[j].attackUnit.gameObject)
                    {
                        //合計値を記録
                        beforeTotalDamage[i] = enemySC.attackedUnitList[j].damage;
                        break;
                    }
                }
            }
        }

        StartCoroutine(TargetChange());
    }
    
    /// <summary>
    /// 必殺チャージをカウント
    /// </summary>
    /// <returns></returns>
    IEnumerator SpecialAttackCounter()
    {
        yield return new WaitForSeconds(spChargeTime);
        spAttackFlg = true;

        spCountDown = spChargeTime;
        StartCoroutine(SpecialAttackCounter());
    }
}
