using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class MonsterManager : ValueSet
    {
        public GameObject[] tempDir;

        protected MonsterType monsterType;

        protected float cd;
        protected float cdTimer;

        /// <summary> 被擊殺後玩家可獲得的經驗值 </summary>
        public static int[] exp = new int[8] { 10, 20, 10, 0, 0, 0, 0, 0 };

        /// <summary> 攻擊、移動目標 </summary>
        public NearestPlayer target;

        float timer = 0;
        float r = 0.3f;
        /*
        //A-Star導航用數據
        /// <summary> 紀錄 0未採用null、1開放open、2關閉close資訊，open表示該點為close的可直達點，close表示該點已確認 </summary>
        int[,] stat;
        /// <summary> 已確認的與起點距離 </summary>
        float[,] g;
        /// <summary> 未確認的預估距離 </summary>
        float[,] h;
        /// <summary> 總距離(g+h) </summary>
        float[,] f;
        /// <summary> 紀錄到該路徑的方向，8方位，上方為0，順時針遞增 </summary>
        int[,] dirs;
        int[] endRow, endCol;
        */
        protected void monsterStart()
        {
            //角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質
            ATK = new float[(int)MonsterType.Count, 1] { { 4 }, { 8 }, { 9 }, { 4 }, { 10 }, { 15 }, { 20 }, { 100 } };
            HP = new float[(int)MonsterType.Count, 1] { { 6 }, { 15 }, { 2 }, { 6 }, { 10 }, { 25 }, { 30 }, { 110 } };
            duration = new float[(int)MonsterType.Count] { 0.4f, 0.4f, 3, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            continuous = new bool[(int)MonsterType.Count] { false, false, true, false, false, false, false, false};

            //初始沒有目標
            target = new NearestPlayer(transform, 0);
        }

        protected void monsterUpdate()
        {
            cdTimer += Time.deltaTime;
            died((int)monsterType, 0);

            if ((timer+=Time.deltaTime) > r)
            {
                target = navigationNearestPlayer();
                r = Random.Range(1f, 2f);
                timer = 0;
            }
            Vector3 dirM = (target.road * Vector2.one - transform.position * Vector2.one).normalized * Time.deltaTime;
            if (dirM.magnitude > (target.road * Vector2.one - transform.position * Vector2.one).magnitude)
            {
                transform.position = target.road;
                target = navigationNearestPlayer();
                r = Random.Range(1f, 2f);
                timer = 0;
            }
            else
            {
                transform.position = transform.position + dirM;
            }
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
            return new NearestPlayer(minDisPlayer, minDis);
        }
        /// <summary> 計算導航後的最佳路徑，使用A-star方法 </summary>
        protected NearestPlayer navigationNearestPlayer()
        {
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
            Debug.Log(startRow + "," + startCol);
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
            for(int i = 0; i < GameManager.Players.childCount; i++)
            {
                if(h[currentRow, currentCol] > Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol))
                {
                    h[currentRow, currentCol] = Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol);
                }
            }
            f[currentRow, currentCol] = g[currentRow, currentCol] + h[currentRow, currentCol];

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
                                RaycastHit2D hit1 = Physics2D.Raycast(currentPos + tempDir, Dir, Dir.magnitude);
                                Debug.DrawRay(currentPos + tempDir, Dir, Color.red, 2);
                                RaycastHit2D hit2 = Physics2D.Raycast(currentPos - tempDir, Dir, Dir.magnitude);
                                Debug.DrawRay(currentPos - tempDir, Dir, Color.red, 2);
                                transform.GetComponent<Collider2D>().enabled = true;
                                bool update = false;
                                if (!(hit1 || hit2))
                                {
                                    //若不碰撞則更新數值
                                    update = true;
                                }
                                else if(hit1)
                                {
                                    if (hit1.collider.tag == "player")
                                    {
                                        //撞到人也可更新
                                        update = true;
                                    }
                                }
                                else if (hit2)
                                {
                                    if (hit2.collider.tag == "player")
                                    {
                                        //撞到人也可更新
                                        update = true;
                                    }
                                }
                                if (update)
                                {
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
                                    for(int k = 0; k < GameManager.Players.childCount; k++)
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
                for (int i = 0; i < GameManager.Players.childCount; i++)
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
                Debug.LogError("end : "+ "P" + (near +1) + " : " + + endRow[near] + "," + endCol[near]);
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
                            print("end");
                            print(nextRow + "," + nextCol);
                            break;
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

        /*
        /// <summary> 計算導航後的最路徑，使用A-star方法 </summary>
        public NearestPlayer navigationNearestPlayer()
        {
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

            //終點像素化
            endRow = new int[GameManager.Players.childCount];
            endCol = new int[GameManager.Players.childCount];
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

            #region//重製數值
            stat = new int[MazeGen.row, MazeGen.col];
            g = new float[MazeGen.row, MazeGen.col];
            h = new float[MazeGen.row, MazeGen.col];
            f = new float[MazeGen.row, MazeGen.col];
            dirs = new int[MazeGen.row, MazeGen.col];
            for (int i = 0; i < MazeGen.row; i++)
            {
                for (int j = 0; j < MazeGen.col; j++)
                {
                    g[i, j] = float.MaxValue;
                    h[i, j] = float.MaxValue;
                    f[i, j] = float.MaxValue;
                    dirs[i, j] = i - 1;
                }
            }
            g[startRow, startCol] = 0;
            print(startRow + "," + startCol);
            #endregion

            //紀錄當前位置
            int currentRow = startRow, currentCol = startCol;
            Debug.LogError(startRow + ","+ startCol);
            Debug.LogError(currentRow + ","+ currentCol);

            //while安全開關，防無限迴圈
            for (int s =0;s<1000;s++)
            {
                //開始找路
                //將當前點(原點)設為close
                stat[currentRow, currentCol] = 2;

                #region//將數據傳給checkPoint更新數值
                if (currentRow > 0)
                {
                    if (currentCol > 0)
                    {
                        checkPos(currentRow - 1, currentCol - 1, currentRow, currentCol, 5);
                    }
                    if (currentCol < MazeGen.col - 1)
                    {
                        checkPos(currentRow - 1, currentCol + 1, currentRow, currentCol, 7);
                    }
                    checkPos(currentRow - 1, currentCol, currentRow, currentCol, 6);
                }
                if (currentRow < MazeGen.row - 1)
                {
                    if (currentCol > 0)
                    {
                        checkPos(currentRow + 1, currentCol - 1, currentRow, currentCol, 3);
                    }
                    if (currentCol < MazeGen.col - 1)
                    {
                        checkPos(currentRow + 1, currentCol + 1, currentRow, currentCol, 1);
                    }
                    checkPos(currentRow + 1, currentCol, currentRow, currentCol, 2);
                }
                if (currentCol > 0)
                {
                    checkPos(currentRow, currentCol - 1, currentRow, currentCol, 4);
                }
                if (currentCol < MazeGen.col - 1)
                {
                    checkPos(currentRow, currentCol + 1, currentRow, currentCol, 0);
                }
                #endregion

                //在open中找f最小的做新的點
                float minF = float.MaxValue;
                int newRow = -1, newCol = -1;
                for(int i = 0; i < MazeGen.row; i++)
                {
                    for(int j = 0; j < MazeGen.col; j++)
                    {
                        if (stat[i,j]==1 && minF > f[i, j])
                        {
                            minF = f[i, j];
                            newRow = i;
                            newCol = j;
                        }
                    }
                }
                currentRow = newRow;
                currentCol = newCol;
                Debug.LogError(newRow + "," + newCol);
                Debug.LogError(currentRow + "," + currentCol);

                for (int i = 0; i < GameManager.Players.childCount; i++)
                {
                    if (endRow[2] == currentRow && endCol[2] == currentCol)
                    {
                        Debug.LogError("YA");
                        Debug.Log(endRow[i] +","+ endCol[i]);
                        Debug.Log(currentRow + ","+ currentCol);
                        int stepRow = currentRow, stepCol = currentCol;
                        int time = 0;
                        for (int t = 0; t < MazeGen.row; t++)
                        {
                            for (int tt = 0; tt < MazeGen.col; tt++)
                            {
                                if(dirs[t, tt] >= 0)
                                {
                                    print(t +","+ tt +","+ dirs[t, tt]);
                                    Instantiate(tempDir[dirs[t, tt]], new Vector3(t * 2 + 1, tt * 2 + 1), Quaternion.identity);
                                }
                            }
                        }
                        do
                        {
                            Debug.Log(dirs[stepRow, stepCol]);
                            switch(dirs[stepRow, stepCol])
                            {
                                case 0:
                                    stepRow--;
                                    break;
                                case 1:
                                    stepRow--;
                                    stepCol--;
                                    break;
                                case 2:
                                    stepCol--;
                                    break;
                                case 3:
                                    stepRow++;
                                    stepCol--;
                                    break;
                                case 4:
                                    stepRow++;
                                    break;
                                case 5:
                                    stepRow++;
                                    stepCol++;
                                    break;
                                case 6:
                                    stepCol++;
                                    break;
                                case 7:
                                    stepRow--;
                                    stepCol++;
                                    break;
                            }
                            if (time++ > 100)
                            {
                                Debug.LogError("Boom");
                                break;
                            }
                        }
                        while (!(stepRow == startRow && stepCol == startCol));

                        Debug.LogError("Boom");
                        return new NearestPlayer(null, 0);
                    }
                    else
                    {
                        print(endRow[2] + "," + endCol[2] + "!=" + currentRow + "," + currentCol);
                    }
                }
                if (s >= 999)
                {
                    Debug.LogError("Boom");
                }
            }
            Debug.LogError("Boom");

            return new NearestPlayer(null, 0);
        }
        /// <summary> 檢查當前點能否前往下個點，並更新其數值 </summary>
        void checkPos(int nextRow, int nextCol, int currentRow, int currentCol, int dir)
        {
            //在closed列表中：不管它
            if (stat[nextRow, nextCol] > 1)
            {
                return;
            }
            //不在open列表中：添加它然后计算出它的和值
            //已经在open列表中：当我们使用当前生成的路径到达那里时，检查F 和值是否更小。如果是，更新它的和值和它的前继
            else
            {
                //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                Vector3 currentPos = new Vector3(currentRow * 2 + 1, currentCol * 2 + 1);
                Vector3 Dir = new Vector3(nextRow * 2+1, nextCol * 2+1) - currentPos;
                Vector3 tempDir = Quaternion.Euler(0, 0, 90) * Dir.normalized / 2;
                RaycastHit2D hit1 = Physics2D.Raycast(currentPos + tempDir, Dir, Dir.magnitude);
                Debug.DrawRay(currentPos + tempDir, Dir, Color.red, 2);
                RaycastHit2D hit2 = Physics2D.Raycast(currentPos - tempDir, Dir, Dir.magnitude);
                Debug.DrawRay(currentPos - tempDir, Dir, Color.red, 2);
                if (hit1 || hit2)
                {
                    Debug.LogWarning(currentRow + "," + currentCol + "->" + nextRow + "," + nextCol);
                    Debug.LogWarning(currentPos + "->" + currentPos + Dir);
                    //會撞到牆，不管他
                    return;
                }
                else
                {
                    print(currentRow + "," + currentCol + "->" + nextRow + "," + nextCol);
                    print(currentPos + "->" + currentPos+Dir);
                    //可以過，新增、更新資料
                    stat[nextRow, nextCol] = 1;
                    //辨別斜走或直走
                    float cost;
                    if (dir % 2 == 0)
                    {
                        cost = 1;
                    }
                    else
                    {
                        cost = 1.414f;
                    }
                    //新增點若當前值過大則更新
                    if (g[nextRow, nextCol] > g[currentRow, currentCol] + cost)
                    {
                        g[nextRow, nextCol] = g[currentRow, currentCol] + cost;
                        dirs[nextRow, nextCol] = dir;
                        print(nextRow + "," + nextCol + "," + dirs[nextRow, nextCol]);
                    }
                    float minDistance = float.MaxValue;
                    for (int i = 0; i < GameManager.Players.childCount; i++)
                    {
                        float dis = Vector3.Distance(new Vector3(nextRow * 2 + 1, nextCol * 2 + 1), GameManager.Players.GetChild(i).position);
                        if (minDistance > dis)
                        {
                            minDistance = dis;
                        }
                    }
                    if (h[nextRow, nextCol] > Mathf.Abs(nextRow-endRow[2])+Mathf.Abs(nextCol-endCol[2]))
                    {
                        h[nextRow, nextCol] = minDistance/2;
                    }
                    if (f[nextRow, nextCol] > g[nextRow, nextCol] + h[nextRow, nextCol])
                    {
                        f[nextRow, nextCol] = g[nextRow, nextCol] + h[nextRow, nextCol];
                    }
                }
            }
            return;
        }
        */
    }

    /// <summary> 拿來儲存最近的玩家的資訊，哪個玩家、距離多遠，智能移動直線移動距離皆可 </summary>
    public class NearestPlayer
    {
        /// <summary> 最近的玩家 </summary>
        public Transform player;
        /// <summary> 距離多遠 </summary>
        public float Distance;
        /// <summary> 導航用最近路徑，非導航則null </summary>
        public Vector3 road;
        public NearestPlayer(Transform _player, float _Distance , Vector3 _road)
        {
            player = _player;
            Distance = _Distance;
            road = _road;
        }
        public NearestPlayer(Transform _player, float _Distance)
        {
            player = _player;
            Distance = _Distance;
        }
    }
}