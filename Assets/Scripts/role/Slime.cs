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
            side = new Transform[MazeGen.row*MazeGen.col - 15];
            mid = new Transform[15];
            for (int i = 0; i < mid.Length; i++)
            {
                float min = float.MaxValue;
                Transform nearestFloor = GameManager.Floors.GetChild(0);
                foreach (Transform child in GameManager.Floors)
                {
                    if (min > Vector3.Distance(transform.position, child.position))
                    {
                        if (i < mid.Length)
                        {
                            if (!mid.Contains(child))
                            {
                                min = Vector3.Distance(transform.position, child.position);
                                nearestFloor = child;
                            }
                        }
                    }
                }
                mid[i] = nearestFloor;
            }
            for (int i = 0; i < side.Length; i++)
            {
                foreach (Transform child in GameManager.Floors)
                {
                    if (!mid.Contains(child) && !side.Contains(child))
                    {
                        side[i] = child;
                        break;
                    }
                }
            }
            #endregion
            if(side[side.Length - 1]== null)
            {
                Debug.LogError("side沒裝滿");
            }
            Invoke("reNavigate", 0.01f);
        }

        void Update()
        {
            actionMode();

            monsterUpdate();
            if (Input.anyKeyDown)
            {
                attack();
            }
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
    }
}