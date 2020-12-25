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
                PlayerManager playerManager = collider.GetComponent<PlayerManager>();
                if (playerManager.SleepTimer < 0)
                {
                    playerManager.SleepTimer -= 3f;
                }
                else
                {
                    playerManager.SleepTimer = -3f;
                }
                if (playerManager.SleepTimer < -8)
                {
                    playerManager.SleepTimer = -8;
                }
                //PlayerManager.HP -= ATKforPlayer;
                collider.GetComponent<PlayerJoyVibration>().hurt();
            }
            if (collider.GetComponent<TaurenBoss>())
            {
                speed = 0;
                Destroy(gameObject, destoryTime);
                GetComponent<Collider2D>().enabled = false;
                //collider.GetComponent<MonsterManager>().HP -= ATKforBoss;
                TaurenBoss taurenBoss = collider.GetComponent<TaurenBoss>();
                if (taurenBoss.SleepTimer < 0)
                {
                    taurenBoss.SleepTimer -= 3f;
                }
                else
                {
                    taurenBoss.SleepTimer = -3f;
                }
                if (taurenBoss.SleepTimer < -8)
                {
                    taurenBoss.SleepTimer = -8;
                }
                Debug.LogError("BOSS" + collider.GetComponent<TaurenBoss>().SleepTimer);
            }
        }
    }
}
