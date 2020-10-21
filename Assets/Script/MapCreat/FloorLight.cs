using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FloorLight : MonoBehaviour
    {
        public string[,] mazeDatas;
        public int totalRow, totalCol;
        List<int[]> side = new List<int[]>();
        void Start()
        {
            mazeDatas = GameManager.mazeCreater.mazeDatas;
            totalRow = GameManager.mazeCreater.roomCountRowNum * GameManager.mazeCreater.objectCountRowNum;
            totalCol = GameManager.mazeCreater.roomCountColNum * GameManager.mazeCreater.objectCountColNum;
            side.Add(new int[] { -1, 0 });
            side.Add(new int[] { 1, 0 });
            side.Add(new int[] { 0, -1 });
            side.Add(new int[] { 0, 1 });
        }

        void Update()
        {

        }

        int[] findNewInsPos()
        {
            int i, row, col, r, tempRow, tempCol;
            List<int[]> side = this.side;
            bool tempIsWall = true;
            do
            {
                do
                {
                    row = Random.Range(0, totalRow);
                    col = Random.Range(0, totalCol);
                } while (mazeDatas[row, col] == "wall" || mazeDatas[row, col] == "" || mazeDatas[row, col] == null);

                for(i = 0; i < 4; i++)
                {
                    r = Random.Range(0, side.Count);
                    tempRow = row + side[r][0];
                    tempCol = col + side[r][1];
                    tempIsWall = mazeDatas[tempRow, tempCol] == "wall" || mazeDatas[tempRow, tempCol] == "" || mazeDatas[tempRow, tempCol] == null;
                    side.RemoveAt(r);
                    if (!tempIsWall)
                    {
                        return new int[] { row, col, tempRow, tempCol };
                    }
                }
            } while (true);
        }
    }
}