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

        void Update()
        {
            if(stat == Stat.guard)
            {
                guard(guardPos);
                if(Vector3.Distance(transform.position*Vector2.one, MinDisPlayer().position * Vector2.one) < 3)
                {
                    stat = Stat.pursue;
                }
            }
            else if(stat == Stat.pursue)
            {
                target = MinDisPlayer();
                if (!pursuePos.Contains((int)transform.position.x * MapCreater.totalCol + (int)transform.position.y))
                {
                    stat = Stat.back;
                }
            }
            else if(stat == Stat.back)
            {
                guard(pursuePos);
                HP = MaxHP;
                if (guardPos.Contains((int)transform.position.x * MapCreater.totalCol + (int)transform.position.y))
                {
                    stat = Stat.guard;
                }
            }
            moveToTarget();
        }

        void guard(HashSet<int> canGo)
        {
            Vector3 endPos = GameManager.maze.GetChild(guardPoint[nextGrardNum]).position;
            int[] endRow = new int[1] { (int)endPos.x }, endCol = new int[1] { (int)endPos.y };
            setNavigateTarget(endRow, endCol, canGo);
        }
    }
}