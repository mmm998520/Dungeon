using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dungeon
{
    public class MonsterManager : NavigationManager
    {
        public GameObject end;
        void Update()
        {
            if (Input.anyKeyDown)
            {
                int[] a = new int[1] { (int)end.transform.position.x }, b = new int[1] { (int)end.transform.position.y };
                int pos = FindRoad((int)transform.position.x, (int)transform.position.y, a, b, orangePos);
                print(pos / MapCreater.totalCol + "," + pos % MapCreater.totalCol);
            }
        }
    }
}