using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class MonsterManager : ValueSet
    {
        protected MonsterType monsterType;

        protected float cd;
        protected float cdTimer;

        /// <summary> 被擊殺後玩家可獲得的經驗值 </summary>
        public static int[] exp = new int[8] { 10, 20, 10, 0, 0, 0, 0, 0 };

        /// <summary> 攻擊、移動目標 </summary>
        public NearestPlayer target;

        float timer = 0;
        float r = 0.3f;
        protected void monsterStart()
        {
            //角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質
            ATK = new float[(int)MonsterType.Count, 1] { { 4 }, { 8 }, { 9 }, { 4 }, { 10 }, { 15 }, { 20 }, { 100 } };
            HP = new float[(int)MonsterType.Count, 1] { { 6 }, { 15 }, { 2 }, { 6 }, { 10 }, { 25 }, { 30 }, { 110 } };
            duration = new float[(int)MonsterType.Count] { 0.4f, 0.4f, 3, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            continuous = new bool[(int)MonsterType.Count] { false, false, true, false, false, false, false, false};

            //初始沒有目標
            target = new NearestPlayer(transform, 0, transform);
        }

        protected void monsterUpdate()
        {
            cdTimer += Time.deltaTime;
            died((int)monsterType, 0);

            #region//間隔一段時間導航移動
            if ((timer+=Time.deltaTime) > r)
            {
                Transform[] end = new Transform[GameManager.Players.childCount];
                for (int i = 0; i < end.Length; i++)
                {
                    end[i] = GameManager.Players.GetChild(i);
                }
                target = navigationNearestPlayer(end);
                r = Random.Range(1f, 2f);
                timer = 0;
            }
            if (target.roadTraget != null)
            {
            Vector3 dirM = (target.roadTraget.position * Vector2.one - transform.position * Vector2.one).normalized * Time.deltaTime;
            //到達定點則重開導航
            if (dirM.magnitude > (target.roadTraget.position * Vector2.one - transform.position * Vector2.one).magnitude)
            {
                transform.position = target.roadTraget.position;
                Transform[] end = new Transform[GameManager.Players.childCount];
                for(int i = 0; i < end.Length; i++)
                {
                    end[i] = GameManager.Players.GetChild(i);
                }
                target = navigationNearestPlayer(end);
                r = Random.Range(1f, 2f);
                timer = 0;
            }
            else
            {
                transform.position = transform.position + dirM;
            }
            }

            #endregion
        }

        /// <summary> 計算直線距離上的最近 玩家 與其 距離  </summary>
        NearestPlayer StraightLineNearestPlayer()
        {
            float minDis = float.MaxValue;
            Transform minDisPlayer = GameManager.Players.GetChild(0);
            for (int i = 0; i < GameManager.Players.childCount; i++)
            {
                float Dis = Vector3.Distance(transform.position, GameManager.Players.GetChild(i).position);
                if (minDis > Dis)
                {
                    minDis = Dis;
                    minDisPlayer = GameManager.Players.GetChild(i);
                }
            }
            return new NearestPlayer(minDisPlayer, minDis,minDisPlayer);
        }
        /// <summary> 計算導航後的最佳路徑，使用A-Star方法 </summary>
        protected NearestPlayer navigationNearestPlayer(Transform[] end)
        {
            //防止錯誤輸入
            if(end.Length == 0)
            {
                Debug.LogError("NoTarget");
                return null;
            }

            //起點像素化
            int startRow, startCol;
            for (startRow = 0; startRow < MazeGen.row; startRow++)
            {
                if (Mathf.Abs(transform.position.x - (startRow * 2 + 1)) <= 1)
                {
                    break;
                }
            }
            for (startCol = 0; startCol < MazeGen.Creat_col; startCol++)
            {
                if (Mathf.Abs(transform.position.y - (startCol * 2 + 1)) <= 1)
                {
                    break;
                }
            }
            Debug.Log(startRow + "," + startCol);
            /*
            //終點像素化
            int[] endRow = new int[GameManager.Players.childCount];
            int[] endCol = new int[GameManager.Players.childCount];
            for (int i = 0; i < GameManager.Players.childCount; i++)
            {
                for (endRow[i] = 0; endRow[i] < MazeGen.row; endRow[i]++)
                {
                    if (Mathf.Abs(GameManager.Players.GetChild(i).position.x - (endRow[i] * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
                for (endCol[i] = 0; endCol[i] < MazeGen.Creat_col; endCol[i]++)
                {
                    if (Mathf.Abs(GameManager.Players.GetChild(i).position.y - (endCol[i] * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
                Debug.Log(endRow[i] + "," + endCol[i]);
            }
            */
            //終點像素化
            int[] endRow = new int[end.Length];
            int[] endCol = new int[end.Length];
            for (int i = 0; i < end.Length; i++)
            {
                for (endRow[i] = 0; endRow[i] < MazeGen.row; endRow[i]++)
                {
                    if (Mathf.Abs(end[i].position.x - (endRow[i] * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
                for (endCol[i] = 0; endCol[i] < MazeGen.Creat_col; endCol[i]++)
                {
                    if (Mathf.Abs(end[i].position.y - (endCol[i] * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
                Debug.Log(endRow[i] + "," + endCol[i]);
            }

            int currentRow = startRow;
            int currentCol = startCol;

            int[,] stat = new int[MazeGen.row, MazeGen.col];
            int[,] dirs = new int[MazeGen.row, MazeGen.col];
            float[,] g = new float[MazeGen.row, MazeGen.col];
            float[,] h = new float[MazeGen.row, MazeGen.col];
            float[,] f = new float[MazeGen.row, MazeGen.col];
            for (int i = 0; i < MazeGen.row; i++)
            {
                for (int j = 0; j < MazeGen.col; j++)
                {
                    stat[i, j] = 0;
                    dirs[i, j] = -1;
                    g[i, j] = 9999;
                    h[i, j] = 9999;
                    f[i, j] = g[i, j] + h[i, j];
                }
            }
            g[currentRow, currentCol] = 0;
            for(int i = 0; i < end.Length; i++)
            {
                if(h[currentRow, currentCol] > Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol))
                {
                    h[currentRow, currentCol] = Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol);
                }
            }
            f[currentRow, currentCol] = g[currentRow, currentCol] + h[currentRow, currentCol];

            //若已在終點上，則改為直接追蹤
            for(int i = 0; i < end.Length; i++)
            {
                if (endRow[i] == currentRow && endCol[i] == currentCol)
                {
                    Debug.LogWarning("nearby");
                    return StraightLineNearestPlayer();
                }
            }

            //開始A-Star
            for (int s = 0; s < 9999; s++)
            {
                //將當前點close
                stat[currentRow, currentCol] = 2;

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        int nextRow = currentRow + i, nextCol = currentCol + j;
                        //判定點有沒有超範圍
                        if (nextRow < MazeGen.row && nextRow >= 0 && nextCol < MazeGen.col && nextCol >= 0)
                        {
                            //判定點是否已經close，已經close就不管他
                            //其餘的判定移動會不會碰撞
                            if (stat[nextRow, nextCol] != 2)
                            {
                                transform.GetComponent<Collider2D>().enabled = false;
                                //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                                Vector3 currentPos = new Vector3(currentRow * 2 + 1, currentCol * 2 + 1);
                                Vector3 Dir = new Vector3(nextRow * 2 + 1, nextCol * 2 + 1) - currentPos;
                                Vector3 tempDir = Quaternion.Euler(0, 0, 90) * Dir.normalized / 2;
                                //layerMask，讓射線只能打牆壁
                                RaycastHit2D hit1 = Physics2D.Raycast(currentPos + tempDir, Dir, Dir.magnitude, 1 << 8);
                                Debug.DrawRay(currentPos + tempDir, Dir, Color.red, 2);
                                RaycastHit2D hit2 = Physics2D.Raycast(currentPos - tempDir, Dir, Dir.magnitude, 1 << 8);
                                Debug.DrawRay(currentPos - tempDir, Dir, Color.red, 2);
                                transform.GetComponent<Collider2D>().enabled = true;
                                if (!(hit1 || hit2))
                                {
                                    //若不碰撞則更新數值
                                    stat[nextRow, nextCol] = 1;
                                    #region//定義方向
                                    int dir = -1;
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
                                    #endregion
                                    float dis;
                                    if (dir % 2 == 0)
                                    {
                                        dis = 1;
                                    }
                                    else
                                    {
                                        dis = 1.4f;
                                    }
                                    if (g[nextRow, nextCol] > g[currentRow, currentCol] + dis)
                                    {
                                        g[nextRow, nextCol] = g[currentRow, currentCol] + dis;
                                        dirs[nextRow, nextCol] = dir;
                                    }
                                    for (int k = 0; k < end.Length; k++)
                                    {
                                        if (h[nextRow, nextCol] > Mathf.Abs(endRow[k] - nextRow) + Mathf.Abs(endCol[k] - nextCol))
                                        {
                                            h[nextRow, nextCol] = Mathf.Abs(endRow[k] - nextRow) + Mathf.Abs(endCol[k] - nextCol);
                                        }
                                    }
                                    if (f[nextRow, nextCol] > g[nextRow, nextCol] + h[nextRow, nextCol])
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
                for (int i = 0; i < MazeGen.row; i++)
                {
                    for (int j = 0; j < MazeGen.col; j++)
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
                            else if (minf == f[i, j])
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
                int minH = 9999;
                int near = -1;
                for (int i = 0; i < end.Length; i++)
                {
                    if (minH > Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol))
                    {
                        minH = Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol);
                        near = i;
                    }
                    else if(minH == Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol))
                    {

                    }
                }
                print("end : "+ "P" + (near +1) + " : " + + endRow[near] + "," + endCol[near]);
                print("current : " + currentRow + "," + currentCol);
                if (endRow[near] == currentRow && endCol[near] == currentCol)
                {
                    print(s);
                    int nextRow = currentCol, nextCol = currentCol;
                    for (int ss = 0; ss < 100; ss++)
                    {
                        print(newRow + "," + newCol + "," + dirs[newRow, newCol]);
                        if (dirs[newRow, newCol] == -1)
                        {
                            Debug.LogWarning(nextRow + "," + nextCol);
                            Debug.LogWarning("end : " + "P" + (near + 1) + " : " +endRow[near] + "," + endCol[near]);
                            return new NearestPlayer(end[near], minH, GameManager.Floors.GetChild(nextRow * MazeGen.col + nextCol));
                        }
                        #region//找曾經的路徑點
                        else if (dirs[newRow, newCol] == 0)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newCol--;
                        }
                        else if (dirs[newRow, newCol] == 1)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newRow--;
                            newCol--;
                        }
                        else if (dirs[newRow, newCol] == 2)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newRow--;
                        }
                        else if (dirs[newRow, newCol] == 3)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newRow--;
                            newCol++;
                        }
                        else if (dirs[newRow, newCol] == 4)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newCol++;
                        }
                        else if (dirs[newRow, newCol] == 5)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newRow++;
                            newCol++;
                        }
                        else if (dirs[newRow, newCol] == 6)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newRow++;
                        }
                        else if (dirs[newRow, newCol] == 7)
                        {
                            nextRow = newRow;
                            nextCol = newCol;
                            newRow++;
                            newCol--;
                        }
                        #endregion
                    }
                    break;
                }
            }
            return null;
        }

        /// <summary> 用override引用 </summary>
        virtual protected void attack()
        {

        }
    }

    /// <summary> 拿來儲存最近的玩家的資訊，哪個玩家、距離多遠，智能移動直線移動距離皆可 </summary>
    public class NearestPlayer
    {
        /// <summary> 最近的玩家 </summary>
        public Transform enemyTraget;
        /// <summary> 距離多遠 </summary>
        public float Distance;
        /// <summary> 導航用最近路徑，非導航則null </summary>
        public Transform roadTraget;
        public NearestPlayer(Transform _enemyTraget, float _Distance , Transform _roadTraget)
        {
            enemyTraget = _enemyTraget;
            Distance = _Distance;
            roadTraget = _roadTraget;
        }
    }
}