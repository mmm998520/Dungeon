using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace com.BoardGameDungeon
{
    public class Tauren : MonsterManager
    {
        Transform[] range;
        int rangeTargetNum = 0;

        void Start()
        {
            monsterStart();
            cd = 2;
            monsterType = MonsterType.Tauren;
            #region//決定守衛範圍
            range = new Transform[15];
            for(int i = 0; i < range.Length; i++)
            {
                float min = float.MaxValue;
                Transform nearestFloor = GameManager.Floors.GetChild(0);
                foreach(Transform child in GameManager.Floors)
                {
                    if(min > Vector3.Distance(transform.position, child.position))
                    {
                        if (!range.Contains(child))
                        {
                            min = Vector3.Distance(transform.position, child.position);
                            nearestFloor = child;
                        }
                    }

                }
                range[i] = nearestFloor;
            }
            #endregion
        }

        void Update()
        {
            //actionMode();
            attackOccasion(straightTarget, 2.5f);

            monsterUpdate();
            if (Input.anyKeyDown)
            {
                attack();
            }
        }
        /*
        override protected void goNavigationNearest(Transform[] end, Transform[] range)
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
                    if (dirM.magnitude > Vector3.Distance(navigateTarget.roadTragets.position * Vector2.one,transform.position * Vector2.one))
                    {
                        transform.position = navigateTarget.roadTragets.position;
                        Debug.LogError("!!!!!!!!!!!!!!!!!!!!!");
                        navigateTarget = navigation(end, range);
                        navigationTimerStoper = Random.Range(navigationTimerStoperMax, navigationTimerStoperMin);
                        navigationTimer = 0;
                        if (range[rangeTargetNum] == navigateTarget.endTraget)
                        {
                            navigateNextPoint(range, rangeTargetNum);
                        }
                    }
                    else
                    {
                        transform.position = transform.position + dirM;
                    }
                }
                else
                {
                    navigateNextPoint(range, rangeTargetNum);
                    Debug.LogError("RRRR");
                }
            }
            else
            {
                navigateNextPoint(range, rangeTargetNum);
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
        override protected void attack()
        {
            //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
            float angle = Vector3.SignedAngle(Vector3.right, straightTarget.endTraget.position * Vector2.one - transform.position * Vector2.one, Vector3.forward);
            GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, Quaternion.Euler(0, 0, angle));
            //設定攻擊參數
            attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], false);
        }
        /*
        /// <summary> 決定要追最近敵人還是巡邏 </summary>
        void actionMode()
        {
            //身邊有敵人就攻擊，沒有就尋路
            Transform[] end = new Transform[GameManager.Players.childCount];
            for (int i = 0; i < end.Length; i++)
            {
                end[i] = GameManager.Players.GetChild(i);
            }
            straightTarget = StraightLineNearest(end);
            //附近沒敵人，守家
            if (straightTarget.Distance > 3)
            {
                goNavigationNearest(new Transform[1] { range[rangeTargetNum] }, range);
                print("導航 : " + navigateTarget.Distance + " , " + navigateTarget.roadTragets.name);
            }
            else
            {
                //附近有敵人，但我超過守備範圍了，回家
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
                if (!range.Contains(GameManager.Floors.GetChild(startRow * MazeGen.col + startCol)))
                {
                    goNavigationNearest(new Transform[1] { range[rangeTargetNum] }, range);
                    print("導航 : " + navigateTarget.Distance + " , " + navigateTarget.roadTragets.name);
                }
                //附近有敵人，追擊
                else
                {
                    Vector3 dirM = (straightTarget.roadTragets.position * Vector2.one - transform.position * Vector2.one).normalized * Time.deltaTime;
                    if (dirM.magnitude > Vector3.Distance(straightTarget.roadTragets.position * Vector2.one, transform.position * Vector2.one))
                    {
                        transform.position = straightTarget.roadTragets.position;
                    }
                    else
                    {
                        transform.position = transform.position + dirM;
                    }
                    print("直行 : " + straightTarget.Distance + " , " + straightTarget.roadTragets.name);
                }
            }
        }*/
    }
}