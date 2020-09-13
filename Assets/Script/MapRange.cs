using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRange : MonoBehaviour
{
    int times;
    public static int[,] mapArray;
    public static int[,] TempArray;
    GameObject cubes;
    public GameObject[] cube = new GameObject[4];

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
        int t = 0;
        do
        {
            t++;
            Destroy(cubes);
            cubes = new GameObject();
            Button1();
            MapCreate.mapCreate.reMapArray();
            mapArray = InitMapArray();
            for (int i = 0; i < 7; i++)
            {
                times++;
                mapArray = SmoothMapArray(mapArray);
            }
            TempArray = CreateMap(mapArray);
            if (t > 10)
            {
                print("aaaaaaaaaaaaaaaaaaaaaaaa");
                break;
            }
        } while (!checkCanGo(TempArray) || !GreenWayNotSeparate(TempArray));
        print(t);
        print(checkCanGo(TempArray));
        print(GreenWayNotSeparate(TempArray));
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

    int[,] InitMapArray()
    {
        int[,] array = new int[MapCreate.row, MapCreate.col];
        for (int i = 0; i < MapCreate.row; i++)
        {
            for (int j = 0; j < MapCreate.col; j++)
            {
                if (i >= MapCreate.row/7*3 && i <= MapCreate.row / 7 * 4 && j >= MapCreate.col / 7 * 3 && j <= MapCreate.row / 7 * 4)
                {
                    array[i, j] = (int)roomStat.green;
                }
                else if(i >= MapCreate.row / 7 * 2 && i <= MapCreate.row / 7 * 5 && j >= MapCreate.col / 7 * 2 && j <= MapCreate.row / 7 * 5)
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
        int[,] newArray = new int[MapCreate.row, MapCreate.col];
        int[] count;
        for (int i = 0; i < MapCreate.row; i++)
        {
            for (int j = 0; j < MapCreate.col; j++)
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
        int[] count = new int[5] { 0, 0, 0, 0, 0 };
        for (int i2 = i - t; i2 < i + t + 1; i2++)
        {
            for (int j2 = j - t; j2 < j + t + 1; j2++)
            {
                if (i2 >= 0 && i2 < MapCreate.row && j2 >= 0 && j2 < MapCreate.col)
                {
                    count[array[i2, j2]]++;
                }
            }
        }
        count[array[i, j]]--;
        return count;
    }

    int[,] CreateMap(int[,] array)
    {
        int[,] newArray = new int[MapCreate.row, MapCreate.col];
        for (int i = 0; i < MapCreate.row; i++)
        {
            for (int j = 0; j < MapCreate.col; j++)
            {
                GameObject go;
                if (MapCreate.mapArray[i, j])
                {
                    go = Instantiate(cube[array[i, j]], new Vector3(i, 1, j), Quaternion.identity);
                    newArray[i, j] = array[i, j];
                }
                else
                {
                    go = Instantiate(cube[0], new Vector3(i, 1, j), Quaternion.identity);
                    newArray[i, j] = 0;
                }
                go.transform.SetParent(cubes.transform);
            }
        }
        return newArray;
    }

    public void Button1()
    {
        times = 0;
        Destroy(cubes);
        cubes = new GameObject();
        mapArray = InitMapArray();
    }


    public void Button2()
    {
        //if (times < 20)
        {
            times++;
            Destroy(cubes);
            cubes = new GameObject();
            mapArray = SmoothMapArray(mapArray);
            CreateMap(mapArray);
        }
    }

    bool checkCanGo(int[,] array)
    {
        //建立可通行路徑清單，由第一個偵測到的綠色區域作為起始點
        bool[,] canGoArray = new bool[MapCreate.row, MapCreate.col];
        //必須到達點
        int[,] mustGo = new int[9, 2];
        for (int i = 0; i < MapCreate.row; i++)
        {
            for (int j = 0; j < MapCreate.col; j++)
            {
                canGoArray[i, j] = false;
            }
        }
        for(int t = 0; t < 9; t++)
        {
            for (int i = MapCreate.row / 7 * (3 * (t / 3)); i < MapCreate.row / 7 * (3 * (t / 3) + 1) ; i++)
            {
                for (int j = MapCreate.row / 7 * (3 * (t % 3)); j < MapCreate.col / 7 * (3 * (t % 3) + 1); j++)
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
        for (int t = 0; t < MapCreate.row * MapCreate.col; t++)
        {
            for (int i = 0; i < MapCreate.row; i++)
            {
                for (int j = 0; j < MapCreate.col; j++)
                {
                    if (canGoArray[i, j])
                    {
                        if(i + 1 < MapCreate.row)
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
                        if (j + 1 < MapCreate.col)
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

            for(int i = 0; i< mustGo.Length / 2; i++)
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
        bool[,] canGoArray = new bool[MapCreate.row, MapCreate.col];
        List<int[]> mustGo = new List<int[]>();
        for (int i = 0; i < MapCreate.row; i++)
        {
            for (int j = 0; j < MapCreate.col; j++)
            {
                canGoArray[i, j] = false;
            }
        }
        for (int i = 0; i < MapCreate.row; i++)
        {
            for (int j = 0; j < MapCreate.col; j++)
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
        for (int t = 0; t < MapCreate.row * MapCreate.col; t++)
        {
            for (int i = 0; i < MapCreate.row; i++)
            {
                for (int j = 0; j < MapCreate.col; j++)
                {
                    if (canGoArray[i, j])
                    {
                        if (i + 1 < MapCreate.row)
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
                        if (j + 1 < MapCreate.col)
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

            for (int i = 0; i < mustGo.Count; i++)
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
}
