using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRange : MonoBehaviour
{
    int times;
    private int[,] mapArray;
    GameObject cubes;
    public GameObject[] cube = new GameObject[4];

    public enum roomStat
    {
        green = 0,
        black = 1,
        blue = 2,
        orange = 3
    }
    void Start()
    {
        cubes = new GameObject();
        mapArray = InitMapArray();
        CreateMap(mapArray);
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
                if (count[0] > 4)
                {
                    newArray[i, j] = (int)roomStat.green;
                }
                else if(count[0] == 4)
                {
                    int r = Random.Range(0, 2);
                    newArray[i, j] = (int)roomStat.green + r;
                }
                else if(count[1] >= 4)
                {
                    newArray[i, j] = (int)roomStat.black;
                }
                else if(count[2] > 4)
                {
                    newArray[i, j] = (int)roomStat.blue;
                }
                else if (count[3] > 4)
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
        int[] count = new int[4] { 0, 0, 0, 0 };
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

    void CreateMap(int[,] array)
    {
        for (int i = 0; i < MapCreate.row; i++)
        {
            for (int j = 0; j < MapCreate.col; j++)
            {
                GameObject go = Instantiate(cube[array[i, j]], new Vector3(i, 1, j), Quaternion.identity);
                go.transform.SetParent(cubes.transform);
            }
        }
    }

    public void Button1()
    {
        times = 0;
        Destroy(cubes);
        cubes = new GameObject();
        mapArray = InitMapArray();
        CreateMap(mapArray);
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
}
