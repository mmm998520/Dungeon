using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dungeon
{
    public class NavigationManager : MonoBehaviour
    {
        /// <summary> 定義斜線、直線距離 </summary>
        const float Straight = 1, hypotenuse = 1.414f;
        int totalRow = MapCreater.totalRow, totalCol = MapCreater.totalCol;
        int startRow, startCol, endRow, endCol;
        /// <summary> g : 起點到該點的移動量，h : 該點到終點的估算移動量(採對角線距離計算)，f : 合值 </summary>
        float[,] g, h, f;
        /// <summary> 
        /// 0 : 未抵達點，1 (open) : 已抵達點可以計算可能性，2 (close) : 已計算過點不需重複計算
        /// <para> 輸入數值為currentRow*totalCol+currentCol </para>
        /// <para> HashSet優點 : 快速查找 </para>
        /// </summary>
        HashSet<int> open = new HashSet<int>(), close = new HashSet<int>();

        /// <summary> 尋路 </summary>
        void FindRoad()
        {
            //初始設置
            int i = 0, j = 0, t =0;
            for(i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    g[i, j] = 1000000;
                    h[i, j] = 1000000;
                    f[i, j] = 2000000;
                    //如果該點是牆，則不考慮經過他
                    if(MapCreater.mapArray[i,j] == (int)MapCreater.roomStat.wall)
                    {
                        close.Add(i * totalCol + j);
                    }
                }
            }
            g[startRow, startCol] = 0;
            h[startRow, startCol] = findDiagonalDistance(Mathf.Abs(startRow - endRow), Mathf.Abs(startCol - endCol));
            f[startRow, startCol] = g[startRow, startCol] + h[startRow, startCol];
            open.Add(startRow * totalCol + startCol);

            //開始找路
            int minFPos = -1;
            float minF = float.MaxValue;
            t = 0;
            do
            {
                if (t++ > 20000)
                {
                    Debug.LogError("Why!!!!!!");
                    break;
                }
                //當open包含終點則找到終點
                if (open.Contains(endRow * totalCol + endCol))
                {
                    break;
                }

                //找值最小的open點，將它做為新的當前點，並close它
                foreach (int a in open)
                {
                    if (minF > f[a / totalCol, a % totalCol])
                    {
                        minF = f[a / totalCol, a % totalCol];
                        minFPos = a;
                    }
                }
                open.Remove(minFPos);
                close.Add(minFPos);

                //從當前點往8方未確認可通行點，並將它們加入open
                open = checkCanGo(minFPos, open, close);
            } while (false);
        }


        /// <summary> 確認該點附近那些點可以移動，可以的話就計算GHF </summary>
        HashSet<int> checkCanGo(int currentPos, HashSet<int> open, HashSet<int> close)
        {
            bool up = !close.Contains(currentPos + 1);
            bool down = !close.Contains(currentPos - 1);
            bool right = !close.Contains(currentPos + totalCol);
            bool left = !close.Contains(currentPos - totalCol);
            bool upRight = !close.Contains(currentPos + 1 + totalCol);
            bool upLeft = !close.Contains(currentPos + 1 - totalCol);
            bool downRight = !close.Contains(currentPos - 1 + totalCol);
            bool downLeft = !close.Contains(currentPos - 1 - totalCol);
            if (up)
            {
                countGHF(currentPos, currentPos + 1, Straight);
                open.Add(currentPos + 1);
                if(right && upRight)
                {
                    countGHF(currentPos, currentPos + 1 + totalCol, hypotenuse);
                    open.Add(currentPos + 1 + totalCol);
                }
                if (left && upLeft)
                {
                    countGHF(currentPos, currentPos + 1 - totalCol, hypotenuse);
                    open.Add(currentPos + 1 - totalCol);
                }
            }
            if (down)
            {
                countGHF(currentPos, currentPos - 1, Straight);
                open.Add(currentPos - 1);
                if (right && downRight)
                {
                    countGHF(currentPos, currentPos - 1 + totalCol, hypotenuse);
                    open.Add(currentPos - 1 + totalCol);
                }
                if (left && downLeft)
                {
                    countGHF(currentPos, currentPos - 1 - totalCol, hypotenuse);
                    open.Add(currentPos - 1 - totalCol);
                }
            }
            if (right)
            {
                countGHF(currentPos, currentPos + totalCol, Straight);
                open.Add(currentPos + totalCol);
            }
            if (left)
            {
                countGHF(currentPos, currentPos - totalCol, Straight);
                open.Add(currentPos - totalCol);
            }
            return open;
        }


        /// <summary> 計算該點的GHF </summary>
        void countGHF(int currentPos, int nextPos, float dis)
        {
            int currentRow = currentPos / totalCol, currentCol = currentPos % totalCol, nextRow = nextPos / totalCol, nextCol = nextPos % totalCol;
            if(g[nextRow, nextCol]> g[currentRow, currentCol] + dis)
            {
                g[nextRow, nextCol] = g[currentRow, currentCol] + dis;
                h[nextRow, nextCol] = findDiagonalDistance(Mathf.Abs(nextRow - endRow), Mathf.Abs(nextCol - endCol));
                f[nextRow, nextCol] = g[nextRow, nextCol] + h[nextRow, nextCol];
            }
        }


        /// <summary> 給直橫距離，計算對角線距離的公式 </summary>
        float findDiagonalDistance(int distanceRow, int distanceCol)
        {
            return Mathf.Abs(distanceRow - distanceCol) + (Mathf.Min(distanceRow, distanceRow) * hypotenuse);
        }
    }
}