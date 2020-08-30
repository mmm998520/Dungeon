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

        /// <summary> 計算導航後的最路徑，使用A-star方法 </summary>
        public NearestPlayer navigationNearestPlayer()
        {
            //重製數值
            stat = new int[MazeGen.row, MazeGen.col];
            g = new float[MazeGen.row, MazeGen.col];
            h = new float[MazeGen.row, MazeGen.col];
            f = new float[MazeGen.row, MazeGen.col];
            for (int i = 0; i < MazeGen.row; i++)
            {
                for (int j = 0; j < MazeGen.col; j++)
                {
                    g[i, j] = int.MaxValue;
                    h[i, j] = int.MaxValue;
                    f[i, j] = int.MaxValue;
                }
            }

            //起點像素化
            int startRow, startCol;
            for (startRow = 0; startRow < MazeGen.Creat_col; startRow++)
            {
                if (Mathf.Abs(transform.position.x - (startRow * 2 + 1)) <= 1)
                {
                    break;
                }
            }
            for (startCol = 0; startCol < MazeGen.Creat_col; startCol++)
            {
                if (Mathf.Abs(transform.position.x - (startCol * 2 + 1)) <= 1)
                {
                    break;
                }
            }
            //終點像素化
            int[] endRow = new int[GameManager.Players.childCount], endCol = new int[GameManager.Players.childCount];
            for (int i = 0; i < GameManager.Players.childCount; i++)
            {
                for (endRow[i] = 0; endRow[i] < MazeGen.Creat_col; endRow[i]++)
                {
                    if (Mathf.Abs(GameManager.Players.GetChild(i).position.x - (endRow[i] * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
                for (endCol[i] = 0; endCol[i] < MazeGen.Creat_col; endCol[i]++)
                {
                    if (Mathf.Abs(GameManager.Players.GetChild(i).position.x - (endCol[i] * 2 + 1)) <= 1)
                    {
                        break;
                    }
                }
            }
            //紀錄當前位置
            int currentRow = startRow, currentCol = startCol;
            //暫存鄰近點，方便看是否有牆
            int[] roundRow = new int[2];
            int[] roundCol = new int[2];

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
                int newRow = 0, newCol = 0;
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
                for (int i = 0; i < GameManager.Players.childCount; i++)
                {
                    if (endRow[i] == currentRow && endCol[i] == currentCol)
                    {
                        Debug.LogError("YA");
                        return new NearestPlayer(null, 0);
                    }
                }
                if (s > 999)
                {
                    Debug.LogError("Boom");
                }
            } 

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
            
            else if(stat[nextRow, nextCol] < 1)
            {
                //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                Vector3 currentPos = new Vector3(currentRow * 2 + 1, currentCol * 2 + 1);
                Vector3 Dir = new Vector3(nextRow * 2+1, nextCol * 2+1) - currentPos;
                Vector3 tempDir = Quaternion.Euler(0, 0, 90) * Dir.normalized / 2;
                RaycastHit2D hit1 = Physics2D.Raycast(currentPos + tempDir, Dir, Dir.magnitude - 0.1f);
                Debug.DrawRay(currentPos + tempDir, Dir, Color.red, 2);
                RaycastHit2D hit2 = Physics2D.Raycast(currentPos - tempDir, Dir, Dir.magnitude - 0.1f);
                Debug.DrawRay(currentPos - tempDir, Dir, Color.red, 2);
                if (hit1 || hit2)
                {
                    //會撞到牆，不管他
                    return;
                }
                else
                {
                    //可以過，新增資料
                    stat[nextRow, nextCol] = 1;
                }
            }
            //已经在open列表中：当我们使用当前生成的路径到达那里时，检查F 和值是否更小。如果是，更新它的和值和它的前继
            //辨別斜走或直走
            float cost;
            if(dir % 2 == 0)
            {
                cost = 1;
            }
            else
            {
                cost = 1.414f;
            }
            //新增點若當前值過大則更新
            if(g[nextRow, nextCol] > g[currentRow, currentCol] + cost)
            {
                g[nextRow, nextCol] = g[currentRow, currentCol] + cost;
                dirs[nextRow, nextCol] = dir;
            }
            if(h[nextRow, nextCol]>Vector3.Distance(new Vector3(nextRow * 2 + 1, nextCol * 2 + 1), new Vector3(currentRow * 2 + 1, currentCol * 2 + 1)))
            {
                h[nextRow, nextCol] = Vector3.Distance(new Vector3(nextRow * 2 + 1, nextCol * 2 + 1), new Vector3(currentRow * 2 + 1, currentCol * 2 + 1));
            }
            if(f[nextRow, nextCol]> g[nextRow, nextCol]+ h[nextRow, nextCol])
            {
                f[nextRow, nextCol] = g[nextRow, nextCol] + h[nextRow, nextCol];
            }
            return;
        }
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