using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Tauren : MonsterManager
    {
        void Start()
        {
            monsterStart();
            cd = 2;
        }

        void Update()
        {
            if (Input.anyKeyDown)
            {
                Transform[] end = new Transform[GameManager.Players.childCount];
                for (int i = 0; i < end.Length; i++)
                {
                    end[i] = GameManager.Players.GetChild(i);
                }
                navigationNearestPlayer(end);
            }
            
            monsterUpdate();
            if (Input.anyKeyDown)
            {
                attack();
            }
            if (Vector3.Distance(transform.position, target.enemyTraget.position)<2 && cdTimer>cd)
            {
                attack();
                cdTimer = 0;
            }
        }

        override protected void attack()
        {
            //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
            float angle = Vector3.SignedAngle(Vector3.right, target.enemyTraget.position * Vector2.one - transform.position * Vector2.one, Vector3.forward);
            GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, Quaternion.Euler(0, 0, angle));
            //設定攻擊參數
            attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], false);
        }
    }
}