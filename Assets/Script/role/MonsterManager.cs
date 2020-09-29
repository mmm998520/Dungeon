using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterManager : DirectionChanger
    {
        public float MaxHP, HP, ATK, CD, CDTimer, preparation1, preparation2, preparationTimer, hand, atkTime, speed;
        protected int prepare = 0;
        public GameObject attack;
        /// <summary> 守備區域 </summary>
        protected HashSet<int> guardPos = new HashSet<int>();
        /// <summary> 追擊區域 </summary>
        protected HashSet<int> pursuePos = new HashSet<int>();
        public int[] guardPoint;
        public int nextGrardNum;
        protected enum Stat
        {
            guard,
            pursue,
            back
        }
        protected Stat stat = Stat.guard;


        /// <summary> 將怪物誕生區域設為守備區域並給予追擊範圍 </summary>
        protected void setCanGoPos()
        {
            //設置巡邏區域
            guardPos = new HashSet<int>();
            int i, j, t;
            MapCreater.roomStat roomStat;
            guardPos.Add((int)transform.position.x * MapCreater.totalCol[MapCreater.level] + (int)transform.position.y);
            roomStat = (MapCreater.roomStat)MapCreater.mapArray[(int)transform.position.x, (int)transform.position.y];
            do
            {
                t = 0;
                for (i = 0; i < MapCreater.totalRow[MapCreater.level]; i++)
                {
                    for (j = 0; j < MapCreater.totalCol[MapCreater.level]; j++)
                    {
                        if (MapCreater.mapArray[i, j] == (int)roomStat && !guardPos.Contains(MapCreater.RowColToNum(i,j)))
                        {
                            if (guardPos.Contains(MapCreater.RowColToNum(i + 1, j)) || guardPos.Contains(MapCreater.RowColToNum(i - 1, j)) || guardPos.Contains(MapCreater.RowColToNum(i, j + 1)) || guardPos.Contains(MapCreater.RowColToNum(i, j - 1)))
                            {
                                guardPos.Add(MapCreater.RowColToNum(i, j));
                                t++;
                            }
                        }
                    }
                }
            } while (t > 0);
            //設置追擊區域
            pursuePos = new HashSet<int>();
            foreach(int num in guardPos)
            {
                int[] array = MapCreater.NumToRowCol(num);
                for (i = -3; i <= 3; i++)
                {
                    for(j = -3; j <= 3; j++)
                    {
                        if (array[0]+i >= 0 && array[0]+i < MapCreater.totalRow[MapCreater.level] && array[1]+j >= 0 && array[1]+j < MapCreater.totalCol[MapCreater.level])
                        {
                            pursuePos.Add(MapCreater.RowColToNum(i, j));
                        }
                    }
                }
            }
        }


        /// <summary> 獲取最近玩家 </summary>
        public Transform MinDisPlayer()
        {
            int i;
            float minDis = float.MaxValue;
            Transform minDisPlayer = null;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                if (minDis > Vector3.Distance(player.position, transform.position))
                {
                    minDis = Vector3.Distance(player.position, transform.position);
                    minDisPlayer = player;
                }
            }
            return minDisPlayer;
        }


        /// <summary> 轉向並向下一個目標點前進 </summary>
        protected void moveToTarget()
        {
            if (prepare != 2)
            {
                changeDirection();
            }
            if (prepare == 0)
            {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
            }
        }

        /// <summary> 巡邏怪行為 </summary>
        protected void guardBehaviour()
        {
            if (stat == Stat.guard)
            {
                if (Vector3.Distance(GameManager.maze.GetChild(guardPoint[nextGrardNum]).position * Vector2.one, transform.position * Vector2.one) < 0.5f)
                {
                    print("arrive");
                    if (++nextGrardNum >= 4)
                    {
                        nextGrardNum = 0;
                    }
                }
                guard(guardPos);

                if (Vector3.Distance(transform.position * Vector2.one, MinDisPlayer().position * Vector2.one) < 3)
                {
                    stat = Stat.pursue;
                }
            }
            else if (stat == Stat.pursue)
            {
                target = MinDisPlayer();
                if (!pursuePos.Contains((int)transform.position.x * MapCreater.totalCol[MapCreater.level] + (int)transform.position.y))
                {
                    stat = Stat.back;
                }
            }
            else if (stat == Stat.back)
            {
                guard(pursuePos);
                HP = MaxHP;
                if (guardPos.Contains((int)transform.position.x * MapCreater.totalCol[MapCreater.level] + (int)transform.position.y))
                {
                    stat = Stat.guard;
                }
            }
            moveToTarget();
        }
        protected void guard(HashSet<int> canGo)
        {
            Vector3 endPos = GameManager.maze.GetChild(guardPoint[nextGrardNum]).position;
            int[] endRow = new int[1] { (int)endPos.x }, endCol = new int[1] { (int)endPos.y };
            setNavigateTarget(endRow, endCol, canGo);
        }


        protected void attackCD()
        {
            if ((CDTimer += Time.deltaTime) >= CD)
            {
                if (Vector3.Distance(transform.position * Vector2.one, MinDisPlayer().position * Vector2.one) < hand && stat == Stat.pursue)
                {
                    if (prepare == 0)
                    {
                        prepare = 1;
                    }
                }
                else
                {
                    preparationTimer = 0;
                    prepare = 0;
                }
            }
        }

        protected void prepareAttack()
        {
            if (stat != Stat.back)
            {
                if ((preparationTimer += Time.deltaTime) >= preparation1)
                {
                    prepare = 2;
                }
                if ((preparationTimer += Time.deltaTime) >= preparation1 + preparation2)
                {
                    Attack();
                }
            }
            else
            {
                CDTimer = 0;
                preparationTimer = 0;
                prepare = 0;
            }
        }

        protected virtual void Attack()
        {
            MonsterAttack monsterAttack = Instantiate(attack, transform.position, transform.rotation).GetComponent<MonsterAttack>();
            Destroy(monsterAttack.gameObject, atkTime);
            monsterAttack.ATK = ATK;
            CDTimer = 0;
            preparationTimer = 0;
            prepare = 0;
        }
    }
}