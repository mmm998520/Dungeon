using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spider : MonsterManager
    {
        public enum SpiderBehavior
        {
            ramdomMove = 0,
            attack = 1
        }
        public SpiderBehavior spiderBehavior;

        bool attacking = false;
        float timer;
        void Start()
        {
            ReCD();
            RepTime = new WaitForSeconds(repTimer);
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
            randomTarget();
            ArmorBar = transform.GetChild(1);
            findRoadWait = new WaitForSeconds(Random.Range(0.3f, 0.5f));
        }

        void Update()
        {
            if(spiderBehavior == SpiderBehavior.ramdomMove)
            {
                Move();
                if ((CDTimer += Time.deltaTime) >= CD && Vector3.Distance(transform.position * Vector2.one, MinDisPlayer().position * Vector2.one) < hand)
                {
                    if (!attacking)
                    {
                        attacking = true;
                        GetComponent<Animator>().SetTrigger("Attack");
                    }
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                }
                else
                {
                    attacking = false;
                }
            }
            ArmorBar.gameObject.SetActive(Armor > 0);
            ArmorBar.localScale = Vector3.one * ((Armor / MaxArmor) * 0.6f + 0.4f);
        }

        void randomTarget()
        {
            int r = Random.Range(0, canGo.Count);
            endRow = new int[] { canGo[r] / MazeCreater.totalCol };
            endCol = new int[] { canGo[r] % MazeCreater.totalCol };
        }

        void randomMove()
        {
            if (roads.Count == 0)
            {
                randomTarget();
                findRoad();
            }
            else
            {
                Vector3 nextPos = new Vector3(roads[roads.Count - 1][0], roads[roads.Count - 1][1]);
                GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(nextPos - transform.position) * speed;
                if (Vector3.Distance(transform.position, nextPos) < 0.3f)
                {
                    roads.RemoveAt(roads.Count - 1);
                }
            }
        }

        void Move()
        {
            if (TauntTarge == null)
            {
                randomMove();
            }
            else if ((timer += Time.deltaTime) > 0.3f)
            {
                timer = 0;
                findRoad();
                Vector3 nextPos = new Vector3(roads[roads.Count - 1][0], roads[roads.Count - 1][1]);
                GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(nextPos - transform.position) * speed;
                if (Vector3.Distance(transform.position, nextPos) < 0.3f)
                {
                    roads.RemoveAt(roads.Count - 1);
                }
            }
            if (GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public void ReCD()
        {
            CDTimer = 0;
            CD = Random.Range(CDMin, CDMax);
        }

        protected override void afterAttack()
        {
            ArmorReCharge();
        }
    }
}