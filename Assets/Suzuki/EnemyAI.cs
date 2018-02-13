using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyAI : MonoBehaviour
{
    public float searchDegree;
    public float searchDistance;
    public float attackRange;

    Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();

        //攻撃の実行(攻撃状態時に毎フレーム呼んでok)
        enemy.Attack();
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
                if (Physics.Linecast(transform.position,nearestPlayer.transform.position,out hit))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        //Playerの方向に回転
                        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation
                            (new Vector3(nearestPlayer.transform.position.x - transform.position.x, 0, nearestPlayer.transform.position.z - transform.position.z)), enemy.rotSpeed * Time.deltaTime);
                        //攻撃範囲かどうか
                        if (Vector3.Distance(nearestPlayer.transform.position, transform.position) <= attackRange)
                        {
                            enemy.Attack();
                        }
                        //攻撃範囲外
                        else
                        {
                            transform.position += (transform.forward * enemy.moveSpeed) * Time.deltaTime;
                        }
                    }
                }
            }
        }
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
            if (dis <= nearestDis)
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
}
