using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.Dungeon
{
    public class MapCreater : MonoBehaviour
    {
        int times;
        public static int[,] mapArray;
        GameObject cubes;
        public GameObject[] cube = new GameObject[4];
        public const int totalRow = 70, totalCol = 70;


        /// <summary> 地形種類 </summary>
        public enum roomStat
        {
            wall = 0,
            green = 1,
            black = 2,
            blue = 3,
            orange = 4
        }


        void Awake()
        {
            setMap();
        }


        /// <summary> 製作地圖 </summary>
        void setMap()
        {
            int t = 0;
            do
            {
                t++;
                reMap();
                WallCreater.wallCreater.reMap();
                mapArray = InitMapArray();
                for (int i = 0; i < 7; i++)
                {
                    times++;
                    mapArray = SmoothMapArray(mapArray);
                }
                mapArray = AddMapWall(mapArray);
                createMap(mapArray);
                Destroy(cubes);
                cubes = new GameObject();
                mapArray = checkNum(mapArray);
                if (t > 30)
                {
                    break;
                }
            } while (!checkCanGo(mapArray) || !GreenWayNotSeparate(mapArray));
            createMap(mapArray);
            print(t);
        }


        /// <summary> 生成區域資訊，中心綠，周圍藍橘，其餘黑 </summary>
        int[,] InitMapArray()
        {
            int i, j;
            int[,] array = new int[totalRow, totalCol];
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    if (i >= totalRow / 7 * 3 && i <= totalRow / 7 * 4 && j >= totalCol / 7 * 3 && j <= totalRow / 7 * 4)
                    {
                        array[i, j] = (int)roomStat.green;
                    }
                    else if (i >= totalRow / 7 * 2 && i <= totalRow / 7 * 5 && j >= totalCol / 7 * 2 && j <= totalRow / 7 * 5)
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
                        else if (r >= 5)
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


        /// <summary>潤飾地圖，讓地圖更像洞穴  </summary>
        int[,] SmoothMapArray(int[,] array)
        {
            int i, j;
            int[,] newArray = new int[totalRow, totalCol];
            int[] count;
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    count = CheckNeighborWalls(array, i, j, 1);
                    if (count[(int)roomStat.green] > 4)
                    {
                        newArray[i, j] = (int)roomStat.green;
                    }
                    else if (count[(int)roomStat.green] == 4)
                    {
                        int r = 0;
                        if (times < 7)
                        {
                            r = Random.Range(0, 2);
                        }
                        newArray[i, j] = (int)roomStat.green + r;
                    }
                    else if (count[(int)roomStat.black] > 4)
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
                    else if (count[(int)roomStat.blue] > 4)
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


        /// <summary> 潤飾地圖時偵測周圍同類數，藉此同化數值 </summary>
        int[] CheckNeighborWalls(int[,] array, int i, int j, int t)
        {
            int _i, _j;
            int[] count = new int[5] { 0, 0, 0, 0, 0 };
            for (_i = i - t; _i < i + t + 1; _i++)
            {
                for (_j = j - t; _j < j + t + 1; _j++)
                {
                    if (_i >= 0 && _i < totalRow && _j >= 0 && _j < totalCol)
                    {
                        count[array[_i, _j]]++;
                    }
                }
            }
            count[array[i, j]]--;
            return count;
        }


        /// <summary> 將WallCreater資料同步彙整 </summary>
        int[,] AddMapWall(int[,] array)
        {
            int i, j;
            int[,] newArray = new int[totalRow, totalCol];
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
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


        /// <summary> 將地圖實例化 </summary>
        void createMap(int[,] array)
        {
            int i, j;
            GameObject go;
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
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


        /// <summary> 重製地圖 </summary>
        public void reMap()
        {
            Destroy(cubes);
            cubes = new GameObject();
            times = 0;
            mapArray = InitMapArray();
        }


        /// <summary> 偵測製作出的地圖是否可通行所有必需經過點 </summary>
        bool checkCanGo(int[,] array)
        {
            int t, j, i;
            //建立可通行路徑清單，由第一個偵測到的綠色區域作為起始點
            bool[,] canGoArray = new bool[totalRow, totalCol];
            //必須到達點
            int[,] mustGo = new int[9, 2];
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    canGoArray[i, j] = false;
                }
            }
            for (t = 0; t < 9; t++)
            {
                for (i = totalRow / 7 * (3 * (t / 3)); i < totalRow / 7 * (3 * (t / 3) + 1); i++)
                {
                    for (j = totalRow / 7 * (3 * (t % 3)); j < totalCol / 7 * (3 * (t % 3) + 1); j++)
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
                                if (t == 0)
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
                for (i = 0; i < totalRow; i++)
                {
                    for (j = 0; j < totalCol; j++)
                    {
                        if (canGoArray[i, j])
                        {
                            if (i + 1 < totalRow)
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
                            if (j + 1 < totalCol)
                            {
                                if (array[i, j + 1] != (int)roomStat.wall)
                                {
                                    canGoArray[i, j + 1] = true;
                                }
                            }
                            if (j - 1 >= 0)
                            {
                                if (array[i, j - 1] != (int)roomStat.wall)
                                {
                                    canGoArray[i, j - 1] = true;
                                }
                            }
                        }
                    }
                }

                for (i = 0; i < mustGo.Length / 2; i++)
                {
                    if (!canGoArray[mustGo[i, 0], mustGo[i, 1]])
                    {
                        break;
                    }
                    if (i >= mustGo.Length / 2 - 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary> 確認綠色區域沒有被一分為二 </summary>
        bool GreenWayNotSeparate(int[,] array)
        {
            int t, i, j;
            bool[,] canGoArray = new bool[totalRow, totalCol];
            List<int[]> mustGo = new List<int[]>();
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    canGoArray[i, j] = false;
                }
            }
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
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
                for (i = 0; i < totalRow; i++)
                {
                    for (j = 0; j < totalCol; j++)
                    {
                        if (canGoArray[i, j])
                        {
                            if (i + 1 < totalRow)
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
                            if (j + 1 < totalCol)
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
                    if (i >= mustGo.Count - 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary> 確認各個小區的點數量，避免區域過小怪不能移動 </summary>
        int[,] checkNum(int[,] array)
        {
            int t, i, j;
            bool[,] Checked = new bool[totalRow, totalCol];
            int[,] type = new int[totalRow, totalCol];
            List<int> typeNum = new List<int>();
            typeNum.Add(0);
            //先確認各區域分別有幾個格子
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
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
                for (i = 0; i < totalRow; i++)
                {
                    for (j = 0; j < totalCol; j++)
                    {
                        if (type[i, j] == 0)
                        {
                            type[i, j] = typeNum.Count;
                            typeNum.Add(1);
                            goto FindRoad;
                        }
                    }
                }
                //沒觸發goto則代表全都搜尋完畢了
                break;

            FindRoad:;
                //從起始點開始向外找尋可通行路徑，直到沒有路為止
                int times2 = 0;
                do
                {
                    t = 0;
                    if (times2++ > 100)
                    {
                        print(times2);
                        break;
                    }
                    for (i = 0; i < totalRow; i++)
                    {
                        for (j = 0; j < totalCol; j++)
                        {
                            if (!Checked[i, j] && type[i, j] != 0)
                            {
                                if (i + 1 < totalRow)
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
                                if (j + 1 < totalCol)
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
            //將格子數量太少的挑出來
            for (i = 0; i < totalRow; i++)
            {
                for (j = 0; j < totalCol; j++)
                {
                    if (typeNum[type[i, j]] < 10)
                    {
                        if (i - 1 >= 0 && array[i, j] != (int)roomStat.wall)
                        {
                            array[i, j] = array[i - 1, j];
                        }
                    }
                }
            }
            return array;
        }
    }
}