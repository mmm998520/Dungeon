using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FloorLight : MonoBehaviour
    {
        public string[,] mazeDatas;
        int totalRow, totalCol;
        List<int[]> side = new List<int[]>();
        List<List<float>> lights = new List<List<float>>();
        List<GameObject> lightGameObjects = new List<GameObject>();
        public GameObject lightPrefab;
        void Start()
        {
            mazeDatas = GameManager.mazeCreater.mazeDatas;
            totalRow = GameManager.mazeCreater.roomCountRowNum * GameManager.mazeCreater.objectCountRowNum;
            totalCol = GameManager.mazeCreater.roomCountColNum * GameManager.mazeCreater.objectCountColNum;
            side.Add(new int[] { -1, 0 });
            side.Add(new int[] { 1, 0 });
            side.Add(new int[] { 0, -1 });
            side.Add(new int[] { 0, 1 });
            for(int i = 0; i < 5; i++)
            {
                Invoke("create", Random.Range(0.1f, 0.5f));
            }
        }

        void Update()
        {
            for(int i = lights.Count - 1; i >= 0 ; i--)
            {
                if((lights[i][5] += Time.deltaTime) >= 0.3f)
                {
                    lights[i][5] = 0;
                    if (++lights[i][6] > 3)
                    {
                        lights.RemoveAt(i);
                        GameObject temp = lightGameObjects[i];
                        lightGameObjects.Remove(temp);
                        Destroy(temp);
                    }
                    else
                    {
                        lights[i] = changPos(lights[i]);
                        if (lights[i] == null)
                        {
                            lights.RemoveAt(i);
                            GameObject temp = lightGameObjects[i];
                            lightGameObjects.Remove(temp);
                            Destroy(temp);
                        }
                    }
                }
                else
                {
                    lightGameObjects[i].transform.position = new Vector3((float)(lights[i][0] + lights[i][2]) / 2, (float)(lights[i][1] + lights[i][3]) / 2,-1);
                    if (lights[i][4] == 1 || lights[i][4] == 3)
                    {
                        lightGameObjects[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    }
                    else
                    {
                        lightGameObjects[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                    }
                }
            }
        }

        void create()
        {
            createLight();
            Invoke("create", Random.Range(0.1f, 0.5f));
        }

        void createLight()
        {
            int[] newInsPos = findNewInsPos();
            int newInsPosDir = setNewInsDir(newInsPos);
            List<float> temp = new List<float>();
            int i;
            for (i = 0; i < newInsPos.Length; i++)
            {
                temp.Add(newInsPos[i]);//介於哪兩個地板之間
            }
            temp.Add(newInsPosDir);//方向
            temp.Add(0);//時間
            temp.Add(0);
            lights.Add(temp);
            lightGameObjects.Add(Instantiate(lightPrefab));
        }

        int[] findNewInsPos()
        {
            int i, row, col, r, tempRow, tempCol;
            List<int[]> side;
            bool tempIsWall = true;
            do
            {
                side = new List<int[]>();
                for (i=0;i< 4; i++)
                {
                    side.Add(this.side[i]);
                }

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

        int setNewInsDir(int[] newPos)
        {
            if (newPos[0] == newPos[2])
            {
                if(Random.Range(0, 2) > 0)
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            else
            {
                if (Random.Range(0, 2) > 0)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
        }

        List<float> changPos(List<float> light)
        {
            int r;
            List<float> temp = new List<float>();
            //向上
            if (light[4] == 0)
            {
                bool rowUpIsWall = mazeDatas[(int)light[0], (int)light[1] + 1] == "wall" || mazeDatas[(int)light[0], (int)light[1] + 1] == "" || mazeDatas[(int)light[0], (int)light[1] + 1] == null;
                bool tempRowUpIsWall = mazeDatas[(int)light[2], (int)light[3] + 1] == "wall" || mazeDatas[(int)light[2], (int)light[3] + 1] == "" || mazeDatas[(int)light[2], (int)light[3] + 1] == null;
                if (rowUpIsWall || tempRowUpIsWall)
                {
                    return null;
                }
                else
                {
                    r = Random.Range(0, 4);
                    //向下導致相撞則強迫直行
                    if (r == 2)
                    {
                        r = 0;
                    }
                    if (r == 0)
                    {
                        temp.Add(light[0]);
                        temp.Add(light[1] + 1);
                        temp.Add(light[2]);
                        temp.Add(light[3] + 1);
                        temp.Add(0);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 1)
                    {
                        float maxRow = Mathf.Max(light[0], light[2]);
                        temp.Add(maxRow);
                        temp.Add(light[1]);
                        temp.Add(maxRow);
                        temp.Add(light[1] + 1);
                        temp.Add(1);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 3)
                    {
                        float minRow = Mathf.Min(light[0], light[2]);
                        temp.Add(minRow);
                        temp.Add(light[1]);
                        temp.Add(minRow);
                        temp.Add(light[1] + 1);
                        temp.Add(3);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    return temp;
                }
            }
            //向右
            if (light[4] == 1)
            {
                bool rowUpIsWall = mazeDatas[(int)light[0] + 1, (int)light[1]] == "wall" || mazeDatas[(int)light[0] + 1, (int)light[1]] == "" || mazeDatas[(int)light[0] + 1, (int)light[1]] == null;
                bool tempRowUpIsWall = mazeDatas[(int)light[2] + 1, (int)light[3]] == "wall" || mazeDatas[(int)light[2] + 1, (int)light[3]] == "" || mazeDatas[(int)light[2] + 1, (int)light[3]] == null;
                if (rowUpIsWall || tempRowUpIsWall)
                {
                    return null;
                }
                else
                {
                    r = Random.Range(0, 4);
                    //向左導致相撞則強迫直行
                    if (r == 3)
                    {
                        r = 1;
                    }
                    if (r == 0)
                    {
                        float maxCol = Mathf.Max(light[1], light[3]);
                        temp.Add(light[0]);
                        temp.Add(maxCol);
                        temp.Add(light[0] + 1);
                        temp.Add(maxCol);
                        temp.Add(0);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 1)
                    {
                        temp.Add(light[0] + 1);
                        temp.Add(light[1]);
                        temp.Add(light[2] + 1);
                        temp.Add(light[3]);
                        temp.Add(1);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 2)
                    {
                        float minCol = Mathf.Min(light[1], light[3]);
                        temp.Add(light[0]);
                        temp.Add(minCol);
                        temp.Add(light[0] + 1);
                        temp.Add(minCol);
                        temp.Add(2);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    return temp;
                }
            }
            //向下
            if (light[4] == 2)
            {
                bool rowUpIsWall = mazeDatas[(int)light[0], (int)light[1] - 1] == "wall" || mazeDatas[(int)light[0], (int)light[1] - 1] == "" || mazeDatas[(int)light[0], (int)light[1] - 1] == null;
                bool tempRowUpIsWall = mazeDatas[(int)light[2], (int)light[3] - 1] == "wall" || mazeDatas[(int)light[2], (int)light[3] - 1] == "" || mazeDatas[(int)light[2], (int)light[3] - 1] == null;
                if (rowUpIsWall || tempRowUpIsWall)
                {
                    return null;
                }
                else
                {
                    r = Random.Range(0, 4);
                    //向上導致相撞則強迫直行
                    if (r == 0)
                    {
                        r = 2;
                    }
                    if (r == 1)
                    {
                        float maxRow = Mathf.Max(light[0], light[2]);
                        temp.Add(maxRow);
                        temp.Add(light[1]);
                        temp.Add(maxRow);
                        temp.Add(light[1] - 1);
                        temp.Add(1);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 2)
                    {
                        temp.Add(light[0]);
                        temp.Add(light[1] - 1);
                        temp.Add(light[2]);
                        temp.Add(light[3] - 1);
                        temp.Add(2);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 3)
                    {
                        float minRow = Mathf.Min(light[0], light[2]);
                        temp.Add(minRow);
                        temp.Add(light[1]);
                        temp.Add(minRow);
                        temp.Add(light[1] - 1);
                        temp.Add(3);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    return temp;
                }
            }
            //向左
            if (light[4] == 3)
            {
                bool rowUpIsWall = mazeDatas[(int)light[0] - 1, (int)light[1]] == "wall" || mazeDatas[(int)light[0] - 1, (int)light[1]] == "" || mazeDatas[(int)light[0] - 1, (int)light[1]] == null;
                bool tempRowUpIsWall = mazeDatas[(int)light[2] - 1, (int)light[3]] == "wall" || mazeDatas[(int)light[2] - 1, (int)light[3]] == "" || mazeDatas[(int)light[2] - 1, (int)light[3]] == null;
                if (rowUpIsWall || tempRowUpIsWall)
                {
                    return null;
                }
                else
                {
                    r = Random.Range(0, 4);
                    //向右導致相撞則強迫直行
                    if (r == 1)
                    {
                        r = 3;
                    }
                    if (r == 0)
                    {
                        float maxCol = Mathf.Max(light[1], light[3]);
                        temp.Add(light[0]);
                        temp.Add(maxCol);
                        temp.Add(light[0] - 1);
                        temp.Add(maxCol);
                        temp.Add(0);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 2)
                    {
                        float minCol = Mathf.Min(light[1], light[3]);
                        temp.Add(light[0]);
                        temp.Add(minCol);
                        temp.Add(light[0] - 1);
                        temp.Add(minCol);
                        temp.Add(2);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    if (r == 3)
                    {
                        temp.Add(light[0] - 1);
                        temp.Add(light[1]);
                        temp.Add(light[2] - 1);
                        temp.Add(light[3]);
                        temp.Add(3);
                        temp.Add(light[5]);
                        temp.Add(light[6]);
                    }
                    return temp;
                }
            }

            Debug.LogError("方向錯誤");
            return null;
        }
    }
}