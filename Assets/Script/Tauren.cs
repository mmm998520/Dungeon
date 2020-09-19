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
                guard(greenPos);
            }
            else if(stat == Stat.pursue)
            {
                target = MinDisPlayer();
            }
            else if(stat == Stat.back)
            {
                guard(orangePos);
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