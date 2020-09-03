using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Slime : MonsterManager
    {
        //當前移動目標
        NearestEnd target;
        Transform[] range;
        Transform[] randomRangePoint;
        //定義移動區域
        Transform[] mid, side;
        Transform[] randomMidPoint, randomSidePoint;

        void Start()
        {
            monsterStart();
            cd = 0;
            moveSpeed = 1;
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

        /*
                void goNavigationNearest(Transform[] end, Transform[] range, int nextTargetNum)
                {
                    if ((navigationTimer += Time.deltaTime) > navigationTimerStoper)
                    {
                        navigateTarget = navigation(end, range);
                        navigationTimerStoper = Random.Range(navigationTimerStoperMax, navigationTimerStoperMin);
                        navigationTimer = 0;
                    }
                    if (navigateTarget != null)
                    {
                        if (navigateTarget.roadTragets != null)
                        {
                            Vector3 dirM = (navigateTarget.roadTragets.position * Vector2.one - transform.position * Vector2.one).normalized * Time.deltaTime;
                            //到達定點則重開導航
                            if (dirM.magnitude > Vector3.Distance(navigateTarget.roadTragets.position * Vector2.one, transform.position * Vector2.one))
                            {
                                transform.position = navigateTarget.roadTragets.position;
                                Debug.LogError("!!!!!!!!!!!!!!!!!!!!!");
                                navigateTarget = navigation(end, range);
                                navigationTimerStoper = Random.Range(navigationTimerStoperMax, navigationTimerStoperMin);
                                navigationTimer = 0;
                                if (range[midTargetNum] == navigateTarget.endTraget)
                                {
                                    navigateNextPoint(range, nextTargetNum);
                                }
                            }
                            else
                            {
                                transform.position = transform.position + dirM;
                            }
                        }
                        else
                        {
                            navigateNextPoint(range, nextTargetNum);
                            Debug.LogError("RRRR");
                        }
                    }
                    else
                    {
                        navigateNextPoint(range, nextTargetNum);
                        Debug.LogError("RRRR");
                    }
                }

                override protected void navigateNextPoint(Transform[] range, int nextTargetNum)
                {
                    int temp = nextTargetNum;
                    do
                    {
                        nextTargetNum = Random.Range(0, range.Length);
                    } while (temp == nextTargetNum);
                    print(range[nextTargetNum].name);
                    navigationTimer = 0;
                    navigateTarget = navigation(new Transform[1] { range[nextTargetNum] }, range);
                }*/
        void actionMode()
        {
            Transform[] end = new Transform[GameManager.Players.childCount];
            for (int i = 0; i < end.Length; i++)
            {
                end[i] = GameManager.Players.GetChild(i);
            }
            straightTarget = StraightLineNearest(end);
            //距離玩家很遠，安心走自己的
            if (straightTarget.Distance > 3)
            {
                range = side;
                randomRangePoint = randomSidePoint;
                target = goNavigation(randomRangePoint, null, navigation(randomRangePoint, null));
                //goNavigationNearest(new Transform[1] { side[sideTargetNum] }, side, sideTargetNum);
            }
            //距離玩家太近，逃向中央
            else
            {
                range = mid;
                randomRangePoint = randomMidPoint;
                target = goNavigation(randomRangePoint, null, navigation(randomRangePoint, null));
                //goNavigationNearest(new Transform[1] { mid[midTargetNum] }, mid, midTargetNum);
            }
        }

        void reNavigate()
        {
            randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
            randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
            range = side;
            randomRangePoint = randomSidePoint;
            target = navigation(randomRangePoint, range);
        }

        protected override NearestEnd navigateNextPoint(Transform[] end, Transform[] range, NearestEnd nearestEnd)
        {
            randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
            randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
            if (straightTarget.Distance > 3)
            {
                range = side;
                randomRangePoint = randomSidePoint;
            }
            //距離玩家太近，逃向中央
            else
            {
                range = mid;
                randomRangePoint = randomMidPoint;
            }

            target = navigation(randomRangePoint, range);
            return target;
        }
        protected override NearestEnd nearby(Transform[] end)
        {
            randomSidePoint = new Transform[1] { side[Random.Range(0, side.Length)] };
            randomMidPoint = new Transform[1] { mid[Random.Range(0, mid.Length)] };
            if (straightTarget.Distance > 3)
            {
                range = side;
                randomRangePoint = randomSidePoint;
            }
            //距離玩家太近，逃向中央
            else
            {
                range = mid;
                randomRangePoint = randomMidPoint;
            }
            
            target = navigation(randomRangePoint, range);
            return target;
        }

        protected override void afterDied()
        {
            attack();
        }

        protected override void attack()
        {
            //生成攻擊在自己腳下
            GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, Quaternion.identity);
            attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], false);
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
                    if(minDis > Vector3.Distance(nextPos, player.position))
                    {
                        minDis = Vector3.Distance(nextPos, player.position);
                    }
                }
                dis += 5 / minDis;
            }
            return dis;
        }
    }
}