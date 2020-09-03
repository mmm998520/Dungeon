using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace com.BoardGameDungeon
{
    public class Tauren : MonsterManager
    {
        /// <summary> 守衛範圍 </summary>
        public Transform[] range;
        Transform[] randomRangePoint;
        NearestEnd target;
        public List<int> rangeRow = new List<int>();
        public List<int> rangeCol = new List<int>();

        void Start()
        {
            monsterStart();
            cd = 2;
            moveSpeed = 1;
            monsterType = MonsterType.Tauren;

            Invoke("reNavigate", 0.01f);
        }

        void Update()
        {
            actionMode();
            attackOccasion(straightTarget, 2.5f);

            monsterUpdate();
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
                GoNavigate(target);
                if (target == null)
                {
                    Debug.LogError("a");
                }
                //print("導航 : " + target.Distance + " , " + target.endTraget.name);
            }
            else
            {
                //附近有敵人，但超過守備範圍，回家
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
                    GoNavigate(target);
                    print("導航 : " + target.Distance + " , " + target.endTraget.name);
                }
                //附近有敵人，追擊
                else
                {
                    Vector3 endPos = straightTarget.endTraget.position * Vector2.one;
                    Vector3 dir = endPos * Vector2.one - transform.position * Vector2.one;
                    //單位時間移動量
                    float dis = Time.deltaTime * moveSpeed;
                    if(dis > dir.magnitude)
                    {
                        transform.position = endPos + transform.position.z * Vector3.forward;
                    }
                    else
                    {
                        transform.Translate(dis * dir.normalized);
                    }
                    print("直行 : " + straightTarget.Distance + " , " + straightTarget.endTraget.name);
                }
            }
        }

        protected override void GoNextRoad()
        {
            target = Navigate(randomRangePoint, range);
        }

        protected override NearestEnd GoNextTarget()
        {
            randomRangePoint = new Transform[1] { range[Random.Range(0, range.Length)] };
            target = Navigate(randomRangePoint, range);
            return target;
        }

        //牛頭人優先走中心區域
        protected override float priority(float dis, int nextRow, int nextCol)
        {
            if (!(nextRow < MazeGen.row - 3 && nextRow >= 3 && nextCol < MazeGen.col - 3 && nextCol >= 3))
            {
                dis += 1000;
            }
            return dis;
        }

        void reNavigate()
        {
            randomRangePoint = new Transform[1] { range[Random.Range(0, range.Length)] };
            target = Navigate(randomRangePoint, range);
            InvokeRepeating("Re", 0.5f, 0.5f);
        }

        private void Re()
        {
            target = Navigate(randomRangePoint, range);
        }
    }
}