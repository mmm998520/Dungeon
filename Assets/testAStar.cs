using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAStar : MonoBehaviour
{
    int startRow = 2, startCol = 2;
    int endRow = 0, endCol = 2;
    int[,] stat,dirs;
    float[,] g, h, f;
    int currentRow, currentCol;
    void findRoad()
    {
        currentRow = startRow;
        currentCol = startCol;

        stat = new int[3, 3];
        dirs = new int[3, 3];
        g = new float[3, 3];
        h = new float[3, 3];
        f = new float[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                stat[i, j] = 0;
                dirs[i, j] = -1;
                g[i, j] = 100;
                h[i, j] = 100;
                f[i, j] = g[i, j] + h[i, j];
            }
        }
        g[currentRow, currentCol] = 0;
        h[currentRow, currentCol] = Mathf.Abs(endRow - currentRow) + Mathf.Abs(endCol - currentCol);
        f[currentRow, currentCol] = g[currentRow, currentCol] + h[currentRow, currentCol];

        for (int s = 0; s < 50; s++)
        {
            //將當前點close
            stat[currentRow, currentCol] = 2;

            for(int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int nextRow = currentRow + i, nextCol = currentCol + j;
                    //判定點有沒有超範圍
                    if (nextRow < 3 && nextRow >= 0 && nextCol < 3 && nextCol >= 0)
                    {
                        //判定點是否已經close，已經close就不管他，其餘的判定移動會不會碰撞
                        if (stat[nextRow, nextCol] != 2)
                        {
                            //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                            Vector3 currentPos = new Vector3(currentRow, currentCol);
                            Vector3 Dir = new Vector3(nextRow, nextCol) - currentPos;
                            Vector3 tempDir = Quaternion.Euler(0, 0, 90) * Dir.normalized / 3;
                            RaycastHit2D hit1 = Physics2D.Raycast(currentPos + tempDir, Dir, Dir.magnitude);
                            Debug.DrawRay(currentPos + tempDir, Dir, Color.red, 2);
                            RaycastHit2D hit2 = Physics2D.Raycast(currentPos - tempDir, Dir, Dir.magnitude);
                            Debug.DrawRay(currentPos - tempDir, Dir, Color.red, 2);
                            if (!(hit1 || hit2))
                            {
                                //若不碰撞則更新數值
                                stat[nextRow, nextCol] = 1;
                                if(g[nextRow, nextCol] > g[currentRow, currentCol] + Dir.magnitude)
                                {
                                    g[nextRow, nextCol] = g[currentRow, currentCol] + Dir.magnitude;
                                    int dir = -1;
                                    //更新當前路徑時dir也會跟著改變
                                    if (i == 0 && j == 1)
                                    {
                                        dir = 0;
                                    }
                                    else if (i == 1 && j == 1)
                                    {
                                        dir = 1;
                                    }
                                    else if (i == 1 && j == 0)
                                    {
                                        dir = 2;
                                    }
                                    else if (i == 1 && j == -1)
                                    {
                                        dir = 3;
                                    }
                                    else if (i == 0 && j == -1)
                                    {
                                        dir = 4;
                                    }
                                    else if (i == -1 && j == -1)
                                    {
                                        dir = 5;
                                    }
                                    else if (i == -1 && j == 0)
                                    {
                                        dir = 6;
                                    }
                                    else if (i == -1 && j == 1)
                                    {
                                        dir = 7;
                                    }
                                    dirs[nextRow, nextCol] = dir;
                                }
                                print(nextRow+","+ nextCol + ","+g[nextRow, nextCol]);
                                if(h[nextRow, nextCol] > Mathf.Abs(endRow - nextRow) + Mathf.Abs(endCol - nextCol))
                                {
                                    h[nextRow, nextCol] = Mathf.Abs(endRow - nextRow) + Mathf.Abs(endCol - nextCol);
                                }
                                print(nextRow + "," + nextCol + "," + h[nextRow, nextCol]);
                                if(f[nextRow, nextCol] > g[nextRow, nextCol] + h[nextRow, nextCol])
                                {
                                    f[nextRow, nextCol] = g[nextRow, nextCol] + h[nextRow, nextCol];
                                }
                                print(nextRow + "," + nextCol + "," + f[nextRow, nextCol]);
                            }
                        }
                    }
                }
            }

            //從open中找f最小的當新的current，沒有則找h最小
            float minf = 9999;
            float minh = 9999;
            int newRow = 0, newCol = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (stat[i, j] == 1)
                    {
                        if (minf > f[i, j])
                        {
                            minf = f[i, j];
                            newRow = i;
                            newCol = j;
                            minh = 9999;
                        }
                        else if(minf == f[i, j])
                        {
                            if (minh > h[i, j])
                            {
                                minh = h[i, j];
                                newRow = i;
                                newCol = j;
                            }
                        }
                    }
                }
            }

            currentRow = newRow;
            currentCol = newCol;
            print("end : "+endRow + "," + endCol);
            print("current : "+currentRow + "," + currentCol);
            if (endRow == currentRow && endCol == currentCol)
            {
                print(s);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Debug.LogError("all : "+i+","+j+","+dirs[i, j]);
                    }
                }
                for(int ss = 0; ss < 100; ss++)
                {
                    Debug.LogError(newRow+","+ newCol + ","+dirs[newRow, newCol]);
                    if (dirs[newRow, newCol] == -1)
                    {
                        print("end");
                        break;
                    }
                    else if(dirs[newRow, newCol] == 0)
                    {
                        newCol--;
                    }
                    else if (dirs[newRow, newCol] == 1)
                    {
                        newRow--;
                        newCol--;
                    }
                    else if (dirs[newRow, newCol] == 2)
                    {
                        newRow--;
                    }
                    else if (dirs[newRow, newCol] == 3)
                    {
                        newRow--;
                        newCol++;
                    }
                    else if (dirs[newRow, newCol] == 4)
                    {
                        newCol++;
                    }
                    else if (dirs[newRow, newCol] == 5)
                    {
                        newRow++;
                        newCol++;
                    }
                    else if (dirs[newRow, newCol] == 6)
                    {
                        newRow++;
                    }
                    else if (dirs[newRow, newCol] == 7)
                    {
                        newRow++;
                        newCol--;
                    }
                }
                break;
            }
        }
    }
}
