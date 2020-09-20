using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MapCreater : MonoBehaviour
    {
        int times;
        public static int[,] mapArray;
        GameObject cubes;
        public GameObject[] cube = new GameObject[4];
        public static int[] totalRow = new int[2] { 12, 180 }, totalCol = new int[2] { 9, 180 };
        public static int level = 0;

        /// <summary> 地形種類 </summary>
        public enum roomStat
        {
            wall = 0,
            green = 1,//起始生成+幽靈
            black = 2,//陷阱
            blue = 3,//怪物
            orange = 4//怪物
        }


        void Awake()
        {
            setMap();
            NavigationManager.setHashSet(mapArray);
        }


        /// <summary> 製作地圖 </summary>
        void setMap()
        {
            int t = 0;
            if (level == 0)
            {
                Destroy(cubes);
                cubes = new GameObject();
                mapArray = InitMapArrayOne();
            }
            else
            {
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
            }

            createMap(mapArray);
            GameManager.maze = cubes.transform;
            print(t);
        }


        ///第一關無邊界的
        int[,] InitMapArrayOne()
        {
            int[,] temp = new int[totalRow[level], totalCol[level]];
            int i, j;
            for(i = 0;i< totalRow[level]; i++)
            {
                for(j = 0; j < totalCol[level]; j++)
                {
                    if(i < 1 || i >= totalRow[level] - 1 || j < 1 || j >= totalCol[level] - 1)
                    {
                        temp[i, j] = (int)roomStat.wall;
                    }
                    else if(i < 3 || i >= totalRow[level] - 3 || j < 2 || j >= totalCol[level] - 2)
                    {
                        temp[i, j] = (int)roomStat.blue;
                    }
                    else if(i < 5 || i >= totalRow[level] - 5 || j < 3 || j >= totalCol[level] - 3)
                    {
                        temp[i, j] = (int)roomStat.black;
                    }
                    else
                    {
                        temp[i, j] = (int)roomStat.green;
                    }
                }
            }
            return temp;
        }

        /// <summary> 生成區域資訊，中心綠，周圍藍橘，其餘黑 </summary>
        int[,] InitMapArray()
        {
            int i, j;
            int[,] array = new int[totalRow[level], totalCol[level]];
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
                {
                    if (i >= totalRow[level] / 7 * 3 && i <= totalRow[level] / 7 * 4 && j >= totalCol[level] / 7 * 3 && j <= totalRow[level] / 7 * 4)
                    {
                        array[i, j] = (int)roomStat.green;
                    }
                    else if (i >= totalRow[level] / 7 * 2 && i <= totalRow[level] / 7 * 5 && j >= totalCol[level] / 7 * 2 && j <= totalRow[level] / 7 * 5)
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
            int[,] newArray = new int[totalRow[level], totalCol[level]];
            int[] count;
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
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
        int[] CheckNeighborWalls(int[,] array, int currentRow, int currentCol, int t)
        {
            int i, j;
            int[] count = new int[5] { 0, 0, 0, 0, 0 };
            for (i = currentRow - t; i <= currentRow + t; i++)
            {
                for (j = currentCol - t; j <= currentCol + t; j++)
                {
                    if (i >= 0 && i < totalRow[level] && j >= 0 && j < totalCol[level])
                    {
                        count[array[i, j]]++;
                    }
                }
            }
            count[array[currentRow, currentCol]]--;
            return count;
        }


        /// <summary> 將WallCreater資料同步彙整 </summary>
        int[,] AddMapWall(int[,] array)
        {
            int i, j;
            int[,] newArray = new int[totalRow[level], totalCol[level]];
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
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
            GameObject cube;
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
                {
                    if(level == 0)
                    {
                        cube = Instantiate(this.cube[array[i, j]], new Vector3(i, j, 1), Quaternion.identity);
                    }
                    else
                    {
                        if (WallCreater.mapArray[i, j])
                        {
                            cube = Instantiate(this.cube[array[i, j]], new Vector3(i, j, 1), Quaternion.identity);
                        }
                        else
                        {
                            cube = Instantiate(this.cube[0], new Vector3(i, j, 1), Quaternion.identity);
                        }
                    }
                    cube.transform.SetParent(cubes.transform);
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
            bool[,] canGoArray = new bool[totalRow[level], totalCol[level]];
            //必須到達點
            int[,] mustGo = new int[9, 2];
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
                {
                    canGoArray[i, j] = false;
                }
            }
            for (t = 0; t < 9; t++)
            {
                for (i = totalRow[level] / 7 * (3 * (t / 3)); i < totalRow[level] / 7 * (3 * (t / 3) + 1); i++)
                {
                    for (j = totalRow[level] / 7 * (3 * (t % 3)); j < totalCol[level] / 7 * (3 * (t % 3) + 1); j++)
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
                for (i = 0; i < totalRow[level]; i++)
                {
                    for (j = 0; j < totalCol[level]; j++)
                    {
                        if (canGoArray[i, j])
                        {
                            if (i + 1 < totalRow[level])
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
                            if (j + 1 < totalCol[level])
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
            bool[,] canGoArray = new bool[totalRow[level], totalCol[level]];
            List<int[]> mustGo = new List<int[]>();
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
                {
                    canGoArray[i, j] = false;
                }
            }
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
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
                for (i = 0; i < totalRow[level]; i++)
                {
                    for (j = 0; j < totalCol[level]; j++)
                    {
                        if (canGoArray[i, j])
                        {
                            if (i + 1 < totalRow[level])
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
                            if (j + 1 < totalCol[level])
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
            bool[,] Checked = new bool[totalRow[level], totalCol[level]];
            int[,] type = new int[totalRow[level], totalCol[level]];
            List<int> typeNum = new List<int>();
            typeNum.Add(0);
            //先確認各區域分別有幾個格子
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
                {
                    Checked[i, j] = false;
                    type[i, j] = 0;
                }
            }
            int times = 0;
            do
            {
                if (times++ > 40000)
                {
                    print(times);
                    break;
                }
                //將最前方的標記為新起始
                for (i = 0; i < totalRow[level]; i++)
                {
                    for (j = 0; j < totalCol[level]; j++)
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
                    for (i = 0; i < totalRow[level]; i++)
                    {
                        for (j = 0; j < totalCol[level]; j++)
                        {
                            if (!Checked[i, j] && type[i, j] != 0)
                            {
                                if (i + 1 < totalRow[level])
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
                                if (j + 1 < totalCol[level])
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
            for (i = 0; i < totalRow[level]; i++)
            {
                for (j = 0; j < totalCol[level]; j++)
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


        public static int RowColToNum(int row,int col)
        {
            return (row * totalCol[level]) + col;
        }

        public static int[] NumToRowCol(int num)
        {
            return new int[2] { num / totalCol[level], num % totalCol[level] };
        }
    }
}