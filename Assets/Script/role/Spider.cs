using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spider : MonsterManager
    {
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
            timer();
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);
            randomMove();
            attackCD();
            if (prepare != 0)
            {
                if (ridiculed == null)
                {
                    target = MinDisPlayer();
                }
                else
                {
                    target = ridiculed;
                }
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
    }
}