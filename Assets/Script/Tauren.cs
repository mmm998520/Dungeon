using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Tauren : MonsterManager
    {
        void Start()
        {
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
            guardBehaviour();
            attackCD();
            if (prepare)
            {
                prepareAttack();
            }
        }
    }
}