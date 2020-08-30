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

        /// <summary> 計算導航後的最近 玩家 與其 距離 、 路徑，使用A-star方法 </summary>
        public NearestPlayer navigationNearestPlayer()
        {

            return new NearestPlayer(null, 0);
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