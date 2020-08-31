using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        }

        void Update()
        {
            //身邊有敵人就攻擊，沒有就尋路
            Transform[] end = new Transform[GameManager.Players.childCount];
            for (int i = 0; i < end.Length; i++)
            {
                end[i] = GameManager.Players.GetChild(i);
            }
            straightTarget = StraightLineNearest(end);
            if (straightTarget.Distance > 3)
            {
                navigationNearestPlayer(new Transform[1] { range[rangeTargetNum] }, range);
                print("導航 : " + navigateTarget.Distance + " , " + navigateTarget.roadTraget.name);
            }
            else
            {
                Vector3 dirM = (straightTarget.roadTraget.position * Vector2.one - transform.position * Vector2.one).normalized * Time.deltaTime;
                if (dirM.magnitude > Vector3.Distance(straightTarget.roadTraget.position * Vector2.one, transform.position * Vector2.one))
                {
                    transform.position = straightTarget.roadTraget.position;
                }
                else
                {
                    transform.position = transform.position + dirM;
                }
                print("直行 : " + straightTarget.Distance + " , " + straightTarget.roadTraget.name);
            }

            monsterUpdate();
            if (Input.anyKeyDown)
            {
                attack();
            }

        }

        override protected void navigationNearestPlayer(Transform[] end, Transform[] range)
        {

            if ((navigationTimer += Time.deltaTime) > navigationTimerStoper)
            {
                navigateTarget = navigation(end, range);
                navigationTimerStoper = Random.Range(navigationTimerStoperMax, navigationTimerStoperMin);
                navigationTimer = 0;
            }
            if (navigateTarget != null)
            {
                if (navigateTarget.roadTraget != null)
                {
                    Vector3 dirM = (navigateTarget.roadTraget.position * Vector2.one - transform.position * Vector2.one).normalized * Time.deltaTime;
                    //到達定點則重開導航
                    if (dirM.magnitude > Vector3.Distance(navigateTarget.roadTraget.position * Vector2.one,transform.position * Vector2.one))
                    {
                        transform.position = navigateTarget.roadTraget.position;
                        Debug.LogError("!!!!!!!!!!!!!!!!!!!!!");
                        navigateTarget = navigation(end, range);
                        navigationTimerStoper = Random.Range(navigationTimerStoperMax, navigationTimerStoperMin);
                        navigationTimer = 0;
                        if (range[rangeTargetNum] == navigateTarget.endTraget)
                        {
                            navigateNextPoint();
                        }
                    }
                    else
                    {
                        transform.position = transform.position + dirM;
                    }
                }
                else
                {
                    navigateNextPoint();
                    Debug.LogError("RRRR");
                }
            }
            else
            {
                navigateNextPoint();
                Debug.LogError("RRRR");
            }
        }
        protected override void navigateNextPoint()
        {
            int temp = rangeTargetNum;
            do
            {
                rangeTargetNum = Random.Range(0, range.Length);
            } while (temp == rangeTargetNum);
            print(range[rangeTargetNum].name);
            navigationTimer = 0;
            navigateTarget = navigation(new Transform[1] { range[rangeTargetNum] }, range);
        }
        override protected void attack()
        {
            //生成攻擊在觸控方向，並旋轉攻擊朝向該方向
            float angle = Vector3.SignedAngle(Vector3.right, navigateTarget.endTraget.position * Vector2.one - transform.position * Vector2.one, Vector3.forward);
            GameObject attack = Instantiate(MonsterAttack[(int)monsterType], transform.position, Quaternion.Euler(0, 0, angle));
            //設定攻擊參數
            attack.GetComponent<AttackManager>().setValue(ATK[(int)monsterType, 0], duration[(int)monsterType], continuous[(int)monsterType], false);
        }
    }
}