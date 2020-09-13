using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreater : MonoBehaviour
{
    int times;
    public static int[,] mapArray;
    public static int[,] TempArray;
    GameObject cubes;
    public GameObject[] cube = new GameObject[4];
    public const int row = 70, col = 70;

    public enum roomStat
    {
        wall = 0,
        green = 1,
        black = 2,
        blue = 3,
        orange = 4
    }

    void Start()
    {
        StartCoroutine("s");
    }

    IEnumerator s()
    {
        int t = 0;
        do
        {
            t++;
            Destroy(cubes);
            cubes = new GameObject();
            Button1();
            WallCreater.wallCreater.reMapArray();
            mapArray = InitMapArray();
            for (int i = 0; i < 7; i++)
            {
                times++;
                mapArray = SmoothMapArray(mapArray);
            }
            TempArray = AddMapWall(mapArray);
            createMap(TempArray);
            yield return new WaitForSeconds(2);
            Destroy(cubes);
            cubes = new GameObject();
            TempArray = checkNum(TempArray);
            createMap(TempArray);
            yield return new WaitForSeconds(2);
            if (t > 30)
            {
                break;
            }
        } while (!checkCanGo(TempArray) || !GreenWayNotSeparate(TempArray));
        print(t);
    }

    int[,] InitMapArray()
    {
        int i, j;
        int[,] array = new int[row, col];
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                if (i >= row/7*3 && i <= row / 7 * 4 && j >= col / 7 * 3 && j <= row / 7 * 4)
                {
                    array[i, j] = (int)roomStat.green;
                }
                else if(i >= row / 7 * 2 && i <= row / 7 * 5 && j >= col / 7 * 2 && j <= row / 7 * 5)
                {
                    array[i, j] = (int)roomStat.black;
                }
                else
                {
                    int r = Random.Range(0, 11);
                    if (r >= 10)
                    {
                        array[i, j] = (int)roomStat.black;
                    }
                    else if(r >= 5)
                    {
                        array[i, j] = (int)roomStat.blue;
                    }
                    else
                    {
                        array[i, j] = (int)roomStat.orange;
                    }
                }
            }
        }
        return array;
    }

    int[,] SmoothMapArray(int[,] array)
    {
        int i, j;
        int[,] newArray = new int[row, col];
        int[] count;
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                count = CheckNeighborWalls(array, i, j, 1);
                if (count[(int)roomStat.green] > 4)
                {
                    newArray[i, j] = (int)roomStat.green;
                }
                else if(count[(int)roomStat.green] == 4)
                {
                    int r = 0;
                    if (times < 7)
                    {
                        r = Random.Range(0, 2);
                    }
                    newArray[i, j] = (int)roomStat.green + r;
                }
                else if(count[(int)roomStat.black] > 4)
                {
                    newArray[i, j] = (int)roomStat.black;
                }
                else if (count[(int)roomStat.black] == 4)
                {
                    int r = 0;
                    if (times < 7)
                    {
                        r = Random.Range(0, 11);
                        if (r >= 10)
                        {
                            newArray[i, j] = (int)roomStat.black;
                        }
                        else if (r >= 5)
                        {
                            newArray[i, j] = (int)roomStat.blue;
                        }
                        else
                        {
                            newArray[i, j] = (int)roomStat.orange;
                        }
                    }
                    else
                    {
                        newArray[i, j] = (int)roomStat.black;
                    }
                }
                else if(count[(int)roomStat.blue] > 4)
                {
                    newArray[i, j] = (int)roomStat.blue;
                }
                else if (count[(int)roomStat.orange] > 4)
                {
                    newArray[i, j] = (int)roomStat.orange;
                }
                else
                {
                    int r = Random.Range(0, 2);
                    newArray[i, j] = (int)roomStat.blue + r;
                }
            }
        }
        return newArray;
    }

    int[] CheckNeighborWalls(int[,] array, int i, int j, int t)
    {
        int _i, _j;
        int[] count = new int[5] { 0, 0, 0, 0, 0 };
        for (_i = i - t; _i < i + t + 1; _i++)
        {
            for (_j = j - t; _j < j + t + 1; _j++)
            {
                if (_i >= 0 && _i < row && _j >= 0 && _j < col)
                {
                    count[array[_i, _j]]++;
                }
            }
        }
        count[array[i, j]]--;
        return count;
    }

    int[,] AddMapWall(int[,] array)
    {
        int i, j;
        int[,] newArray = new int[row, col];
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                if (WallCreater.mapArray[i, j])
                {
                    newArray[i, j] = array[i, j];
                }
                else
                {
                    newArray[i, j] = 0;
                }
            }
        }
        return newArray;
    }

    void createMap(int[,] array)
    {
        int i, j;
        GameObject go;
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                if (WallCreater.mapArray[i, j])
                {
                    go = Instantiate(cube[array[i, j]], new Vector3(i, j, 1), Quaternion.identity);
                }
                else
                {
                    go = Instantiate(cube[0], new Vector3(i, j, 1), Quaternion.identity);
                }
                go.transform.SetParent(cubes.transform);
            }
        }
    }

    public void Button1()
    {
        times = 0;
        mapArray = InitMapArray();
    }

    bool checkCanGo(int[,] array)
    {
        int t, j, i;
        //建立可通行路徑清單，由第一個偵測到的綠色區域作為起始點
        bool[,] canGoArray = new bool[row, col];
        //必須到達點
        int[,] mustGo = new int[9, 2];
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                canGoArray[i, j] = false;
            }
        }
        for(t = 0; t < 9; t++)
        {
            for (i = row / 7 * (3 * (t / 3)); i < row / 7 * (3 * (t / 3) + 1) ; i++)
            {
                for (j = row / 7 * (3 * (t % 3)); j < col / 7 * (3 * (t % 3) + 1); j++)
                {
                    if (t == 4)
                    {
                        if (array[i, j] == (int)roomStat.green)
                        {
                            mustGo[t, 0] = i;
                            mustGo[t, 1] = j;
                            goto Break;
                        }
                    }
                    else
                    {
                        if (array[i, j] != (int)roomStat.wall)
                        {
                            if(t == 0)
                            {
                                canGoArray[i, j] = true;
                            }
                            mustGo[t, 0] = i;
                            mustGo[t, 1] = j;
                            goto Break;
                        }
                    }
                }
            }
            //如果在區域沒發現終點(沒觸發goto)則失敗
            return false;
            Break:;
        }
        //從最左上方的必須抵達點掃看能抵達點，若所有必需抵達點都為true則成功，反之失敗
        for (t = 0; t < 100; t++)
        {
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    if (canGoArray[i, j])
                    {
                        if(i + 1 < row)
                        {
                            if (array[i + 1, j] != (int)roomStat.wall)
                            {
                                canGoArray[i + 1, j] = true;
                            }
                        }
                        if (i - 1 >= 0)
                        {
                            if (array[i - 1, j] != (int)roomStat.wall)
                            {
                                canGoArray[i - 1, j] = true;
                            }
                        }
                        if (j + 1 < col)
                        {
                            if (array[i, j + 1] != (int)roomStat.wall)
                            {
                                canGoArray[i, j + 1] = true;
                            }
                        }
                        if(j - 1 >= 0)
                        {
                            if (array[i, j - 1] != (int)roomStat.wall)
                            {
                                canGoArray[i, j - 1] = true;
                            }
                        }
                    }
                }
            }

            for(i = 0; i< mustGo.Length / 2; i++)
            {
                if(!canGoArray[mustGo[i,0], mustGo[i, 1]])
                {
                    break;
                }
                if(i >= mustGo.Length / 2 - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool GreenWayNotSeparate(int[,] array)
    {
        int t, i, j;
        bool[,] canGoArray = new bool[row, col];
        List<int[]> mustGo = new List<int[]>();
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                canGoArray[i, j] = false;
            }
        }
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                if (array[i, j] == (int)roomStat.green)
                {
                    mustGo.Add(new int[] { i, j });
                }
            }
        }
        if (mustGo.Count > 0)
        {
            canGoArray[mustGo[0][0], mustGo[0][1]] = true;
        }
        else
        {
            return false;
        }
        //從最左上方的必須抵達點掃看能抵達點，若所有必需抵達點都為true則成功，反之失敗
        for (t = 0; t < 100; t++)
        {
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    if (canGoArray[i, j])
                    {
                        if (i + 1 < row)
                        {
                            if (array[i + 1, j] == (int)roomStat.green)
                            {
                                canGoArray[i + 1, j] = true;
                            }
                        }
                        if (i - 1 >= 0)
                        {
                            if (array[i - 1, j] == (int)roomStat.green)
                            {
                                canGoArray[i - 1, j] = true;
                            }
                        }
                        if (j + 1 < col)
                        {
                            if (array[i, j + 1] == (int)roomStat.green)
                            {
                                canGoArray[i, j + 1] = true;
                            }
                        }
                        if (j - 1 >= 0)
                        {
                            if (array[i, j - 1] == (int)roomStat.green)
                            {
                                canGoArray[i, j - 1] = true;
                            }
                        }
                    }
                }
            }

            for (i = 0; i < mustGo.Count; i++)
            {
                if (!canGoArray[mustGo[i][0], mustGo[i][1]])
                {
                    break;
                }
                if (i >= mustGo.Count-1)
                {
                    return true;
                }
            }
        }
        return false;
    }

    int[,] checkNum(int[,] array)
    {
        int t, i, j;
        bool[,] Checked = new bool[row, col];
        int[,] type = new int[row, col];
        List<int> typeNum = new List<int>();
        typeNum.Add(0);
        //先確認各區域分別有幾個格子
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                Checked[i, j] = false;
                type[i, j] = 0;
            }
        }
        int times = 0;
        do
        {
            if (times++ > 1000)
            {
                print(times);
                break;
            }
            //將最前方的標記為新起始
            for (i = 0; i < row; i++)
            {
                for (j = 0; j < col; j++)
                {
                    if(type[i, j] == 0)
                    {
                        type[i, j] = typeNum.Count;
                        print(type[i, j]);
                        typeNum.Add(1);
                        goto FindRoad;
                    }
                }
            }
            //沒觸發goto則代表全都搜尋完畢了
            break;
        FindRoad:;
            //從起始待使向外找尋可同型路徑，直到沒有路為止
            t = 0;
            int times2 = 0;
            do
            {
                if (times2++ > 100)
                {
                    print(times2);
                    break;
                }
                for (i = 0; i < row; i++)
                {
                    for (j = 0; j < col; j++)
                    {
                        if (!Checked[i, j] && type[i, j] != 0)
                        {
                            if (i + 1 < row)
                            {
                                if (array[i, j] == array[i + 1, j] && type[i + 1, j] == 0)
                                {
                                    t++;
                                    type[i + 1, j] = type[i, j];
                                    typeNum[type[i, j]]++;
                                }
                            }
                            if (i - 1 >= 0)
                            {
                                if (array[i, j] == array[i - 1, j] && type[i - 1, j] == 0)
                                {
                                    t++;
                                    type[i - 1, j] = type[i, j];
                                    typeNum[type[i, j]]++;
                                }
                            }
                            if (j + 1 < col)
                            {
                                if (array[i, j] == array[i, j + 1] && type[i, j + 1] == 0)
                                {
                                    t++;
                                    type[i, j + 1] = type[i, j];
                                    typeNum[type[i, j]]++;
                                }
                            }
                            if (j - 1 >= 0)
                            {
                                if (array[i, j] == array[i, j - 1] && type[i, j - 1] == 0)
                                {
                                    t++;
                                    type[i, j - 1] = type[i, j];
                                    typeNum[type[i, j]]++;
                                }
                            }
                            Checked[i, j] = true;
                        }
                    }
                }
            } while (t > 0);
        } while (true);
        /*
        for (i = 0; i < row; i++)
        {
            for(j = 0; j < col; j++)
            {
                if(type[i,j] == 0)
                {
                    type[i, j] = typeNum.Count;
                    typeNum.Add(1);
                }
                if (i + 1 < row)
                {
                    if (array[i, j] == array[i + 1, j] && type[i + 1, j] == 0)
                    {
                        type[i + 1, j] = type[i, j];
                        typeNum[type[i, j]]++;
                    }
                }
                if (j + 1 < col)
                {
                    if (array[i, j] == array[i, j + 1] && type[i, j + 1] == 0)
                    {
                        type[i, j + 1] = type[i, j];
                        typeNum[type[i, j]]++;
                    }
                }
            }
        }
        */
        //將格子數量太少的挑出來
        for (i = 0; i < row; i++)
        {
            for (j = 0; j < col; j++)
            {
                if (typeNum[type[i, j]] < 10)
                {
                    if (i - 1 >= 0 && array[i,j] != (int)roomStat.wall)
                    {
                        array[i, j] = array[i - 1, j];
                    }
                }
            }
        }
        return array;

        /*
        int a = 0;
        foreach(int k in typeNum)
        {
            a += k;
        }
        print(a);
        print(typeNum.Count);
        */
    }
}
