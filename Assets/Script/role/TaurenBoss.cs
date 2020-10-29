using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenBoss : MonsterManager
    {
        Rigidbody2D rigidbody;
        public GameObject Axe, Tauren;
        public Transform InsAxePos, center;
        public bool canWalk = false , canPunch = true;
        public int punching = 0;
        public Animator animator, roomAnimator;
        float timer;

        void Start()
        {
            speed = 1;
            rigidbody = GetComponent<Rigidbody2D>();
            ArmorBar = transform.GetChild(3);
        }

        void Update()
        {
            timer = roomAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime * 60;
            while (timer >= 60)
            {
                timer -= 60;
            }
            print(timer);
            if (canWalk)
            {
                if (punching == 1 && canPunch)
                {
                    rigidbody.velocity = Vector2.zero;
                }
                else if (punching == 2 && canPunch)
                {
                    rigidbody.velocity = transform.right * 5 * speed;
                }
                //如果回歸中點的時間不足(不足再讓BOSS亂逛)時向中點移動
                else if((32/* 抵達時間(33) - 容錯值(1) */ - timer) <= Vector2.Distance(transform.position, center.position) / speed)
                {
                    if(Vector2.Distance(transform.position, center.position) > 0.01f)
                    {
                        rigidbody.velocity = Vector3.Normalize((center.position - transform.position) * Vector2.one) * speed;
                        //一但觸發回程，就不會使用揮拳了
                        canPunch = false;
                        Debug.LogWarning("回程");
                    }
                    else
                    {
                        rigidbody.velocity = Vector2.zero;
                    }
                }
                else if (punching == 0)
                {
                    rigidbody.velocity = Vector3.Normalize((MinDisPlayerCube().position - transform.position) * Vector2.one) * speed;
                }
                 
                if (rigidbody.velocity.x > 0)
                {
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                }
                if (rigidbody.velocity.x < 0)
                {
                    transform.rotation = Quaternion.Euler(Vector3.up * 180);
                }
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }

            ArmorBar.gameObject.SetActive(Armor > 0);
            ArmorBar.localScale = Vector3.one * ((Armor / MaxArmor) * 0.6f + 0.4f);
        }

        /// <summary> 丟斧頭 </summary>
        public void throwAxe()
        {
            float angle = Vector3.SignedAngle(Vector3.right, (MinDisPlayer().position - transform.position) * Vector2.one, Vector3.forward);
            Instantiate(Axe, InsAxePos.position, Quaternion.Euler(Vector3.forward * angle));
        }

        public void useArmor(int cost)
        {
            Armor -= cost;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PlayerCube" && canWalk && canPunch)
            {
                animator.SetTrigger("Punch");
            }
            if (collision.GetComponent<PlayerManager>())
            {
                collision.GetComponent<PlayerManager>().a += (Vector2)Vector3.Normalize((collision.transform.position - transform.position) * Vector2.one);
                print("player");
            }
        }
    }
}