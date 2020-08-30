using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class MonsterManager : ValueSet
    {
        /// <summary> 被擊殺後玩家可獲得的經驗值 </summary>
        public static int[] exp = new int[8] { 10, 20, 10, 0, 0, 0, 0, 0 };

        void Start()
        {
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
            Transform minDisPlayer = PlayerManager.players.GetChild(0);
            for (int i = 0; i < PlayerManager.players.childCount; i++)
            {
                float Dis = Vector3.Distance(transform.position, PlayerManager.players.GetChild(i).position);
                if (minDis > Dis)
                {
                    minDis = Dis;
                    minDisPlayer = PlayerManager.players.GetChild(i);
                }
            }
            return new NearestPlayer(minDisPlayer, minDis, null);
        }

        /// <summary> 計算導航後的最近 玩家 與其 距離 、 路徑 /// </summary>
        NearestPlayer navigationNearestPlayer()
        {
            float minDis = float.MaxValue;
            Transform minDisPlayer = PlayerManager.players.GetChild(0);
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