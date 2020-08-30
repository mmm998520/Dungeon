using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class MonsterManager : ValueSet
    {
        /// <summary> 紀錄各個導航點用的pos，0為自己，前半是玩家，後半是能通過的地板 </summary>
        Vector3[] pos;
        /// <summary> 被擊殺後玩家可獲得的經驗值 </summary>
        public static int[] exp = new int[8] { 10, 20, 10, 0, 0, 0, 0, 0 };

        void Start()
        {
            //處理房間部分的不變資訊，暫定所有房間都能通過
            pos = new Vector3[MazeGen.row * MazeGen.col + 1 + GameManager.Players.childCount];
            for(int i = 1 + GameManager.Players.childCount; i < pos.Length; i++)
            {
                pos[i] = GameManager.Floors.GetChild(i).position * Vector2.one;
            }

            //角色素質用2維陣列儲存， 不同職業(1維) 在 對應等級(2維) 時的素質
            //刺客 -> 戰士 -> 法師
            ATK = new float[(int)MonsterType.Count, 1] { { 4 }, { 8 }, { 9 }, { 4 }, { 10 }, { 15 }, { 20 }, { 100 } };
            HP = new float[(int)MonsterType.Count, 1] { { 6 }, { 15 }, { 2 }, { 6 }, { 10 }, { 25 }, { 30 }, { 110 } };
            duration = new float[(int)MonsterType.Count] { 0.4f, 0.4f, 3, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
            continuous = new bool[(int)MonsterType.Count] { false, false, true, false, false, false, false, false};
        }

        void Update()
        {

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
            return new NearestPlayer(minDisPlayer, minDis, null);
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
            //最短路徑的頂點集合
            int[] S = new int[pos.Length];

            //將pos的變動資訊更新(玩家、怪物位置
            pos[0] = transform.position * Vector2.one;
            for (int i = 1; i <= GameManager.Players.childCount; i++)
            {
                pos[i] = GameManager.Players.GetChild(i).position * Vector2.one;
            }
            //計算各點間的距離
            for (int i = 0; i < pos.Length; i++)
            {
                for(int j = 0; j < pos.Length; j++)
                {
                    //向指定pos打出兩道射線(有間距)判定打到甚麼來決定能不能通過
                    Vector3 dir = pos[j] * Vector2.one - pos[i] * Vector2.one;
                    Vector3 tempDir = Quaternion.Euler(0, 0, 90) * dir.normalized / 2;
                    RaycastHit2D hit1 = Physics2D.Raycast(pos[i] + tempDir, dir, dir.magnitude - 0.1f);
                    Debug.DrawRay(pos[i] + tempDir, dir, Color.red, 2);
                    RaycastHit2D hit2 = Physics2D.Raycast(pos[i] - tempDir, dir, dir.magnitude - 0.1f);
                    Debug.DrawRay(pos[i] - tempDir, dir, Color.red, 2);
                    if(hit1 || hit2)
                    {
                        /*
                        if ((hit1.collider.tag == "side" || hit1.collider.tag == "player") && (hit2.collider.tag == "side" || hit2.collider.tag == "player"))
                        {
                            passwayLengths[i, j] = Vector3.Distance(pos[i] * Vector2.one, pos[i] * Vector2.one);
                        }
                        else
                        {
                            passwayLengths[i, j] = cantWalk;
                        }*/
                        passwayLengths[i, j] = cantWalk;
                    }
                    else
                    {
                        passwayLengths[i, j] = Vector3.Distance(pos[i] * Vector2.one, pos[i] * Vector2.one);
                    }
                }
            }

            int min;
            int next;
            for(int i = pos.Length - 1; i > 0; i--)
            {

            }

            float minDis = float.MaxValue;
            Transform minDisPlayer = GameManager.Players.GetChild(0);
            return new NearestPlayer(minDisPlayer, minDis, null);
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
        public Vector3[] road;
        public NearestPlayer(Transform _player, float _Distance , Vector3[] _road)
        {
            player = _player;
            Distance = _Distance;
            road = _road;
        }
    }
}