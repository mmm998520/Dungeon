using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeCreater : MonoBehaviour
{
    /// <summary> 單層地牢的房間行列數 </summary>
    const int roomRowNum = 4, roomColNum = 4;
    /// <summary> 遍歷房間基本形式用，使用前須將他複製為list，0 ┼，1 ├，2 ┬，3 ┤，4 ┴，5 └，6 ┌，7 ┐，8 ┘，9 │，10 ─ </summary>
    int[] roomTypesArray = new int[11] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    void Start()
    {
        mainRoadsePasswaySelect(mainRoadDecide());
    }

    void Update()
    {

    }

    /// <summary> 決定基本路徑，從上方隨機一格開始隨機向下移動(每層都至少要橫向移動一次才能往下) </summary>
    int[,] mainRoadDecide()
    {
        //0:未計算，1:左，2:右，3:下，4:終點
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
        } while (!(currentRow >= roomRowNum - 1 && (currentCol <= 0 || currentCol >= roomColNum - 1) && sameRowNum == 1));
        moveway[currentRow, currentCol] = 4;
        return moveway;
    }

    void mainRoadsePasswaySelect(int[,] mainRoadDecide)
    {
        //第三維紀錄房間四邊是否"必須"/"絕不能"有牆，0為上方牆壁，順時針遞增
        bool[,,] mustRoad = new bool[roomRowNum, roomColNum, 4], mustNotRoad = new bool[roomRowNum, roomColNum, 4];
        int t, i, j;
        for (i = 0; i < roomRowNum; i++)
        {
            for (j = 0; j < roomColNum; j++)
            {
                //檢查直向是否有絕對通路
                if (mainRoadDecide[i, j] == 3)
                {
                    mustRoad[i, j, 2] = true;
                    mustRoad[i + 1, j, 0] = true;
                }

                //檢查直向是否有絕對死路
                else if (i < roomRowNum - 1)
                {
                    if (mainRoadDecide[i, j] != 0 && mainRoadDecide[i + 1, j] != 0)
                    {
                        mustNotRoad[i, j, 2] = true;
                        mustNotRoad[i + 1, j, 0] = true;
                    }
                }

                //檢查橫向是否有絕對通路
                if (mainRoadDecide[i, j] == 1)
                {
                    mustRoad[i, j, 3] = true;
                    mustRoad[i, j - 1, 1] = true;
                }
                else if (mainRoadDecide[i, j] == 2)
                {
                    mustRoad[i, j, 1] = true;
                    mustRoad[i, j + 1, 3] = true;
                }

                //讓終點上方變成絕對死路
                else if (mainRoadDecide[i, j] == 4)
                {
                    mustNotRoad[i, j, 0] = true;
                    mustNotRoad[i - 1, j, 0] = true;
                }
            }
        }


        List<int> roomTypesList;
        int[,] roomtype = new int[roomRowNum,roomColNum];
        //計算通路
        for (i = 0; i < roomRowNum; i++)
        {
            for (j = 0; j < roomColNum; j++)
            {
                roomTypesList = this.roomTypesArray.ToList();
                #region//移除不符規定的通路選擇
                roomTypesList.Remove(0);
                //移除沒上方的
                if (mustRoad[i, j, 0])
                {
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(6);
                    roomTypesList.Remove(7);
                    roomTypesList.Remove(10);
                }
                //移除有上方的
                else if (mustNotRoad[i, j, 0])
                {
                    roomTypesList.Remove(0);
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(5);
                    roomTypesList.Remove(8);
                    roomTypesList.Remove(9);
                }
                //移除沒右方的
                if (mustRoad[i, j, 1])
                {
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(7);
                    roomTypesList.Remove(8);
                    roomTypesList.Remove(9);
                }
                //移除有右方的
                else if (mustNotRoad[i, j, 1])
                {
                    roomTypesList.Remove(0);
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(5);
                    roomTypesList.Remove(6);
                    roomTypesList.Remove(10);
                }
                //移除沒下方的
                if (mustRoad[i, j, 2])
                {
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(5);
                    roomTypesList.Remove(8);
                    roomTypesList.Remove(10);
                }
                //移除有下方的
                else if (mustNotRoad[i, j, 2])
                {
                    roomTypesList.Remove(0);
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(6);
                    roomTypesList.Remove(7);
                    roomTypesList.Remove(9);
                }
                //移除沒左方的
                if (mustRoad[i, j, 3])
                {
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(5);
                    roomTypesList.Remove(6);
                    roomTypesList.Remove(9);
                }
                //移除有左方的
                else if (mustNotRoad[i, j, 3])
                {
                    roomTypesList.Remove(0);
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(7);
                    roomTypesList.Remove(8);
                    roomTypesList.Remove(10);
                }
                #endregion
                //從剩餘選項中隨機挑出一個通道型態
                roomtype[i,j] = roomTypesList[Random.Range(0, roomTypesList.Count)];
            }
        }

        //強制連結支路徑
        bool[,] canGo = new bool[roomRowNum, roomColNum];
        int canGoFalseNum = canGo.Length;
        for (j = 0; j < roomColNum; j++)
        {
            if (mainRoadDecide[roomRowNum - 1, j] == 4)
            {
                canGo[roomRowNum - 1, j] = true;
                canGoFalseNum--;
            }
        }
        //檢查不能通行路徑並強迫開路
        do
        {
            //檢查路是否相通，t == 0時代表檢查完畢
            do
            {
                t = 0;
                for (i = roomRowNum - 1; i >= 0; i--)
                {
                    for (j = roomColNum - 1; j >= 0; j--)
                    {
                        if (canGo[i, j])
                        {

                            #region//看四周房間是否相通
                            //看上方是否有房間
                            if (i - 1 >= 0)
                            {
                                //上方房間是否已被計算過
                                if (canGo[i - 1, j] == false)
                                {
                                    //看上方是否有通道
                                    if (roomtype[i, j] == 0 || roomtype[i, j] == 1 || roomtype[i, j] == 3 || roomtype[i, j] == 4 || roomtype[i, j] == 5 || roomtype[i, j] == 8 || roomtype[i, j] == 9)
                                    {
                                        //看上方的房間是否有下方通道
                                        if (roomtype[i - 1, j] == 0 || roomtype[i - 1, j] == 1 || roomtype[i - 1, j] == 2 || roomtype[i - 1, j] == 3 || roomtype[i - 1, j] == 6 || roomtype[i - 1, j] == 7 || roomtype[i - 1, j] == 9)
                                        {
                                            //確認連接成功
                                            canGo[i - 1, j] = true;
                                            t++;
                                            canGoFalseNum--;
                                        }
                                    }
                                }
                            }
                            //看右方是否有房間
                            if (j + 1 < roomColNum)
                            {
                                //右方房間是否已被計算過
                                if (canGo[i, j + 1] == false)
                                {
                                    //看右方是否有通道
                                    if (roomtype[i, j] == 0 || roomtype[i, j] == 1 || roomtype[i, j] == 2 || roomtype[i, j] == 4 || roomtype[i, j] == 5 || roomtype[i, j] == 6 || roomtype[i, j] == 10)
                                    {
                                        //看右方的房間是否有左方通道
                                        if (roomtype[i, j + 1] == 0 || roomtype[i, j + 1] == 2 || roomtype[i, j + 1] == 3 || roomtype[i, j + 1] == 4 || roomtype[i, j + 1] == 7 || roomtype[i, j + 1] == 8 || roomtype[i, j + 1] == 10)
                                        {
                                            //確認連接成功
                                            canGo[i, j + 1] = true;
                                            t++;
                                            canGoFalseNum--;
                                        }
                                    }
                                }
                            }
                            //看下方是否有房間
                            if (i + 1 < roomRowNum)
                            {
                                //下方房間是否已被計算過
                                if (canGo[i + 1, j] == false)
                                {
                                    //看下方是否有通道
                                    if (roomtype[i, j] == 0 || roomtype[i, j] == 1 || roomtype[i, j] == 2 || roomtype[i, j] == 3 || roomtype[i, j] == 6 || roomtype[i, j] == 7 || roomtype[i, j] == 9)
                                    {
                                        //看下方的房間是否有上方通道
                                        if (roomtype[i + 1, j] == 0 || roomtype[i + 1, j] == 1 || roomtype[i + 1, j] == 3 || roomtype[i + 1, j] == 4 || roomtype[i + 1, j] == 5 || roomtype[i + 1, j] == 8 || roomtype[i + 1, j] == 9)
                                        {
                                            //確認連接成功
                                            canGo[i + 1, j] = true;
                                            t++;
                                            canGoFalseNum--;
                                        }
                                    }
                                }
                            }
                            //看左方是否有房間
                            if (j - 1 >= 0)
                            {
                                //左方房間是否已被計算過
                                if (canGo[i, j - 1] == false)
                                {
                                    //看左方是否有通道
                                    if (roomtype[i, j] == 0 || roomtype[i, j] == 2 || roomtype[i, j] == 3 || roomtype[i, j] == 4 || roomtype[i, j] == 7 || roomtype[i, j] == 8 || roomtype[i, j] == 10)
                                    {
                                        //看左方的房間是否有右方通道
                                        if (roomtype[i, j - 1] == 0 || roomtype[i, j - 1] == 1 || roomtype[i, j - 1] == 2 || roomtype[i, j - 1] == 4 || roomtype[i, j - 1] == 5 || roomtype[i, j - 1] == 6 || roomtype[i, j - 1] == 10)
                                        {
                                            //確認連接成功
                                            canGo[i, j - 1] = true;
                                            t++;
                                            canGoFalseNum--;
                                        }
                                    }
                                }
                            }
                            #endregion

                        }
                    }
                }
            } while (t != 0);
            //強迫開路
            List<int[]> needBoom = new List<int[]>();
            for (i = 0; i < roomRowNum; i++)
            {
                for (j = 0; j < roomColNum; j++)
                {
                    if (!canGo[i, j])
                    {
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        ///原本安插在這可用，但沒有隨機炸通道
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        //////////////////////////////////////////////////////////////////////////////////////////////
                        needBoom.Add(new int[] { i, j });
                    }
                }
            }
            if (needBoom.Count > 0)
            {
                int r = Random.Range(0, needBoom.Count);
                int[] temp = needBoom[r];
                print(temp);
                i = needBoom[r][0];
                j = needBoom[r][1];
                print(i);
                print(j);
                //強制隨機出非邊框數值
                do
                {
                    r = Random.Range(0, 4);
                } while ((r == 0 && i <= 0) || (r == 1 && j >= roomColNum - 1) || (r == 2 && i >= roomRowNum - 1) || (r == 3 && j <= 0));
                //如果新通道為向上，則強迫開啟自己的上方、上方房間的下方
                if (r == 0)
                {
                    //自己的上方
                    switch (roomtype[i, j])
                    {
                        case 2:
                            roomtype[i, j] = 0;
                            break;
                        case 6:
                            roomtype[i, j] = 1;
                            break;
                        case 7:
                            roomtype[i, j] = 3;
                            break;
                        case 10:
                            roomtype[i, j] = 4;
                            break;
                    }
                    //上方房間的下方
                    switch (roomtype[i - 1, j])
                    {
                        case 4:
                            roomtype[i - 1, j] = 0;
                            break;
                        case 5:
                            roomtype[i - 1, j] = 1;
                            break;
                        case 8:
                            roomtype[i - 1, j] = 3;
                            break;
                        case 10:
                            roomtype[i - 1, j] = 2;
                            break;
                    }
                }
                //如果新通道為向右，則強迫開啟自己的右方、右方房間的左方
                if (r == 1)
                {
                    //自己的右方
                    switch (roomtype[i, j])
                    {
                        case 3:
                            roomtype[i, j] = 0;
                            break;
                        case 7:
                            roomtype[i, j] = 2;
                            break;
                        case 8:
                            roomtype[i, j] = 3;
                            break;
                        case 9:
                            roomtype[i, j] = 1;
                            break;
                    }
                    //右方房間的左方
                    switch (roomtype[i, j + 1])
                    {
                        case 1:
                            roomtype[i, j + 1] = 0;
                            break;
                        case 5:
                            roomtype[i, j + 1] = 4;
                            break;
                        case 6:
                            roomtype[i, j + 1] = 2;
                            break;
                        case 9:
                            roomtype[i, j + 1] = 3;
                            break;
                    }
                }
                //如果新通道為向下，則強迫開啟自己的下方、下方房間的上方
                if (r == 2)
                {
                    //自己的下方
                    switch (roomtype[i, j])
                    {
                        case 4:
                            roomtype[i, j] = 0;
                            break;
                        case 5:
                            roomtype[i, j] = 1;
                            break;
                        case 8:
                            roomtype[i, j] = 3;
                            break;
                        case 10:
                            roomtype[i, j] = 2;
                            break;
                    }
                    //下方房間的上方
                    switch (roomtype[i + 1, j])
                    {
                        case 2:
                            roomtype[i + 1, j] = 0;
                            break;
                        case 6:
                            roomtype[i + 1, j] = 1;
                            break;
                        case 7:
                            roomtype[i + 1, j] = 3;
                            break;
                        case 10:
                            roomtype[i + 1, j] = 4;
                            break;
                    }
                }
                //如果新通道為向左，則強迫開啟自己的左方、左方房間的右方
                if (r == 3)
                {
                    //自己的左方
                    switch (roomtype[i, j])
                    {
                        case 1:
                            roomtype[i, j] = 0;
                            break;
                        case 5:
                            roomtype[i, j] = 4;
                            break;
                        case 6:
                            roomtype[i, j] = 2;
                            break;
                        case 9:
                            roomtype[i, j] = 1;
                            break;
                    }
                    //左方房間的右方
                    switch (roomtype[i, j - 1])
                    {
                        case 3:
                            roomtype[i, j - 1] = 0;
                            break;
                        case 7:
                            roomtype[i, j - 1] = 2;
                            break;
                        case 8:
                            roomtype[i, j - 1] = 4;
                            break;
                        case 9:
                            roomtype[i, j - 1] = 1;
                            break;
                    }
                }
            }
        } while (canGoFalseNum != 0);

        for (i = 0; i < roomRowNum; i++)
        {
            string a = "";
            for (j = 0; j < roomColNum; j++)
            {
                switch (roomtype[i, j])
                {
                    case 0:
                        a = a + "┼" + ",";
                        break;
                    case 1:
                        a = a + "├" + ",";
                        break;
                    case 2:
                        a = a + "┬" + ",";
                        break;
                    case 3:
                        a = a + "┤" + ",";
                        break;
                    case 4:
                        a = a + "┴" + ",";
                        break;
                    case 5:
                        a = a + "└" + ",";
                        break;
                    case 6:
                        a = a + "┌" + ",";
                        break;
                    case 7:
                        a = a + "┐" + ",";
                        break;
                    case 8:
                        a = a + "┘" + ",";
                        break;
                    case 9:
                        a = a + "│" + ",";
                        break;
                    case 10:
                        a = a + "─" + ",";
                        break;
                }
            }
            Debug.LogWarning(a);
        }

    }
}
