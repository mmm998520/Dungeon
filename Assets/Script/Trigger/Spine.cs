using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spine : Trigger
    {
        public bool attack;
        public float attackTimer, attackTimerStoper;

        void Update()
        {
            if (attack)
            {
                if ((attackTimer += Time.deltaTime) > attackTimerStoper)
                {
                    attackTimer = 0;
                    attack = false;
                    transform.GetChild(0).gameObject.SetActive(false);
                }
                else
                {
                    Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 0.5f, 1 << 8 | 1 << 9);
                    for(int i=0;i< collider2Ds.Length; i++)
                    {
                        if (collider2Ds[i].GetComponent<PlayerManager>())
                        {
                            PlayerManager.HP -= Time.deltaTime * 5;
                        }
                        else if (collider2Ds[i].GetComponent<MonsterManager>())
                        {
                            collider2Ds[i].GetComponent<MonsterManager>().HP -= Time.deltaTime * 5;
                        }
                    }
                }
            }
        }
    }
}