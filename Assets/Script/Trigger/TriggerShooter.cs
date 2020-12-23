using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TriggerShooter : MonoBehaviour
    {
        public float speed, timer, timerStoper, destoryTime;
        public AudioSource hitSound;
        public Animator animator;
        public bool canRemoveByPlayerAttack = true;
        public float ATKforPlayer, ATKforBoss;
        void Start()
        {

        }

        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            if ((timer += Time.deltaTime) > timerStoper)
            {
                speed = 0;
                animator.SetTrigger("Hit");
                hitSound.Play();
                hitSound.transform.parent = null;
                Destroy(gameObject, destoryTime);
                Destroy(hitSound.gameObject, 5);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                speed = 0;
                Destroy(gameObject, destoryTime);
                GetComponent<Collider2D>().enabled = false;
                if(collider.GetComponent<PlayerManager>().HardStraightTimer < 0)
                {
                    collider.GetComponent<PlayerManager>().HardStraightTimer -= 2f;
                }
                else
                {
                    collider.GetComponent<PlayerManager>().HardStraightTimer = -2f;
                }
                collider.GetComponent<PlayerManager>().HardStraightA = Vector3.zero;
                //PlayerManager.HP -= ATKforPlayer;
                collider.GetComponent<PlayerJoyVibration>().hurt();
            }
            if (collider.GetComponent<TaurenBoss>())
            {
                speed = 0;
                Destroy(gameObject, destoryTime);
                GetComponent<Collider2D>().enabled = false;
                //collider.GetComponent<MonsterManager>().HP -= ATKforBoss;
                if (collider.GetComponent<TaurenBoss>().HardStraightTimer < 0)
                {
                    collider.GetComponent<TaurenBoss>().HardStraightTimer -= 2f;
                }
                else
                {
                    collider.GetComponent<TaurenBoss>().HardStraightTimer = -2f;
                }
                Debug.LogError("BOSS" + collider.GetComponent<TaurenBoss>().HardStraightTimer);
            }
        }
    }
}
