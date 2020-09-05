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
            moveSpeed = 1;
            monsterType = MonsterType.Spider;
            Invoke("reNavigate", 0.01f);
        }

        void Update()
        {
            end = new Transform[GameManager.Players.childCount];
            for (int i = 0; i < end.Length; i++)
            {
                end[i] = GameManager.Players.GetChild(i);
            }
            GoNavigate(navigateTarget);
            attackOccasion(navigateTarget, 2.5f);
            
            monsterUpdate();
        }

        void reNavigate()
        {
            Transform[] end = new Transform[GameManager.Players.childCount];
            for (int i = 0; i < end.Length; i++)
            {
                end[i] = GameManager.Players.GetChild(i);
            }
            navigateTarget = Navigate(end, null);
            Invoke("reNavigate", Random.Range(0.2f, 0.4f));
        }

        override protected void attack()
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

        protected override NearestEnd GoNextTarget()
        {
            //因為蜘蛛的終點就是玩家沒必要找新的了，直接直線追擊就好
            return StraightLineNearest(end);
        }
    }
}