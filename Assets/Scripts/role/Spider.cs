using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Spider : MonsterManager
    {
        Transform[] end;
        void Start()
        {
            monsterStart();
            cd = 1;
            moveSpeed = 1.6f;
            monsterType = MonsterType.Spider;
            Invoke("reNavigate", 0.01f);
        }

        void Update()
        {
            if (!paralysis)
            {
                List<Transform> end = new List<Transform>();
                foreach (Transform player in GameManager.Players)
                {
                    PlayerManager playerManager = player.GetComponent<PlayerManager>();
                    if (!(playerManager.career == Career.Thief && playerManager.statOne))
                    {
                        end.Add(player);
                    }
                }
                this.end = end.ToArray();
                if (ridiculedTarget != null)
                {
                    GoNavigate(ridiculedTarget);
                    attackOccasion(ridiculedTarget, 2.5f);
                }
                else
                {
                    GoNavigate(navigateTarget);
                    attackOccasion(navigateTarget, 2.5f);
                }
            }

            
            monsterUpdate();
        }

        void reNavigate()
        {
            List<Transform> end = new List<Transform>();
            foreach (Transform player in GameManager.Players)
            {
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                if (!(playerManager.career == Career.Thief && playerManager.statOne))
                {
                    end.Add(player);
                }
            }
            navigateTarget = Navigate(end.ToArray(), null);
            Invoke("reNavigate", Random.Range(0.2f, 0.4f));
        }

        override protected void attack()
        {
            StartCoroutine("attackWait");

        }

        WaitForSeconds AttackWait = new WaitForSeconds(1);
        IEnumerator attackWait()
        {
            transform.GetChild(2).gameObject.SetActive(true);
            yield return AttackWait;
            transform.GetChild(2).gameObject.SetActive(false);
            if (!paralysis)
            {
                if (navigateTarget.endTraget != null)
                {
                    //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
                    float angle = Vector3.SignedAngle(Vector3.right, navigateTarget.endTraget.position * Vector2.one - transform.position * Vector2.one, Vector3.forward);
                    GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, Quaternion.Euler(0, 0, angle));
                    //設定攻擊參數
                    attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], null);
                }
            }
        }

        protected override NearestEnd GoNextTarget()
        {
            //因為蜘蛛的終點就是玩家沒必要找新的了，直接直線追擊就好
            return StraightLineNearest(end);
        }
    }
}