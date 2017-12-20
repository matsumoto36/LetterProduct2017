using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    //private bool isRendered = false;    //画面内判定
    private bool spAttackFlg;
    public float speed = 10;                //移動速度(秒速)
    public float attackLine = 3;            //近接攻撃可能距離
    public float searchAngle = 7;           //視野
    public float targetChangeTime = 5.0f;   //
    public float specialAttackTime = 60.0f; //

    // Enemyスクリプト
    private Enemy enemySC;

    //プレイヤー関連
    [SerializeField]
    private GameObject[] player;        //Playerオブジェクト
    private Player[] playerCS;          //Playerスクリプト
    [SerializeField]
    private int target;                 //狙うプレイヤー
    [SerializeField]
    private float distance;           //距離
    [SerializeField]
    private int[] damage;               //負わされたダメージ

    void Awake()
    {
        //初期化
        spAttackFlg = false;
        for (int i = 0; i < player.Length; i++)
        {
            damage[i] = 0;
        }

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
        enemySC = GetComponent<Enemy>();

        StartCoroutine(TargetChange());
        StartCoroutine(SpecialAttackCounter());
    }
    
    void Update()
    {
        //ターゲットとの距離を計算(2乗された値)
        distance = ((transform.position - player[target].transform.position) * 10000 / 10000).sqrMagnitude;

        if (spAttackFlg)
        {
            //必殺
            for(int i = 0; i < 720; i += 6)
            {
                //transform.rotation = Quaternion.Slerp(
                //                transform.rotation,
                //                Quaternion.LookRotation(player[target].transform.position - transform.position),
                //                Time.deltaTime * speed * f);
            }
        }
        else if (distance < Mathf.Pow(attackLine, 2))
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
    /// ターゲット変更
    /// </summary>
    /// <returns></returns>
    IEnumerator TargetChange()
    {
        yield return new WaitForSeconds(targetChangeTime);

        //ダメージ比較
        target = 0;
        for (int i = 1; i < player.Length; i++)
        {
            //比較対象が存在,生存しているか
            if (player[target] == null || playerCS[target].isDead)
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

        for(int i = 0; i < player.Length; i++)
        {
            //初期化
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
        yield return new WaitForSeconds(specialAttackTime);
        spAttackFlg = true;
    }
}
