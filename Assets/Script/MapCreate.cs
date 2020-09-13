using UnityEngine;
using System.Collections;


public class MapCreate : MonoBehaviour
{
    public static int row = 70;
    public static int col = 70;
    public static bool[,] mapArray;
    public static MapCreate mapCreate;
    void Start()
    {
        mapCreate = this;
        reMapArray();
    }

    public void reMapArray()
    {
        mapArray = InitMapArray();
        for (int i = 0; i < 7; i++)
        {
            mapArray = SmoothMapArray(mapArray);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Button1();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Button2();
        }
    }

    /// <summary> 隨機生成地圖資料庫 </summary>
    bool[,] InitMapArray()
    {
        bool[,] array = new bool[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                //60%機率true反之false
                array[i, j] = Random.Range(0, 100) < 60;
                //外壁固定為牆
                if (i == 0 || i == row - 1 || j == 0 || j == col - 1)
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
        bool[,] newArray = new bool[row, col];
        int count1, count2;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
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

                if (i == 0 || i == row - 1 || j == 0 || j == col - 1)
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
        int count = 0;
        for (int i2 = i - t; i2 < i + t + 1; i2++)
        {
            for (int j2 = j - t; j2 < j + t + 1; j2++)
            {
                if (i2 > 0 && i2 < row && j2 >= 0 && j2 < col)
                {
                    if (!array[i2, j2])
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

    public void Button1()
    {
        mapArray = InitMapArray();
    }


    public void Button2()
    {
        //if (times < 20)
        {
            mapArray = SmoothMapArray(mapArray);
        }
    }
}