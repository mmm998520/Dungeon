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
        /// <summary> 紀錄各個導航點用的pos，0為自己，前半是玩家，後半是能通過的地板 </summary>
        Vector3[] pos;
        int[] S;
        /// <summary> 被擊殺後玩家可獲得的經驗值 </summary>
        public static int[] exp = new int[8] { 10, 20, 10, 0, 0, 0, 0, 0 };

        /// <summary> 攻擊、移動目標 </summary>
        public NearestPlayer target;

        float timer = 0;
        float r = 0.3f;
        protected void monsterStart()
        {
            //處理房間部分的不變資訊，暫定所有房間都能通過
            pos = new Vector3[MazeGen.row * MazeGen.col + 1 + GameManager.Players.childCount];
            for(int i = 1 + GameManager.Players.childCount; i < pos.Length; i++)
            {
                pos[i] = GameManager.Floors.GetChild(i-1- GameManager.Players.childCount).position * Vector2.one;
            }

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

        /// <summary> 計算導航後的最近 玩家 與其 距離 、 路徑 </summary>
        NearestPlayer navigationNearestPlayer()
        {
            //每個點到彼此的距離
            float[,] passwayLengths = new float[pos.Length, pos.Length];
            //導航中距離計算分"可過"與"不可過"兩種，由射線做區分
            int cantWalk = 999999;
            //點的路線
            string[] path = new string[pos.Length];
            //List<List<Vector3>> pathPos = new List<List<Vector3>>();
            /*for(int i = 0; i < pos.Length; i++)
            {
                pathPos.Add(new List<Vector3>());
            }*/
            //最短路徑的頂點集合
            S = new int[pos.Length];

            List<NearestPlayer> playerDis = new List<NearestPlayer>();

            //將pos的變動資訊更新(玩家、怪物位置
            pos[0] = transform.position * Vector2.one;
            for (int i = 1; i <= GameManager.Players.childCount; i++)
            {
                pos[i] = GameManager.Players.GetChild(i-1).position * Vector2.one;
            }

            //計算各點間的距離
            for (int i = 0; i < pos.Length; i++)
            {
                for(int j = 0; j < pos.Length; j++)
                {
                    if (i > GameManager.Players.childCount && j > GameManager.Players.childCount && Vector3.Distance(pos[j] * Vector2.one, pos[i] * Vector2.one)>2.001f)
                    {
                        passwayLengths[i, j] = cantWalk;
                    }
                    else
                    {
                        //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                        Vector3 dir = pos[j] * Vector2.one - pos[i] * Vector2.one;
                        Vector3 tempDir = Quaternion.Euler(0, 0, 90) * dir.normalized / 2;
                        RaycastHit2D hit1 = Physics2D.Raycast(pos[i] + tempDir, dir, dir.magnitude - 0.1f);
                        //Debug.DrawRay(pos[i] + tempDir, dir, Color.red, 2);
                        RaycastHit2D hit2 = Physics2D.Raycast(pos[i] - tempDir, dir, dir.magnitude - 0.1f);
                        //Debug.DrawRay(pos[i] - tempDir, dir, Color.red, 2);
                        if ((hit1 || hit2) || (i <= GameManager.Players.childCount && i > 0))
                        {
                            passwayLengths[i, j] = cantWalk;
                        }
                        else
                        {
                            passwayLengths[i, j] = dir.magnitude;
                        }
                    }
                }
            }

            float min;
            int next;
            for(int i = pos.Length - 1; i > 0; i--)
            {
                min = float.MaxValue;
                next = 0;
                for (int j = 1; j < pos.Length; j++)//迴圈第0行的列
                {
                    if ((IsContain(j) == -1) && (passwayLengths[0, j] < min))//不在S中,找出第一行最小的元素所在的列
                    {
                        min = passwayLengths[0, j];
                        next = j;
                    }
                }
                //將下一個點加入S
                S[next] = next;
                //輸出最短距離和路徑
                if (min >= cantWalk)
                {
                    Debug.Log("V0到V" + next + "的最短路徑為：無" + "," + pos[next]);
                }
                else
                {
                    Debug.Log("V0到V" + next + "的最短路徑為：" + min + ",路徑為：V0" + path[next] + "->V" + next + "," + pos[next]);
                }
                if(next<=GameManager.Players.childCount&& next > 0)
                {
                    int nextPoint;
                    if (path[next] != null)
                    {
                        if (path[next].Length > 4)
                        {
                            nextPoint = int.Parse(Regex.Split(path[next], "->V", RegexOptions.IgnoreCase)[1]);
                        }
                        else
                        {
                            nextPoint = next;
                        }
                    }
                    else
                    {
                        nextPoint = next;
                    }
                    playerDis.Add(new NearestPlayer(GameManager.Players.GetChild(next - 1), min, pos[nextPoint]));
                    if (playerDis.Count >= GameManager.Players.childCount)
                    {
                        if(target.player != playerDis[0].player)
                        {
                            if (target.Distance+1 > playerDis[0].Distance)
                            {
                                //Debug.LogError(playerDis[1].player.name);
                                return playerDis[1];
                            }
                            else
                            {
                                //Debug.LogError(playerDis[0].player.name);
                                return playerDis[0];
                            }
                        }
                        //Debug.LogError(playerDis[0].player.name);
                        return playerDis[0];
                    }
                }
                // 重新初始0行所有列值
                for (int j = 1; j < pos.Length; j++)//迴圈第0行的列
                {
                    if (IsContain(j) == -1)//初始化除包含在S中的
                    {
                        if ((passwayLengths[next, j] + min) < passwayLengths[0, j])//如果小於原來的值就替換
                        {
                            passwayLengths[0, j] = passwayLengths[next, j] + min;
                            path[j] = path[next] + "->V" + next;//記錄過程點
                        }
                    }
                }
            }

            Debug.LogError("!!!!!!!!!!!!!!!!!!!!!!!");
            return new NearestPlayer(null, 0);
        }

        int IsContain(int m)//判斷元素是否在mst中
        {
            int index = -1;
            for (int i = 1; i < pos.Length; i++)
            {
                if (S[i] == m)
                {
                    index = i;
                }
            }
            return index;
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