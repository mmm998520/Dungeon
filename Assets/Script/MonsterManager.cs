using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterManager : DirectionChanger
    {
        public float MaxHP,HP,ATK;
        /// <summary> 守備區域 </summary>
        protected HashSet<int> guardPos;
        /// <summary> 追擊區域 </summary>
        protected HashSet<int> pursuePos;
        /// <summary> 將怪物誕生區域設為守備區域並給予追擊範圍 </summary>
        void setCanGoPos()
        {
            //設置巡邏區域
            guardPos = new HashSet<int>();
            int i, j, t;
            MapCreater.roomStat roomStat;
            guardPos.Add((int)transform.position.x * MapCreater.totalCol + (int)transform.position.y);
            roomStat = (MapCreater.roomStat)MapCreater.mapArray[(int)transform.position.x, (int)transform.position.y];
            do
            {
                t = 0;
                for (i = 0; i < MapCreater.totalRow; i++)
                {
                    for (j = 0; j < MapCreater.totalCol; j++)
                    {
                        if (MapCreater.mapArray[i, j] == (int)roomStat && !guardPos.Contains(MapCreater.RowColToNum(i,j)))
                        {
                            if (guardPos.Contains(MapCreater.RowColToNum(i + 1, j)) || guardPos.Contains(MapCreater.RowColToNum(i - 1, j)) || guardPos.Contains(MapCreater.RowColToNum(i, j + 1)) || guardPos.Contains(MapCreater.RowColToNum(i, j - 1)))
                            {
                                guardPos.Add(MapCreater.RowColToNum(i, j));
                                t++;
                            }
                        }
                    }
                }
            } while (t > 0);
            //設置追擊區域
            pursuePos = new HashSet<int>();
            foreach(int num in guardPos)
            {
                int[] array = MapCreater.NumToRowCol(num);
                for (i = -3; i <= 3; i++)
                {
                    for(j = -3; j <= 3; j++)
                    {
                        if (array[0]+i >= 0 && array[0]+i < MapCreater.totalRow && array[1]+j >= 0 && array[1]+j < MapCreater.totalCol)
                        {
                            pursuePos.Add(MapCreater.RowColToNum(i, j));
                        }
                    }
                }
            }
        }

        protected Transform MinDisPlayer()
        {
            int i;
            float minDis = float.MaxValue;
            Transform minDisPlayer = null;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                if (minDis > Vector3.Distance(player.position, transform.position))
                {
                    minDis = Vector3.Distance(player.position, transform.position);
                    minDisPlayer = player;
                }
            }
            return minDisPlayer;
        }

        protected void moveToTarget()
        {
            changeDirection();
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
}