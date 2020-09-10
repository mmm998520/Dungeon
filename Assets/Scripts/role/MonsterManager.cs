using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class MonsterManager : ValueSet
    {
        protected MonsterType monsterType;

        /// <summary> 被擊殺後玩家可獲得的經驗值 </summary>
        public static float[] exp = new float[8] { 10, 20, 10, 0, 0, 0, 0, 0 };
        public List<PlayerManager> HurtMe = new List<PlayerManager>();

        /// <summary> 攻擊、移動目標 </summary>
        public NearestEnd navigateTarget;
        public NearestEnd straightTarget;

        /// <summary> 麻痺狀態 </summary>
        public bool paralysis = false;
        /// <summary> 擊退狀態 </summary>
        public Vector3 cahrged = Vector3.zero;
        public float cahrgedSpeed = 0;
        protected void monsterStart()
        {
            //角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質
            ATK = new float[(int)MonsterType.Count, 1] { { 4 }, { 8 }, { 9 }, { 4 }, { 10 }, { 15 }, { 20 }, { 20 } };
            HP = new float[(int)MonsterType.Count, 1] { { 6 }, { 15 }, { 2 }, { 6 }, { 10 }, { 50 }, { 30 }, { 150 } };
            duration = new float[(int)MonsterType.Count] { 0.4f, 0.4f, 3, 0.4f, 0.4f, 0.4f, 0.4f, 2f };
            continuous = new bool[(int)MonsterType.Count] { false, false, true, false, false, false, false, true};

            //初始沒有目標
            navigateTarget = new NearestEnd(transform, 0, transform);
        }

        protected void monsterUpdate()
        {
            cdTimer += Time.deltaTime;
            died((int)monsterType, 0);
            transform.GetChild(1).localScale = new Vector3((HP[(int)monsterType, 0] - Hurt) / HP[(int)monsterType, 0], 1, 1);
            if ((cahrgedSpeed -= Time.deltaTime*2) > 0)
            {
                transform.Translate(cahrged * Time.deltaTime * cahrgedSpeed);
            }
        }

        #region//導航
        /// <summary> 計算直線距離上的最近點與其距離  </summary>
        protected NearestEnd StraightLineNearest(Transform[] end)
        {
            if(end == null)
            {
                Debug.LogError("RRRR");
                return null;
            }
            float minDis = float.MaxValue;
            Transform minDisEnd = end[0];
            for (int i = 0; i < end.Length; i++)
            {
                float Dis = Vector3.Distance(transform.position, end[i].position);
                if (minDis > Dis)
                {
                    minDis = Dis;
                    minDisEnd = end[i];
                }
            }
            return new NearestEnd(minDisEnd, minDis, minDisEnd);
        }

        protected NearestEnd Navigate(Transform[] end, Transform[] range)
        {
            #region//初始化數值
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
                    g[i, j] = 9999999;
                    h[i, j] = 9999999;
                    f[i, j] = g[i, j] + h[i, j];
                }
            }
            g[currentRow, currentCol] = 0;
            for (int i = 0; i < end.Length; i++)
            {
                if (h[currentRow, currentCol] > Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol))
                {
                    h[currentRow, currentCol] = Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol);
                }
            }
            f[currentRow, currentCol] = g[currentRow, currentCol] + h[currentRow, currentCol];

            //若已在終點上，則追蹤另一點
            for (int i = 0; i < end.Length; i++)
            {
                if (endRow[i] == currentRow && endCol[i] == currentCol)
                {
                    Debug.LogWarning("nearby");
                    //要換成改追蹤下個點
                    return GoNextTarget();
                }
            }

            List<int[]> rangeToInt = new List<int[]>();
            if (range == null)
            {
                foreach (Transform child in GameManager.Floors)
                {
                    string[] sArray = child.name.Split(',');
                    rangeToInt.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
                }
            }
            else
            {
                foreach (Transform child in GameManager.Floors)
                {
                    if (range.Contains(child))
                    {
                        string[] sArray = child.name.Split(',');
                        rangeToInt.Add(new int[] { int.Parse(sArray[0]), int.Parse(sArray[1]) });
                    }
                }
            }
            #endregion

            //開始導航
            for (int s = 0; s < 999; s++)
            {
                //將當前位置點登記維以確認點(close)
                stat[currentRow,currentCol] = 2;

                #region//查看當前點周圍的點
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        int nextRow = currentRow + i, nextCol = currentCol + j;
                        //確定點沒有超範圍
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
                                RaycastHit2D hitWall1 = Physics2D.Raycast(currentPos + tempDir, Dir, Dir.magnitude, 1 << 8);
                                RaycastHit2D hitWall2 = Physics2D.Raycast(currentPos - tempDir, Dir, Dir.magnitude, 1 << 8);
                                RaycastHit2D hitOther = Physics2D.Raycast(currentPos, Dir, Dir.magnitude, 1 << 0);
                                transform.GetComponent<Collider2D>().enabled = true;
                                //如果會撞到則不選擇
                                if (!(hitWall1 || hitWall2))
                                {
                                    int dir;
                                    float dis;
                                    if (hitOther)
                                    {
                                        if (hitOther.collider.tag != "monster" || (hitOther.collider.tag == "monster" && Vector3.Distance(hitOther.collider.transform.position, transform.position) > 10))
                                        {
                                            Debug.DrawRay(currentPos, Dir, Color.green, 5);
                                            stat[nextRow, nextCol] = 1;
                                            dir = canGoDir(nextRow, nextCol, i, j);
                                            dis = canGoDis(dir, nextRow, nextCol, rangeToInt);
                                            if(hitOther.collider.tag == "player")
                                            {
                                                dis = willMeetPlayer(dis);
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
                                        }
                                        else
                                        {
                                            Debug.DrawRay(currentPos, Dir, Color.red, 5);
                                        }
                                    }
                                    else
                                    {
                                        Debug.DrawRay(currentPos, Dir, Color.green, 5);
                                        stat[nextRow, nextCol] = 1;
                                        dir = canGoDir(nextRow, nextCol, i, j);
                                        dis = canGoDis(dir, nextRow, nextCol, rangeToInt);
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
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region//從open中找f最小的當新的current，沒有則找h最小
                float minf = 9999999;
                float minh = 9999999;
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
                                minh = 9999999;
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
                #endregion

                #region//終點
                currentRow = newRow;
                currentCol = newCol;
                int minH = 9999999;
                int near = -1;
                //最近的終點代號
                for (int i = 0; i < end.Length; i++)
                {
                    if (minH > Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol))
                    {
                        minH = Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol);
                        near = i;
                    }
                    else if (minH == Mathf.Abs(endRow[i] - currentRow) + Mathf.Abs(endCol[i] - currentCol))
                    {

                    }
                }
                //抵達終點
                if (endRow[near] == currentRow && endCol[near] == currentCol)
                {
                    int nextRow = currentCol, nextCol = currentCol;
                    for (int ss = 0; ss < 100; ss++)
                    {
                        if (dirs[newRow, newCol] == -1)
                        {
                            return new NearestEnd(end[near], minH, GameManager.Floors.GetChild(nextRow * MazeGen.col + nextCol));
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
                #endregion
            }

            return null;
        }
        /// <summary> 導航找路時的參數，紀錄方向 </summary>
        int canGoDir(int nextRow, int nextCol,int i, int j)
        {
            //若不碰撞則更新數值
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
            return dir;
        }

        /// <summary> 導航找路時的參數，紀錄距離 </summary>
        float canGoDis(int dir, int nextRow, int nextCol, List<int[]> rangeToInt)
        {
            float dis;
            if (dir % 2 == 0)
            {
                dis = 1;
            }
            else
            {
                dis = 1.4f;
            }
            //看該點是否在守備區域內，若有則增加移動難度
            bool inRange = false;
            
            for (int k = 0; k < rangeToInt.Count; k++)
            {
                if (rangeToInt[k][0] == nextRow && rangeToInt[k][1] == nextCol)
                {
                    inRange = true;
                    break;
                }
            }
            if (!inRange)
            {
                dis += 100;
            }
            dis = priority(dis, nextRow, nextCol);
            return dis;
        }

        #region//導航需調用參數
        /// <summary> 給特別會針對路徑上的player做反應的怪用，例如史萊姆 </summary>
        virtual protected float willMeetPlayer(float dis)
        {
            return dis;
        }

        /// <summary> 讓怪朝著目標移動，並在到達定點後開啟GoNavigate </summary>
        virtual protected void GoNavigate(NearestEnd targer)
        {
            Vector3 nextPos = targer.roadTraget.position * Vector2.one;
            Vector3 dir = nextPos * Vector2.one - transform.position * Vector2.one;
            //單位時間移動量
            float dis = Time.deltaTime * moveSpeed;
            //到達定點(移動量大於距離)則重開導航
            if (dis > dir.magnitude)
            {
                transform.position = nextPos + transform.position.z * Vector3.forward;
                GoNextRoad();
            }
            else
            {
                transform.Translate(dis * dir.normalized);
            }
        }

        /// <summary> 抵達路徑點，朝下個路徑點前進 </summary>
        virtual protected void GoNextRoad()
        {
            Debug.LogError("抵達之後要幹嘛你又沒說，override我啦乾");
        }

        /// <summary> 改追蹤下個目標 </summary>
        virtual protected NearestEnd GoNextTarget()
        {
            Debug.LogError("抵達之後要幹嘛你又沒說，override我啦乾");
            return null;
        }

        /// <summary> 決定優先行動區域，讓怪盡量不會超出自己區域 </summary>
        virtual protected float priority(float dis, int nextRow, int nextCol)
        {
            return dis;
        }
        #endregion
        #endregion

        /// <summary> 若選定的目標在範圍(hand)內則每一段時間攻擊一次 </summary>
        virtual protected void attackOccasion(NearestEnd Target, float hand)
        {
            if (paralysis)
            {
                return;
            }
            if (Target.endTraget != null)
            {
                if (Vector3.Distance(transform.position, Target.endTraget.position) < hand && cdTimer > cd)
                {
                    attack();
                    cdTimer = 0;
                }
            }
        }

        /// <summary> 用override引用 </summary>
        virtual protected void attack()
        {

        }

        protected override void died(int type, int level)
        {
            if (Hurt > HP[type, level])
            {
                for (int i = HurtMe.Count - 1; i >= 0; i--)
                {
                    if (HurtMe[i] == null)
                    {
                        print("有人掰囉");
                        HurtMe.RemoveAt(i);
                    }
                }
                foreach (PlayerManager playerManager in HurtMe)
                {
                    playerManager.exp += exp[(int)monsterType] / HurtMe.Count;
                }
                afterDied();
                Destroy(gameObject);
            }
        }
        /// <summary> 增加傷害清單，紀錄誰對他觸發過攻擊 </summary>
        public static void hurtMe(Collider2D collider, PlayerManager user, bool poison)
        {
            if (collider.GetComponent<MonsterManager>() && user != null)
            {
                if(user.career == Career.Thief && user.statOne)
                {
                    user.statOne = false;
                    user.skillOneContinuedTimer = 100;
                }
                if (poison)
                {
                    collider.GetComponent<MonsterManager>().callParalysis(user);
                }
                if (!collider.GetComponent<MonsterManager>().HurtMe.Contains(user))
                {
                    collider.GetComponent<MonsterManager>().HurtMe.Add(user);
                }
            }
        }

        public void callParalysis(PlayerManager player)
        {
            player.statTwo = false;
            player.skillTwoContinuedTimer = player.skillTwoContinued;
            StartCoroutine("Paralysis");
        }

        WaitForSeconds paralysisWait = new WaitForSeconds(4);
        IEnumerator Paralysis()
        {
            paralysis = true;
            if (transform.GetChild(2))
            {
                transform.GetChild(2).gameObject.SetActive(false);
            }
            yield return paralysisWait;
            paralysis = false;
        }
    }

    /// <summary> 拿來儲存最近的玩家的資訊，哪個玩家、距離多遠，智能移動直線移動距離皆可 </summary>
    public class NearestEnd
    {
        /// <summary> 最近的終點 </summary>
        public Transform endTraget;
        /// <summary> 距離多遠 </summary>
        public float Distance;
        /// <summary> 導航用最近路徑，非導航則null </summary>
        public Transform roadTraget;
        public NearestEnd(Transform _endTraget, float _Distance , Transform _roadTragets)
        {
            endTraget = _endTraget;
            Distance = _Distance;
            roadTraget = _roadTragets;
        }
    }
}