﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeCreater : MonoBehaviour
{
    /// <summary> 單層地牢的房間行列數 </summary>
    public int roomCountRowNum, roomCountColNum;
    /// <summary> 單個房間的物件行列數 </summary>
    public int objectCountRowNum, objectCountColNum;
    /// <summary> 遍歷房間基本形式用，使用前須將他複製為list，0 ┼，1 ├，2 ┬，3 ┤，4 ┴，5 ─，6 │ </summary>
    int[] roomPasswayTypes = new int[7] { 0, 1, 2, 3, 4, 5, 6 };
    /// <summary> 紀錄該層迷宮的房間通道類型，生成時須在該點生成對應房間 </summary>
    public int[,] roomPasswayDatas, mazeDatas;


    void Awake()
    {
        int[,] mainRoadDecide = this.mainRoadDecide();
        roomPasswayDatas = explodeRoad(RoadsePasswaySelect(mainRoadDecide), mainRoadDecide);
        for(int i = 0; i < Random.Range(0, 4); i++)
        {
            roomPasswayDatas = rotateDungeon(roomPasswayDatas);
        }
    }

    void Update()
    {

    }
    #region//決定基本房間通道
    /// <summary> 決定基本路徑，從上方隨機一格開始隨機向下移動(每層都至少要橫向移動一次才能往下) </summary>
    int[,] mainRoadDecide()
    {
        //0:未計算，1:左，2:右，3:下，4:終點
        int[,] moveway = new int[roomCountRowNum, roomCountColNum];
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
                else if (currentCol == roomCountColNum - 1)
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
                if ((currentCol == 0 && lastMoveDir == 1) || (currentCol == roomCountColNum - 1 && lastMoveDir == 2))
                {
                    lastMoveDir = 3;
                    sameRowNum = 0;
                }
                else
                {
                    if (currentRow < roomCountRowNum - 1)
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
        } while (!(currentRow >= roomCountRowNum - 1 && (currentCol <= 0 || currentCol >= roomCountColNum - 1) && sameRowNum == 1));
        moveway[currentRow, currentCol] = 4;
        return moveway;
    }

    /// <summary> 決定個房間路徑 </summary>
    int[,] RoadsePasswaySelect(int[,] mainRoadDecide)
    {
        //第三維紀錄房間四邊是否"必須"/"絕不能"有牆，0為上方牆壁，順時針遞增
        bool[,,] mustRoad = new bool[roomCountRowNum, roomCountColNum, 4], mustNotRoad = new bool[roomCountRowNum, roomCountColNum, 4];
        int t, i, j;
        for (i = 0; i < roomCountRowNum; i++)
        {
            for (j = 0; j < roomCountColNum; j++)
            {
                //檢查直向是否有絕對通路
                if (mainRoadDecide[i, j] == 3)
                {
                    mustRoad[i, j, 2] = true;
                    mustRoad[i + 1, j, 0] = true;
                }

                //檢查直向是否有絕對死路
                else if (i < roomCountRowNum - 1)
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
        int[,] roomtype = new int[roomCountRowNum,roomCountColNum];
        //計算通路
        for (i = 0; i < roomCountRowNum; i++)
        {
            for (j = 0; j < roomCountColNum; j++)
            {
                roomTypesList = this.roomPasswayTypes.ToList();
                #region//移除不符規定的通路選擇
                roomTypesList.Remove(0);
                //移除沒上方的
                if (mustRoad[i, j, 0])
                {
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(5);
                }
                //移除有上方的
                else if (mustNotRoad[i, j, 0])
                {
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(6);
                }
                //移除沒右方的
                if (mustRoad[i, j, 1])
                {
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(6);
                }
                //移除有右方的
                else if (mustNotRoad[i, j, 1])
                {
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(5);
                }
                //移除沒下方的
                if (mustRoad[i, j, 2])
                {
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(5);
                }
                //移除有下方的
                else if (mustNotRoad[i, j, 2])
                {
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(6);
                }
                //移除沒左方的
                if (mustRoad[i, j, 3])
                {
                    roomTypesList.Remove(1);
                    roomTypesList.Remove(6);
                }
                //移除有左方的
                else if (mustNotRoad[i, j, 3])
                {
                    roomTypesList.Remove(2);
                    roomTypesList.Remove(3);
                    roomTypesList.Remove(4);
                    roomTypesList.Remove(5);
                }
                #endregion
                //從剩餘選項中隨機挑出一個通道型態
                roomtype[i,j] = roomTypesList[Random.Range(0, roomTypesList.Count)];
            }
        }
        return roomtype;

    }

    /// <summary> 強制連結支路徑 </summary>
    int[,] explodeRoad(int[,] roomPasswayDatas, int[,] mainRoadDecide)
    {
        int i, j, t;
        bool[,] canGo = new bool[roomCountRowNum, roomCountColNum];
        int canGoFalseNum = canGo.Length;
        for (j = 0; j < roomCountColNum; j++)
        {
            if (mainRoadDecide[roomCountRowNum - 1, j] == 4)
            {
                canGo[roomCountRowNum - 1, j] = true;
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
                for (i = roomCountRowNum - 1; i >= 0; i--)
                {
                    for (j = roomCountColNum - 1; j >= 0; j--)
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
                                    if (roomPasswayDatas[i, j] == 0 || roomPasswayDatas[i, j] == 1 || roomPasswayDatas[i, j] == 3 || roomPasswayDatas[i, j] == 4 || roomPasswayDatas[i, j] == 6)
                                    {
                                        //看上方的房間是否有下方通道
                                        if (roomPasswayDatas[i - 1, j] == 0 || roomPasswayDatas[i - 1, j] == 1 || roomPasswayDatas[i - 1, j] == 2 || roomPasswayDatas[i - 1, j] == 3 || roomPasswayDatas[i - 1, j] == 6)
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
                            if (j + 1 < roomCountColNum)
                            {
                                //右方房間是否已被計算過
                                if (canGo[i, j + 1] == false)
                                {
                                    //看右方是否有通道
                                    if (roomPasswayDatas[i, j] == 0 || roomPasswayDatas[i, j] == 1 || roomPasswayDatas[i, j] == 2 || roomPasswayDatas[i, j] == 4 || roomPasswayDatas[i, j] == 5)
                                    {
                                        //看右方的房間是否有左方通道
                                        if (roomPasswayDatas[i, j + 1] == 0 || roomPasswayDatas[i, j + 1] == 2 || roomPasswayDatas[i, j + 1] == 3 || roomPasswayDatas[i, j + 1] == 4 || roomPasswayDatas[i, j + 1] == 5)
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
                            if (i + 1 < roomCountRowNum)
                            {
                                //下方房間是否已被計算過
                                if (canGo[i + 1, j] == false)
                                {
                                    //看下方是否有通道
                                    if (roomPasswayDatas[i, j] == 0 || roomPasswayDatas[i, j] == 1 || roomPasswayDatas[i, j] == 2 || roomPasswayDatas[i, j] == 3 || roomPasswayDatas[i, j] == 6)
                                    {
                                        //看下方的房間是否有上方通道
                                        if (roomPasswayDatas[i + 1, j] == 0 || roomPasswayDatas[i + 1, j] == 1 || roomPasswayDatas[i + 1, j] == 3 || roomPasswayDatas[i + 1, j] == 4 || roomPasswayDatas[i + 1, j] == 6)
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
                                    if (roomPasswayDatas[i, j] == 0 || roomPasswayDatas[i, j] == 2 || roomPasswayDatas[i, j] == 3 || roomPasswayDatas[i, j] == 4 || roomPasswayDatas[i, j] == 5)
                                    {
                                        //看左方的房間是否有右方通道
                                        if (roomPasswayDatas[i, j - 1] == 0 || roomPasswayDatas[i, j - 1] == 1 || roomPasswayDatas[i, j - 1] == 2 || roomPasswayDatas[i, j - 1] == 4 || roomPasswayDatas[i, j - 1] == 5)
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
            List<int[]> needExplode = new List<int[]>();
            for (i = 0; i < roomCountRowNum; i++)
            {
                for (j = 0; j < roomCountColNum; j++)
                {
                    if (!canGo[i, j])
                    {
                        needExplode.Add(new int[] { i, j });
                    }
                }
            }
            if (needExplode.Count > 0)
            {
                int r = Random.Range(0, needExplode.Count);
                int[] temp = needExplode[r];
                print(temp);
                i = needExplode[r][0];
                j = needExplode[r][1];
                print(i);
                print(j);
                //強制隨機出非邊框數值
                do
                {
                    r = Random.Range(0, 4);
                } while ((r == 0 && i <= 0) || (r == 1 && j >= roomCountColNum - 1) || (r == 2 && i >= roomCountRowNum - 1) || (r == 3 && j <= 0));
                //如果新通道為向上，則強迫開啟自己的上方、上方房間的下方
                if (r == 0)
                {
                    //自己的上方
                    switch (roomPasswayDatas[i, j])
                    {
                        case 2:
                            roomPasswayDatas[i, j] = 0;
                            break;
                        case 5:
                            roomPasswayDatas[i, j] = 4;
                            break;
                    }
                    //上方房間的下方
                    switch (roomPasswayDatas[i - 1, j])
                    {
                        case 4:
                            roomPasswayDatas[i - 1, j] = 0;
                            break;
                        case 5:
                            roomPasswayDatas[i - 1, j] = 2;
                            break;
                    }
                }
                //如果新通道為向右，則強迫開啟自己的右方、右方房間的左方
                if (r == 1)
                {
                    //自己的右方
                    switch (roomPasswayDatas[i, j])
                    {
                        case 3:
                            roomPasswayDatas[i, j] = 0;
                            break;
                        case 6:
                            roomPasswayDatas[i, j] = 1;
                            break;
                    }
                    //右方房間的左方
                    switch (roomPasswayDatas[i, j + 1])
                    {
                        case 1:
                            roomPasswayDatas[i, j + 1] = 0;
                            break;
                        case 6:
                            roomPasswayDatas[i, j + 1] = 3;
                            break;
                    }
                }
                //如果新通道為向下，則強迫開啟自己的下方、下方房間的上方
                if (r == 2)
                {
                    //自己的下方
                    switch (roomPasswayDatas[i, j])
                    {
                        case 4:
                            roomPasswayDatas[i, j] = 0;
                            break;
                        case 5:
                            roomPasswayDatas[i, j] = 2;
                            break;
                    }
                    //下方房間的上方
                    switch (roomPasswayDatas[i + 1, j])
                    {
                        case 2:
                            roomPasswayDatas[i + 1, j] = 0;
                            break;
                        case 5:
                            roomPasswayDatas[i + 1, j] = 4;
                            break;
                    }
                }
                //如果新通道為向左，則強迫開啟自己的左方、左方房間的右方
                if (r == 3)
                {
                    //自己的左方
                    switch (roomPasswayDatas[i, j])
                    {
                        case 1:
                            roomPasswayDatas[i, j] = 0;
                            break;
                        case 6:
                            roomPasswayDatas[i, j] = 3;
                            break;
                    }
                    //左方房間的右方
                    switch (roomPasswayDatas[i, j - 1])
                    {
                        case 3:
                            roomPasswayDatas[i, j - 1] = 0;
                            break;
                        case 6:
                            roomPasswayDatas[i, j - 1] = 1;
                            break;
                    }
                }
            }
        } while (canGoFalseNum != 0);
        return roomPasswayDatas;
    }

    int[,] rotateDungeon(int[,] roomPasswayDatas)
    {
        int[,] rotated = roomPasswayDatas;
        int i, j;
        for (i = 0; i < roomPasswayDatas.GetUpperBound(0) + 1; i++)
        {
            for (j = 0; j < roomPasswayDatas.GetUpperBound(1) + 1; j++)
            {
                rotated[i, roomPasswayDatas.GetUpperBound(1) - j] = roomPasswayDatas[j, i];
                switch (rotated[i, roomPasswayDatas.GetUpperBound(1) - j])
                {
                    case 1:
                    case 2:
                    case 3:
                        rotated[i, roomPasswayDatas.GetUpperBound(1) - j]++;
                        break;
                    case 4:
                        rotated[i, roomPasswayDatas.GetUpperBound(1) - j] = 1;
                        break;
                    case 5:
                        rotated[i, roomPasswayDatas.GetUpperBound(1) - j] = 6;
                        break;
                    case 6:
                        rotated[i, roomPasswayDatas.GetUpperBound(1) - j] = 5;
                        break;
                }
            }
        }
        return rotated;
    }
    #endregion

    void mazeCreat(int passwayType, int level, int functionTypeNum)
    {
        int[,] objectDatas = new int[objectCountRowNum, objectCountColNum];
    }
}
