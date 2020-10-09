using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCreater : MonoBehaviour
{
    /// <summary> 單層地牢的房間行列數 </summary>
    int roomRowNum = 4, roomColNum = 4;
    void Start()
    {
        mainRoadDecide();
    }

    void Update()
    {
        
    }

    /// <summary> 決定基本路徑，從上方隨機一格開始隨機向下移動(每層都至少要橫向移動一次才能往下) </summary>
    void mainRoadDecide()
    {
        //0:未計算，1:左，2:右，3:下
        int[,] moveway = new int[roomRowNum, roomColNum];
        //決定起始房間
        int currentRow = 0, currentCol = Random.Range(0, 4);
        //紀錄橫向移動了多少次
        int sameRowNum = 0;
        int lastMoveDir = 0;
        do
        {
            if (sameRowNum == 0)
            {
                if (currentCol == 0)
                {
                    lastMoveDir = 2;
                }
                else if (currentCol == roomColNum - 1)
                {
                    lastMoveDir = 1;
                }
                else
                {
                    lastMoveDir = Random.Range(1, 3);
                }
                sameRowNum = 1;
            }
            else
            {
                if ((currentCol == 0 && lastMoveDir == 1) || (currentCol == roomColNum - 1 && lastMoveDir == 2))
                {
                    lastMoveDir = 3;
                    sameRowNum = 0;
                }
                else
                {
                    if (currentRow < roomRowNum - 1)
                    {
                        int r = Random.Range(0, 2);
                        if (r < 1)
                        {
                            lastMoveDir = 3;
                            sameRowNum = 0;
                        }
                    }
                }
            }
            moveway[currentRow, currentCol] = lastMoveDir;
            if (lastMoveDir == 1)
            {
                currentCol--;
            }
            else if (lastMoveDir == 2)
            {
                currentCol++;
            }
            else if (lastMoveDir == 3)
            {
                currentRow++;
            }
        } while (!(currentRow >= roomRowNum - 1 && (currentCol <= 0 || currentCol >= roomColNum - 1)));
    }
}
