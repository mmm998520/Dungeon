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
        }

        void Update()
        {
            if(spiderBehavior == SpiderBehavior.ramdomMove)
            {
                Move();
                Stuck(2, 1);
                attacking = false;
                SpriteAnimator.SetBool("Stop", false);
            }
            else
            {
                attack();
            }
        }

        public new void attack()
        {
            if (!attacking)
            {
                attacking = true;
                GetComponent<Animator>().SetTrigger("Attack");
            }
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularDrag = 0;
            if (MinDisPlayer().transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (MinDisPlayer().transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            SpriteAnimator.SetBool("Stop", true);
        }

        public override void beforeDied()
        {
            GameManager.KillSpider++;
            SpriteAnimator.transform.parent = null;
            SpriteAnimator.SetTrigger("Destroy");
        }
    }
}