using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    /// <summary>
    /// 迷宮生成類
    /// 迷宮生成思路：
    /// 規則:二維陣列中奇數行列視為圍牆，偶數為路徑
    /// 從起始點開始，隨機從上下左右四個位置尋找周圍沒有被找到過的位置，找到後此點標記為1，並把此點與前一點之間的位置設定為1
    /// 如果全部位置已經找到過，則退回到上一個點重複次邏輯，直到所有點都記錄到或 退回到起始點且沒有可用點
    /// </summary>
    public class MazeCreate : MonoBehaviour
    {

        public enum PointType
        {
            wall = 0,//牆
            way = 1,//路
            startpoint = 2,//起始點
        }

        //行數
        public int row { get; private set; }
        //列數
        public int col { get; private set; }
        //全部點數量
        int maxcount;

        private MazeCreate(int row, int col)
        {
            this.row = row;
            this.col = col;
            Start();
        }

        public static MazeCreate GetMaze(int row, int col)
        {
            MazeCreate maze = new MazeCreate(row, col);
            return maze;
        }

        /// <summary> 迷宮生成過程中，記錄已經找到的點 </summary>
        public List<int> findList = new List<int>();

        //迷宮資料
        public List<List<int>> mapList = new List<List<int>>();

        void Start()
        {
            maxcount = row * col;

            //初始化陣列
            for (int i = 0; i < row; i++)
            {
                mapList.Add(new List<int>());
                for (int j = 0; j < col; j++)
                {
                    mapList[i].Add((int)PointType.wall);
                }
            }

            //起始點
            int _row = Random.Range(1, row - 1);
            int _col = Random.Range(1, col - 1);
            if (_row % 2 == 0) { _row += 1; }
            if (_col % 2 == 0) { _col += 1; }

            mapList[_row][_col] = (int)PointType.startpoint;

            int nowindex = _row * col + _col;
            findList.Add(nowindex);

            //遞迴生成路徑
            FindPoint(nowindex);
        }

        void FindPoint(int nowindex)
        {
            if (findList.Count >= maxcount)
            {
                return;
            }

            List<int> nearpoint = new List<int>();
            FindNearPoint(nearpoint, nowindex);
            while (nearpoint.Count > 0)
            {
                int rand = Random.Range(0, nearpoint.Count);

                //中間的格子
                int midindex = nowindex + (nearpoint[rand] - nowindex) / 2;
                SetPoint(midindex);

                //新的格子
                int newindex = nearpoint[rand];
                SetPoint(newindex);
                nearpoint.RemoveAt(rand);
                //遞迴
                FindPoint(newindex);

                FindNearPoint(nearpoint, nowindex);
            }
        }

        //尋找附近可用的點
        void FindNearPoint(List<int> nearpoint, int index)
        {
            nearpoint.Clear();
            int _row = index / col;
            int _col = index % col;
            //up
            if (_row >= 2)
            {
                AddNearPoint(nearpoint, (_row - 2) * col + _col);
            }
            //down
            if (_row < row - 2)
            {
                AddNearPoint(nearpoint, (_row + 2) * col + _col);
            }
            //left
            if (_col >= 2)
            {
                AddNearPoint(nearpoint, _row * col + _col - 2);
            }
            //up
            if (_col < col - 2)
            {
                AddNearPoint(nearpoint, _row * col + _col + 2);
            }

        }

        //設定路徑
        void SetPoint(int index)
        {
            int _row = index / col;
            int _col = index % col;
            mapList[_row][_col] = (int)PointType.way;

            findList.Add(index);
        }

        //附近的點是否滿足尋找條件
        void AddNearPoint(List<int> nearpoint, int p)
        {
            int _row = p / col;
            int _col = p % col;

            if (p >= 0 && p < maxcount && mapList[_row][_col] == (int)PointType.wall)
            {
                nearpoint.Add(p);
            }
        }
    }
}