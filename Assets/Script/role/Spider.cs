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
        public Animator SpriteAnimator;

        void Start()
        {
            ReCD();
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
                if ((CDTimer += Time.deltaTime) >= CD && Vector3.Distance(transform.position * Vector2.one, MinDisPlayer().position * Vector2.one) < hand)
                {
                    if (!attacking)
                    {
                        attacking = true;
                        GetComponent<Animator>().SetTrigger("Attack");
                    }
                    GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    GetComponent<Rigidbody2D>().angularDrag = 0;
                    SpriteAnimator.SetBool("Stop", true);
                }
                else
                {
                    Move();
                    Stuck(2, 1);
                    attacking = false;
                    SpriteAnimator.SetBool("Stop", false);
                }
            }
            ArmorBar.gameObject.SetActive(Armor > 0);
            ArmorBar.localScale = Vector3.one * ((Armor / MaxArmor) * 0.6f + 0.4f);
        }
    }
}