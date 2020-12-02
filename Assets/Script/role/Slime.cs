using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Slime : MonsterManager
    {
        public enum SlimeBehavior
        {
            ramdomMove = 0,
            stop = 1
        }
        public SlimeBehavior slimeBehavior;

        bool stoping = false;
        public Animator SpriteAnimator;

        void Start()
        {
            ReCD();
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
            randomTarget();
        }

        void Update()
        {
            if (slimeBehavior == SlimeBehavior.ramdomMove)
            {
                if ((CDTimer += Time.deltaTime) >= CD)
                {
                    if (!stoping)
                    {
                        stoping = true;
                        GetComponent<Animator>().SetTrigger("Stop");
                    }
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.angularDrag = 0;
                    SpriteAnimator.SetBool("Stop", true);
                }
                else
                {
                    Move();
                    Stuck(1, 1);
                    stoping = false;
                    SpriteAnimator.SetBool("Stop", false);
                }
            }
        }

        bool Blew = false;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<PlayerManager>())
            {
                collision.collider.GetComponent<PlayerManager>().StickTimer = 0;
                attackSource.Play();
                attackSource.transform.parent = null;
                Destroy(gameObject);
                Blew = true;
                Destroy(attackSource.gameObject, 5);
            }
        }

        public override void beforeDied()
        {
            base.beforeDied();
            if (!Blew)
            {
                GameManager.KillSpider++;
                SpriteAnimator.transform.parent = null;
                SpriteAnimator.SetTrigger("Destroy");
            }
        }
    }
}