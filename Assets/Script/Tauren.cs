using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Tauren : MonsterManager
    {
        Transform hp;

        void Start()
        {
            hp = transform.GetChild(1);
            rotateSpeed = 200;
            int i, j, t =0;
            greenPos = new HashSet<int>();
            pursuePos = new HashSet<int>();
            for (i = 0; i < MapCreater.totalRow[MapCreater.level]; i++)
            {
                for (j = 0; j < MapCreater.totalCol[MapCreater.level]; j++)
                {
                    if (MapCreater.mapArray[i, j] == (int)MapCreater.roomStat.green)
                    {
                        print(t);
                        guardPos.Add(t);
                        pursuePos.Add(t);
                    }
                    else if (MapCreater.mapArray[i, j] == (int)MapCreater.roomStat.black)
                    {
                        pursuePos.Add(t);
                    }
                    t++;
                }
            }
        }

        void Update()
        {
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);
            guardBehaviour();
            attackCD();
            if (prepare)
            {
                prepareAttack();
            }
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            ButtonCreater.Taurens.Remove(this);
            if (ButtonCreater.Taurens.Count == 0)
            {
                ButtonCreater.Exit.enabled = true;
            }
        }
    }
}