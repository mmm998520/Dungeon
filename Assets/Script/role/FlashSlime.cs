using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace com.DungeonPad
{
    public class FlashSlime : MonsterManager
    {
        public enum SlimeBehavior
        {
            ramdomMove = 0,
            stop = 1
        }
        public SlimeBehavior slimeBehavior;

        bool stoping = false;
        public Animator SpriteAnimator;
        public float flashTimer;
        public Light2D light2D;

        void Start()
        {
            ReCD();
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
            randomTarget();
        }

        protected override void Update()
        {
            base.Update();
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

            if (Vector2.Distance(transform.position, MinDisPlayer().position) < 6)
            {
                flashTimer += Time.deltaTime;
                if (flashTimer >= 5)
                {
                    boom();
                    Destroy(gameObject);
                }
            }
            else
            {
                flashTimer = Mathf.Lerp(flashTimer, 0, Time.deltaTime * 5);
            }
            light2D.intensity = Mathf.Sin(Mathf.Pow(flashTimer, 3)) * 5;
        }

        void boom()
        {
            ShowFlashSlimeLight.LightTimer = 5;
        }
        /*
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
        */
        public override void beforeDied()
        {
            base.beforeDied();
            GameManager.KillSpider++;
            SpriteAnimator.transform.parent = null;
            SpriteAnimator.SetTrigger("Destroy");
        }
    }
}