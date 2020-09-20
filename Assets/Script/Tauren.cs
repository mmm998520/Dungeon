using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Tauren : MonsterManager
    {
        public int[] guardPoint;
        public int nextGrardNum;
        enum Stat
        {
            guard,
            pursue,
            back
        }
        Stat stat = Stat.guard;

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
            if(stat == Stat.guard)
            {
                if (Vector3.Distance(GameManager.maze.GetChild(guardPoint[nextGrardNum]).position * Vector2.one, transform.position * Vector2.one) < 0.5f)
                {
                    print("arrive");
                    if (++nextGrardNum >= 4)
                    {
                        nextGrardNum = 0;
                    }
                }
                guard(guardPos);

                if (Vector3.Distance(transform.position*Vector2.one, MinDisPlayer().position * Vector2.one) < 3)
                {
                    stat = Stat.pursue;
                }
            }
            else if(stat == Stat.pursue)
            {
                target = MinDisPlayer();
                if (!pursuePos.Contains((int)transform.position.x * MapCreater.totalCol[MapCreater.level] + (int)transform.position.y))
                {
                    stat = Stat.back;
                }
            }
            else if(stat == Stat.back)
            {
                guard(pursuePos);
                HP = MaxHP;
                if (guardPos.Contains((int)transform.position.x * MapCreater.totalCol[MapCreater.level] + (int)transform.position.y))
                {
                    stat = Stat.guard;
                }
            }
            moveToTarget();
        }

        void guard(HashSet<int> canGo)
        {
            Vector3 endPos = GameManager.maze.GetChild(guardPoint[nextGrardNum]).position;
            Debug.Log(endPos);
            int[] endRow = new int[1] { (int)endPos.x }, endCol = new int[1] { (int)endPos.y };
            setNavigateTarget(endRow, endCol, canGo);
        }
    }
}