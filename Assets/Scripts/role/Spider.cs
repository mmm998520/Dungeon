using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Spider : MonsterManager
    {
        void Start()
        {
            monsterStart();
            cd = 1;
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
                navigation(end,null);
            }
            
            monsterUpdate();
            if (Input.anyKeyDown)
            {
                attack();
            }
            if (target.endTraget != null)
            {
                if (Vector3.Distance(transform.position, target.endTraget.position) < 2 && cdTimer > cd)
                {
                    attack();
                    cdTimer = 0;
                }
            }
        }

        override protected void attack()
        {
            if (target.endTraget != null)
            {
                //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
                float angle = Vector3.SignedAngle(Vector3.right, target.endTraget.position * Vector2.one - transform.position * Vector2.one, Vector3.forward);
                GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, Quaternion.Euler(0, 0, angle));
                //設定攻擊參數
                attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], false);
            }
        }
    }
}