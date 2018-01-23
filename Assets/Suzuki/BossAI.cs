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
    [SerializeField]
    private bool spAttackFlg;
    private int rollingCount = 0;           //回転した回数
    private int rollingMax = 2;             //何回転するか
    private float spChargeTime = 60.0f;     //チャージ時間(秒)
    private float spAttackTime = 2;         //攻撃持続時間(秒)
    private float rotationNum = 0;          //回転した角度(度)

    // Enemyスクリプト
    private Enemy enemySC;

    //プレイヤー関連
    [SerializeField]
    private List<GameObject> playerList;    //Playerリスト
    private Player[] playerCS;              //Playerスクリプト
    [SerializeField]
    private int target;                     //狙うプレイヤー
    [SerializeField]
    private float distance;                 //ターゲットとの距離
    [SerializeField]
    private int[] damage;                   //各々に負わされたダメージ
    //[SerializeField]
    //private GameObject[] player;            //Playerオブジェクト

    //確認用変数
    [SerializeField]
    private float spCountDown = 60;//spChargeTimeの値

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

        //Player参照
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i] != null)
            {
                //Playerスクリプトを人数分取得
                playerCS[i] = playerList[i].GetComponent<Player>();
            }
        }

        //Enemyスクリプトを取得
        enemySC = GetComponent<Enemy>();

        //初期化
        spAttackFlg = false;
        target = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            damage[i] = 0;
        }

        //攻撃されたときの通知に登録
        enemySC.OnAttacked += OnAttacked;

        //カウントコルーチン始動
        StartCoroutine(TargetChange());
        StartCoroutine(SpecialAttackCounter());
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
            Debug.Log("武器変更");
        }

        enemySC.Attack();

        ////案1 回るが自然でない
        //float f = 360 * rollingMax * Time.deltaTime / spAttackTime;
        //rotationNum += f;
        //transform.Rotate(new Vector3(0, f, 0));

        //案2 微妙に良くなった?
        float f = 360 * rollingMax * Time.deltaTime / spAttackTime;
        rotationNum += f;
        var v = Quaternion.Slerp
            (Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z),
            Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z),
            Time.deltaTime);
        transform.Rotate(v.eulerAngles * spAttackTime);

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
            spAttackFlg = false;
        }
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
            for (int i = 0; i < playerList.Count; i++)
            {
                if (target == i)
                {
                    //比較対象がすでにターゲット状態
                }
                //比較対象が存在,生存しているか
                else if (playerList[target] == null || playerCS[target].isDead)
                {
                    target = i;
                }
                else if (playerList[i] == null || playerCS[i].isDead)
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
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i] == enemySC.attackedUnitList[enemySC.attackedUnitList.Count - 1].attackUnit)
                {
                    target = i;
                }
            }
        }

        //初期化
        for (int i = 0; i < playerList.Count; i++)
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

        spCountDown = 10;
        StartCoroutine(SpecialAttackCounter());
    }
}
