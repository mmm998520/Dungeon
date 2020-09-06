using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Slime : MonsterManager
    {
        enum OrigenStat
        {
            mid,side
        }
        OrigenStat origenStat = OrigenStat.side;
        //當前移動目標
        NearestEnd midTarget, sideTarget;
        //Transform[] range;
        //定義移動區域
        Transform[] mid, side;
        Transform[] randomMidPoint, randomSidePoint;

        void Start()
        {
            monsterStart();
            cd = 0;
            moveSpeed = 0.5f;
            monsterType = MonsterType.Slime;

            #region//決定範圍
            side = new Transform[68];
            mid = new Transform[12];
            int sideNum = 0, midNum = 0;
            for(int i = 0; i < MazeGen.row; i++)
            {
                for(int j = 0; j < MazeGen.col; j++)
                {
                    if (i < 2 || j < 2 || i > MazeGen.row - 3 || j > MazeGen.col - 3)
                    {
                        side[sideNum++] = GameManager.Floors.GetChild(i * MazeGen.col + j);
                    }
                    if(i > 3 && j > 2 && i < MazeGen.row - 4 && j < MazeGen.col - 4)
                    {
                        mid[midNum++] = GameManager.Floors.GetChild(i * MazeGen.col + j);
                    }
                }
            }
            #endregion
            Invoke("reNavigate", 0.01f);
        }

        void Update()
        {
            actionMode();

            monsterUpdate();
        }

        void actionMode()
        {
            Transform[] end = new Transform[GameManager.Players.childCount];
            for (int i = 0; i < end.Length; i++)
            {
                end[i] = GameManager.Players.GetChild(i);
            }
            straightTarget = StraightLineNearest(end);
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
            bool DodgeStone = false;
            List<Transform> noneStoneway = new List<Transform>();
            foreach(Transform child in GameManager.Floors)
            {
                noneStoneway.Add(child);
            }
            for (int i = 0; i < stoneway.Count; i++)
            {
                noneStoneway.Remove(GameManager.Floors.GetChild(stoneway[i][0] * MazeGen.col + stoneway[i][1]));
                if (stoneway[i][0] == startRow && stoneway[i][1] == startCol)
                {
                    DodgeStone = true;
                }
            }
            if (DodgeStone)
            {
                GoNavigate(Navigate(noneStoneway.ToArray(), null));
            }
            //距離玩家很遠，安心走自己的
            else if (straightTarget.Distance > 3)
            {
                if(sideTarget == null)
                {
                    randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
                    sideTarget = Navigate(randomSidePoint, side);
                }
                GoNavigate(sideTarget);
                if (origenStat != OrigenStat.side)
                {
                    randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
                    randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
                    sideTarget = Navigate(randomSidePoint, side);
                    origenStat = OrigenStat.side;
                }
            }
            //距離玩家太近，逃向中央
            else
            {
                if (midTarget == null)
                {
                    randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
                    midTarget = Navigate(randomMidPoint, mid);
                }
                GoNavigate(midTarget);
                if (origenStat != OrigenStat.mid)
                {
                    randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
                    randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
                    midTarget = Navigate(randomMidPoint, mid);
                    origenStat = OrigenStat.mid;
                }
            }
        }

        protected override float willMeetPlayer(float dis)
        {
            dis += 50;
            return dis;
        }

        protected override void GoNextRoad()
        {
            sideTarget =  Navigate(randomSidePoint, side);
            midTarget = Navigate(randomMidPoint, mid);
        }

        protected override NearestEnd GoNextTarget()
        {
            randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
            randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
            if (straightTarget.Distance > 3)
            {
                sideTarget = Navigate(randomSidePoint, side);
                return sideTarget;
            }
            //距離玩家太近，逃向中央
            else
            {
                midTarget = Navigate(randomMidPoint, mid);
                return midTarget;
            }
        }

        protected override float priority(float dis, int nextRow, int nextCol)
        {
            //距離玩家還很遠就盡量不要進中心
            if (straightTarget.Distance > 3)
            {
                if (nextRow < MazeGen.row - 3 && nextRow >= 3 && nextCol < MazeGen.col - 3 && nextCol >= 3)
                {
                    dis += 10;
                }
            }
            //距離玩家很近就避開會面向玩家的道路
            else
            {
                float minDis = 99999;
                Vector3 nextPos = new Vector3(nextRow * 2 + 1, nextCol * 2 + 1);
                foreach (Transform player in GameManager.Players)
                {
                    if (minDis > Vector3.Distance(nextPos, player.position))
                    {
                        minDis = Vector3.Distance(nextPos, player.position);
                    }
                }
                dis += 5 / minDis;
            }
            return dis;
        }

        void reNavigate()
        {
            randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
            randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
            sideTarget = Navigate(randomSidePoint, side);
            midTarget = Navigate(randomMidPoint, mid);
            InvokeRepeating("Re", 0.5f, 0.5f);
        }

        private void Re()
        {
            sideTarget = Navigate(randomSidePoint, side);
            midTarget = Navigate(randomMidPoint, mid);
        }

        protected override void afterDied()
        {
            attack();
        }

        protected override void attack()
        {
            //生成攻擊在自己腳下
            GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, Quaternion.identity);
            attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], null);
        }
    }
}