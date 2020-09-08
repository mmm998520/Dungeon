﻿using System.Collections;
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
            moveSpeed = 0.8f;
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
            if (paralysis)
            {
                return;
            }
            List<Transform> end = new List<Transform>();
            foreach (Transform player in GameManager.Players)
            {
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                if (!(playerManager.career == Career.Thief && playerManager.statOne))
                {
                    end.Add(player);
                }
            }
            straightTarget = StraightLineNearest(end.ToArray());
            //距離玩家很遠，安心走自己的
            if (straightTarget.Distance > 3)
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
                    PlayerManager playerManager = player.GetComponent<PlayerManager>();
                    if (!(playerManager.career == Career.Thief && playerManager.statOne))
                    {
                        if (minDis > Vector3.Distance(nextPos, player.position))
                        {
                            minDis = Vector3.Distance(nextPos, player.position);
                        }
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
        /*
        private void OnCollisionStay2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "monster")
            {
                reNavigate();
            }
        }
        */
    }
}