using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class SlimeManager : MonsterManager
    {
        Transform[] mid, side;
        int midTargetNum, sideTargetNum;

        void Start()
        {
            monsterStart();
            cd = 0;
            monsterType = MonsterType.Slime;
            #region//決定範圍
            side = new Transform[MazeGen.row*MazeGen.col - 15];
            mid = new Transform[15];
            for (int i = 0; i < mid.Length+side.Length; i++)
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
                        else
                        {
                            if (!mid.Contains(child) && !side.Contains(child))
                            {
                                min = Vector3.Distance(transform.position, child.position);
                                nearestFloor = child;
                            }
                        }
                    }
                }
                if (i < mid.Length)
                {
                    mid[i] = nearestFloor;
                }
                else
                {
                    side[i-mid.Length] = nearestFloor;
                }
            }
            #endregion
        }

        void Update()
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
                Debug.LogError("安心");
                //goNavigationNearest(new Transform[1] { side[sideTargetNum] }, side, sideTargetNum);
            }
            //距離玩家太近，逃向中央
            else
            {
                Debug.LogError("逃命");
                //goNavigationNearest(new Transform[1] { mid[midTargetNum] }, mid, midTargetNum);
            }

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
    }
}