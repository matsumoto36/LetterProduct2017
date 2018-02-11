using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// 攻撃タイプ
    /// </summary>
    public enum Mode : int { BASIS, APPROACH, DUAL }//基本,接近,二刀流
    public Mode mode;

    /// <summary>
    /// 攻撃モード
    /// </summary>
    public enum DistanceMode : int { MELEE, RANGED }    //近接,遠距離
    public DistanceMode disMode;
    
    //private bool isRendered = false;    //画面内判定
    public float speed = 10;            //移動速度(秒速)
    public float dashSpeed = 15;        //急接近時の速度(秒速)
    public float moveLine = 20;         //検知範囲
    public float stepLine = 10;         //ステップ攻撃範囲
    public float attackLine = 3;        //攻撃距離
    public float searchAngle = 7;       //視野(度)
    public float rangedAttackTime = 5;

    // Enemyスクリプト
    private Enemy enemySC;

    //プレイヤー関連
    [SerializeField]
    private List<Player> playerList;//Playerリスト
    [SerializeField]
    private int target;                 //一番近いプレイヤー
    [SerializeField]
    private float[] distance;           //距離
    private int passCount;
    //[SerializeField]
    //private GameObject[] player;        //Playerオブジェクト

    /// <summary>
    /// 初期設定
    /// </summary>
    void Start()
    {
		//Debug.Log("Start");

		//リストからプレイ人数の取得
		playerList = Unit.unitList
			.Where(item => item)
			.Where(item => item is Player)
			.Select(item => (Player)item)
			.ToList();

		distance = new float[playerList.Count];

        //Enemyスクリプトを取得
        enemySC = GetComponent<Enemy>();

        //初期化
        passCount = 0;

        TargetChange();
        if (distance[target] < attackLine)
        {
            disMode = DistanceMode.MELEE;
        }
        else
        {
            disMode = DistanceMode.RANGED;
            //遠距離用の武器に交換
            enemySC.SwitchWeapon(1);
        }
    }

	void Update()
    {
		//画面に映っていたら行動を起こす(廃止)
		if (true)//isRendered
        {
            //初期化
            passCount = 0;
            TargetChange();

            

            //ルート化し正しき値へ
            distance[target] = Mathf.Sqrt(distance[target]);

            if (mode == Mode.DUAL)
            {
                //方向転換
                DirctionChange();

                //プレイヤーの見えている正面からの角度(正規化)
                Vector3 vec3 = (playerList[target].transform.position - transform.position).normalized;
                float f = Vector3.Angle(vec3, transform.forward);

                if (distance[target] < moveLine && distance[target] > attackLine)
                {
                    Move();
                }

                if (enemySC.isAttack == false && f <= searchAngle)
                {
                    //if (enemySC.equipWeapon[0].weaponType != WeaponType.Ranged && distance[target] >= attackLine + 1)
                    if (disMode != DistanceMode.RANGED && distance[target] >= attackLine + 1)
                    {
                        //遠距離用の武器に交換
                        enemySC.SwitchWeapon(1);
                        //Debug.Log("交換");

                        disMode = DistanceMode.RANGED;
                    }

                    //遠距離攻撃
                    enemySC.Attack(rangedAttackTime);
                }
            }


            
            if (distance[target] < attackLine)
            {
                DirctionChange();

                if (enemySC.isAttack == false)
                {
                    //if (enemySC.equipWeapon[0].weaponType != WeaponType.Melee)
                    if (disMode != DistanceMode.MELEE)
                    {
                        //近接用の武器に交換
                        enemySC.SwitchWeapon(1);
                        //Debug.Log("交換");

                        disMode = DistanceMode.MELEE;
                    }

                    //攻撃
                    enemySC.Attack();
                }
            }
            else if (enemySC.isAttack == false)
            {
                if (distance[target] < stepLine && mode != Mode.BASIS)
                {
                    //急接近
                    Dash();
                }
                else if(distance[target] < moveLine)
                {
                    //移動
                    Move();
                }
            }
        }
        //isRendered = false;
    }

    private void TargetChange()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
			if(!playerList[i]) continue;

            //player[i]が死亡なら処理をパス
            if (playerList[i].isDead)
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
            if (playerList[i].isDead)
            {
                /*ターゲット変更無*/
            }
            else if (playerList[target].isDead)
            {
                target = i;
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
    }

    /// <summary>
    /// 方向転換
    /// </summary>
    private void DirctionChange()
    {
        float f1 = 0.5f;

        if (enemySC.isAttack)
        {
            //攻撃中は少し遅い
            f1 /= 2;
        }

        //transform.rotation = Quaternion.Slerp(
        //                        transform.rotation,
        //                        Quaternion.LookRotation(playerList[target].transform.position - transform.position),
        //                        Time.deltaTime * speed * f1);

		
        Vector3 targetPositon = playerList[target].transform.position;
        //高さを統一
        if (transform.position.y != targetPositon.y)
        {
            targetPositon = new Vector3(targetPositon.x, transform.position.y, targetPositon.z);
        }
        Quaternion targetRotation = Quaternion.LookRotation(targetPositon - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * speed * f1);
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
    //private void OnWillRenderObject()
    //{
    //    isRendered = true;
    //}
}
