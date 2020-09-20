﻿using UnityEngine;
using System.Collections;


namespace com.DungeonPad
{
    public class WallCreater : MonoBehaviour
    {
        public int totalRow, totalCol;
        public static bool[,] mapArray;
        public static WallCreater wallCreater;


        void Awake()
        {
            totalRow = MapCreater.totalRow[MapCreater.level];
            totalCol = MapCreater.totalCol[MapCreater.level];
            wallCreater = this;
            reMap();
        }

        /// <summary> 重新製作牆壁 </summary>
        public void reMap()
        {
            mapArray = InitMapArray();
            for (int i = 0; i < 7; i++)
            {
                mapArray = SmoothMapArray(mapArray);
            }
        }


        /// <summary> 隨機生成牆壁資料庫 </summary>
        bool[,] InitMapArray()
        {
            int i, j;
            bool[,] array = new bool[totalRow, totalCol];
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    //60%機率true(路)反之false(牆)
                    array[i, j] = Random.Range(0, 100) < 60;
                    //外壁固定為牆
                    if (i == 0 || i == totalRow - 1 || j == 0 || j == totalCol - 1)
                    {
                        array[i, j] = false;
                    }
                }
            }
            return array;
        }


        /// <summary> 潤飾，讓隨機數據符合周圍地圖 </summary>
        bool[,] SmoothMapArray(bool[,] array)
        {
            int i, j;
            bool[,] newArray = new bool[totalRow, totalCol];
            int count1, count2;
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    count1 = CheckNeighborWalls(array, i, j, 1);
                    count2 = CheckNeighborWalls(array, i, j, 2);

                    if (count1 >= 5 || count2 <= 2)
                    {
                        newArray[i, j] = false;
                    }
                    else
                    {
                        newArray[i, j] = true;
                    }

                    if (i == 0 || i == totalRow - 1 || j == 0 || j == totalCol - 1)
                    {
                        newArray[i, j] = false;
                    }
                }
            }
            return newArray;
        }


        /// <summary> 查看點周圍的牆壁數，t圍距離圈(t=1九宮格，t=2二十五宮格) </summary>
        int CheckNeighborWalls(bool[,] array, int i, int j, int t)
        {
            int _i, _j;
            int count = 0;
            for (_i = i - t; _i < i + t + 1; _i++)
            {
                for (_j = j - t; _j < j + t + 1; _j++)
                {
                    if (_i > 0 && _i < totalRow && _j >= 0 && _j < totalCol)
                    {
                        if (!array[_i, _j])
                        {
                            count++;
                        }
                    }
                }
            }
            if (!array[i, j])
                count--;
            return count;
        }
    }
}