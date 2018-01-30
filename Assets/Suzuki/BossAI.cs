using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    //private bool isRendered = false;    //画面内判定

    //基本
    public float speed = 10;                //移動速度(秒速)
    public float attackLine = 3;            //近接攻撃可能距離
    public float searchAngle = 7;           //視野
    public float targetChangeTime = 5.0f;   //ターゲット変更時間

    //必殺関連
    private bool spAttackFlg;
    private bool oneRolling;
    public float spChargeTime = 60.0f;      //チャージ時間
    public float spAttackTime = 2;          //攻撃持続時間
    public float rotationNum = 0;           //回転数(角度)

    // Enemyスクリプト
    private denger2 enemySC;
    //ステータス
    private EnemyStructure statusSC;

    //プレイヤー関連
    [SerializeField]
    private GameObject[] player;    //Playerオブジェクト
    private Player[] playerCS;      //Playerスクリプト
    [SerializeField]
    private int target;             //狙うプレイヤー
    [SerializeField]
    private int fainalPlayer;       //最後に攻撃したプレイヤー
    [SerializeField]
    private float distance;         //ターゲットとの距離
    [SerializeField]
    private int[] damage;           //各々に負わされたダメージ

    void Awake()
    {
        //プレイ人数の取得
        player = GameObject.FindGameObjectsWithTag("Player");
        playerCS = new Player[player.Length];

        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] != null)
            {
                //Playerスクリプトを人数分取得
                playerCS[i] = player[i].GetComponent<Player>();
            }
        }
        //Enemyスクリプトを取得
        enemySC = GetComponent<denger2>();

        //初期化
        spAttackFlg = false;
        target = 0;
        for (int i = 0; i < player.Length; i++)
        {
            damage[i] = 0;
        }

        //攻撃されたときの通知に登録
        enemySC.OnAttacked += OnAttacked;

        //カウントコルーチン始動
        StartCoroutine(TargetChange());
        //StartCoroutine(SpecialAttackCounter());
    }
    
    void Update()
    {
        //ターゲットとの距離を計算(2乗された値)
        distance = ((transform.position - player[target].transform.position) * 10000 / 10000).sqrMagnitude;

        //HP判定
        if (statusSC.nowHP <= statusSC.maxHP * 0.3f)
        {
            //HPが30%以下なら必殺技を使う
            spAttackFlg = true;
        }

        ////行動処理
        //if (spAttackFlg)
        //{
        //    //必殺技
        //    SpecialAttack();
        //}
        /*else */if (distance < Mathf.Pow(attackLine, 2))
        {
            if (enemySC.equipWeapon[0].weaponType != WeaponType.Melee)
            {
                //近接用の武器に交換
                enemySC.SwitchWeapon(1);
                Debug.Log("武器変更");
            }

            //近接攻撃
            enemySC.Attack();
        }
        else
        {
            Move();
            if (enemySC.equipWeapon[0].weaponType != WeaponType.Ranged)
            {
                //遠距離用の武器に交換
                enemySC.SwitchWeapon(1);
                Debug.Log("武器変更");
            }

            //遠距離攻撃
            enemySC.Attack();
        }
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    private void DirctionChange()
    {
        float f = 0.5f;

        transform.rotation = Quaternion.Slerp(
                                transform.rotation,
                                Quaternion.LookRotation(player[target].transform.position - transform.position),
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
    /// 必殺技
    /// </summary>
    public void SpecialAttack()
    {
        if (enemySC.equipWeapon[0].weaponType != WeaponType.Ranged)
        {
            //遠距離用の武器に交換
            enemySC.SwitchWeapon(1);
            Debug.Log("武器変更");
        }

        enemySC.Attack();

        float f = 720 * Time.deltaTime / spAttackTime;
        transform.Rotate(new Vector3(0, f, 0));
        rotationNum += f;

        if (rotationNum >= 360)
        {
            rotationNum -= 360;    //初期化
            if (oneRolling)
            {
                oneRolling = false;
                spAttackFlg = false;
            }
            else
            {
                oneRolling = true;
            }
        }
    }
    
    /// <summary>
    /// 攻撃されたときに実行される
    /// </summary>
    /// <param name="from">攻撃したキャラクター</param>
    void OnAttacked(Unit from)
    {
        for (int i = 0; i < player.Length; i++)
        {
            if (player[i] == from)
            {
                damage[i] += enemySC.attackedUnitList[enemySC.attackedUnitList.Count - 1].damage;
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

        //targetChangeTime秒間に攻撃を受けた
        if (Time.time - enemySC.attackedUnitList[enemySC.attackedUnitList.Count - 1].time <= targetChangeTime)
        {
            //ダメージ比較
            for (int i = 0; i < player.Length; i++)
            {
                if (target == i)
                {
                    //比較対象がすでにターゲット状態
                }
                //比較対象が存在,生存しているか
                else if (player[target] == null || playerCS[target].isDead)
                {
                    target = i;
                }
                else if (player[i] == null || playerCS[i].isDead)
                {
                    //処理無し
                }

                //通常処理
                else
                {
                    if (damage[i] < damage[target])
                    {
                        target = i;
                    }
                }
            }
        }
        //攻撃を受けなかった
        else
        {
            for (int i = 0; i < player.Length; i++)
            {
                if (player[i] == enemySC.attackedUnitList[enemySC.attackedUnitList.Count - 1].attackUnit)
                {
                    target = i;
                }
            }
        }

        //初期化
        for (int i = 0; i < player.Length; i++)
        {
            damage[i] = 0;
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
    }
}
