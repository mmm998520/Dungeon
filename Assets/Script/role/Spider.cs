using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spider : MonsterManager
    {
        Transform hp;

        void Start()
        {
            RepTime = new WaitForSeconds(repTimer);
            stat = Stat.pursue;
            hp = transform.GetChild(1);
            int i, j, t = 0;
            greenPos = new HashSet<int>();
            pursuePos = new HashSet<int>();
            for (i = 0; i < MapCreater.totalRow[MapCreater.level]; i++)
            {
                for (j = 0; j < MapCreater.totalCol[MapCreater.level]; j++)
                {
                    if (MapCreater.mapArray[i, j] != (int)MapCreater.roomStat.wall)
                    {
                        print(t);
                        guardPos.Add(t);
                        pursuePos.Add(t);
                    }
                    t++;
                }
            }
        }

        void Update()
        {
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);
            randomMove();
            attackCD();
            if (prepare != 0)
            {
                target = MinDisPlayer();
                prepareAttack();
            }
            moveToTarget();
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
            GetComponent<Rigidbody2D>().WakeUp();
        }

        void randomMove()
        {
            if (Vector3.Distance(GameManager.maze.GetChild(guardPoint[nextGrardNum]).position * Vector2.one, transform.position * Vector2.one) < 0.5f)
            {
                print("arrive");
                nextGrardNum = Random.Range(0, guardPoint.Length);
            }
            guard(guardPos);
        }

        protected override void Attack()
        {
            print("a");
            StartCoroutine("AttackRep");
        }

        WaitForSeconds RepTime;
        IEnumerator AttackRep()
        {
            for(int i = 0; i < repTimes; i++)
            {
                base.Attack();
                if (i != repTimes - 1)
                {
                    yield return RepTime;
                }
            }
            nextGrardNum = Random.Range(0, guardPoint.Length);
            preparationTimer = 0;
            prepare = 0;
            attacked = false;
        }
    }
}