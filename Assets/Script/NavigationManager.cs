using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class NavigationManager : MonoBehaviour
    {
        int totalRow = MapCreater.totalRow, totalCol = MapCreater.totalCol;
        int startRow, startCol;
        int[] endRow, endCol;
        /// <summary> 導航後得出，到終點時下一個要到的點 </summary>
        int[] nextPos = new int[MapCreater.totalRow * MapCreater.totalCol];
        /// <summary> g : 起點到該點的移動量，h : 該點到終點的估算移動量(採對角線距離計算)，f : 合值 </summary>
        float[,] g = new float[MapCreater.totalRow, MapCreater.totalCol], h = new float[MapCreater.totalRow, MapCreater.totalCol], f = new float[MapCreater.totalRow, MapCreater.totalCol];
        /// <summary> 定義斜線、直線距離 </summary>
        const float Straight = 1, hypotenuse = 1.414f;
        /// <summary> 
        /// 0 : 未抵達點，1 (open) : 已抵達點可以計算可能性，2 (close) : 已計算過點不需重複計算
        /// <para> 輸入數值為currentRow*totalCol+currentCol </para>
        /// <para> HashSet優點 : 快速查找 </para>
        /// </summary>
        HashSet<int> open = new HashSet<int>(), close = new HashSet<int>(), hardToGo = new HashSet<int>();
        /// <summary> 紀錄map同資料的各點 </summary>
        public static HashSet<int> wallPos = new HashSet<int>(), greenPos = new HashSet<int>(), blackPos = new HashSet<int>(), bluePos = new HashSet<int>(), orangePos = new HashSet<int>();

        public static void setHashSet(int[,] mapArray)
        {
            int i, j, t = 0;
            for (i = 0; i < MapCreater.totalRow; i++)
            {
                for (j = 0; j < MapCreater.totalCol; j++)
                {
                    switch(mapArray[i, j])
                    {
                        case (int)MapCreater.roomStat.wall:
                            wallPos.Add(t);
                            break;
                        case (int)MapCreater.roomStat.green:
                            greenPos.Add(t);
                            break;
                        case (int)MapCreater.roomStat.black:
                            blackPos.Add(t);
                            break;
                        case (int)MapCreater.roomStat.blue:
                            bluePos.Add(t);
                            break;
                        case (int)MapCreater.roomStat.orange:
                            orangePos.Add(t);
                            break;
                    }
                    t++;
                }
            }
        }

        /// <summary> 尋路，return下一個路徑點 </summary>
        protected int FindRoad(int _startRow, int _startCol, int[] _endRow, int[] _endCol, HashSet<int> canGo)
        {
            //初始設置
            startRow = _startRow;
            startCol = _startCol;
            endRow = _endRow;
            endCol = _endCol;
            open = new HashSet<int>();
            close = new HashSet<int>();
            hardToGo = new HashSet<int>();
            int i, j, t =0;
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    g[i, j] = 1000000;
                    h[i, j] = 1000000;
                    f[i, j] = 2000000;
                    if (wallPos.Contains(t))
                    {
                        close.Add(t);
                    }
                    if (!canGo.Contains(t))
                    {
                        hardToGo.Add(t);
                    }
                    t++;
                }
            }
            g[startRow, startCol] = 0;
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
                    Debug.LogError("我猜你不在自己的範圍內，讓canGo變全部區域試試");
                    return startRow * totalCol + startCol;
                }
                //當open包含終點則找到終點
                for(i = 0; i < endRow.Length; i++)
                {
                    if (open.Contains(endRow[i] * totalCol + endCol[i]))
                    {
                        foreach(int pos in open)
                        {
                            if(pos == endRow[i] * totalCol + endCol[i])
                            {
                                return nextPos[pos];
                            }
                        }
                    }
                }

                //找值最小的open點，將它做為新的當前點，並close它
                minFPos = -1;
                minF = float.MaxValue;
                foreach (int pos in open)
                {
                    if (minF > f[pos / totalCol, pos % totalCol])
                    {
                        minF = f[pos / totalCol, pos % totalCol];
                        minFPos = pos;
                    }
                }
                print(open.Count);
                open.Remove(minFPos);
                print(open.Count);
                close.Add(minFPos);

                //從當前點往8方未確認可通行點，並將它們加入open
                open = checkCanGo(minFPos, open, close);
            } while (true);
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
            if (hardToGo.Contains(nextPos))
            {
                dis *= 10;
            }
            if(g[nextRow, nextCol]> g[currentRow, currentCol] + dis)
            {
                g[nextRow, nextCol] = g[currentRow, currentCol] + dis;
                float minDis = float.MaxValue;
                for (int i = 0; i < endRow.Length; i++)
                {
                    float temp = findDiagonalDistance(Mathf.Abs(nextRow - endRow[i]), Mathf.Abs(nextCol - endCol[i]));
                    if (minDis > temp)
                    {
                        minDis = temp;
                    }
                }
                h[nextRow, nextCol] = minDis;
                f[nextRow, nextCol] = g[nextRow, nextCol] + h[nextRow, nextCol];
                if(currentPos == startRow * totalCol + startCol)
                {
                    this.nextPos[nextPos] = nextPos;
                    this.nextPos[currentPos] = currentPos;
                }
                else
                {
                    this.nextPos[nextPos] = this.nextPos[currentPos];
                }
            }
        }


        /// <summary> 給直橫距離，計算對角線距離的公式 </summary>
        float findDiagonalDistance(int distanceRow, int distanceCol)
        {
            return Mathf.Abs(distanceRow - distanceCol) + (Mathf.Min(distanceRow, distanceRow) * hypotenuse);
        }
    }
}