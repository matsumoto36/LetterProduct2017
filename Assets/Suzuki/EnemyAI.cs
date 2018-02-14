using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public float searchDegree;
    public float searchDistance;
    public float attackRange;

    bool lookPlayer = false;

    bool moveFlg = true;

    //AIが向く方向を入れる
    Quaternion lookRotation;

    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        enemy.OnAttacked += (unit, point) =>
        {
            lookRotation = Quaternion.LookRotation
                (new Vector3(unit.transform.position.x - transform.position.x, 0, unit.transform.position.z - transform.position.z));
        };

        lookRotation = transform.rotation;
    }

    private void Update()
    {
        //移動処理はせるふ

        //視認距離にプレイヤーがいるかどうか
        if (SearchNearestPlayer() != null)
        {
            GameObject nearestPlayer = SearchNearestPlayer();
            //視野角内にプレイヤーがいるかどうか
            if (Vector3.Angle((nearestPlayer.transform.position - transform.position).normalized, transform.forward) <= searchDegree / 2)
            {
                RaycastHit hit;
                //PlayerとAIの間に障害物がないか判断
                if (Physics.Raycast(transform.position,(nearestPlayer.transform.position - transform.position).normalized,out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        lookPlayer = true;

                        lookRotation = Quaternion.LookRotation
                            (new Vector3(nearestPlayer.transform.position.x - transform.position.x, 0, nearestPlayer.transform.position.z - transform.position.z));


                        //攻撃範囲かどうか
                        if (Vector3.Distance(nearestPlayer.transform.position, transform.position) <= attackRange)
                        {
                            enemy.Attack();
                        }
                        //攻撃範囲外
                        else
                        {
                            //transform.position += (transform.forward * enemy.moveSpeed) * Time.deltaTime;
                            if (moveFlg)
                            {
                                StartCoroutine(EnemyMoveDirection());
                            }
                        }
                    }
                    else
                    {
                        lookPlayer = false;
                    }
                }
            }
        }

        EnemyRotation();
    }


    /// <summary>
    /// 視認距離内で一番近いプレイヤーを検索して返す
    /// </summary>
    /// <returns>視認距離内で一番近いプレイヤー</returns>
    private GameObject SearchNearestPlayer()
    {
        //プレイヤーの配列の取得(要素がnullの場合もある)
        var playerArray = GameData.instance.spawnedPlayer;

        GameObject nearestPlayer = null;

        //一番近い距離を判断するために一番大きい値で初期化しておく
        float nearestDis = Mathf.Infinity;

        //一番近いプレイヤーを検索
        foreach (var player in playerArray)
        {
            if (!player) continue;

            float dis = Vector3.Distance(transform.position, player.transform.position);
            if (dis <= nearestDis && !player.isDead)
            {
                nearestPlayer = player.gameObject;
                nearestDis = dis;
            }
        }

        //視認距離内ならそのプレイヤーを返す
        if (nearestDis < searchDistance)
        {
            return nearestPlayer;
        }
        //いなかったらnull
        else
        {
            return null;
        }
    }

    IEnumerator EnemyMoveDirection()
    {
        moveFlg = false;

        float timeInterval = 2.0f;

        float moveTimer = 0.0f;

        while (timeInterval > moveTimer)
        {
            transform.position += (transform.forward * enemy.moveSpeed) * Time.deltaTime;

            moveTimer += Time.deltaTime;

            yield return null;
        }

        moveFlg = true;
    }

    private void EnemyRotation()
    {
        //Playerの方向に回転
        transform.rotation = Quaternion.Slerp(transform.rotation,lookRotation, enemy.rotSpeed * Time.deltaTime);
    }

    public bool LookPlayer()
    {
        return lookPlayer;
    }
}
