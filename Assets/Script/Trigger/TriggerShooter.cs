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
                animator.SetTrigger("Hit");
                hitSound.Play();
                hitSound.transform.parent = null;
                Destroy(gameObject, destoryTime);
                Destroy(hitSound.gameObject, 5);

                PlayerManager.HP -= ATKforPlayer;
                collider.GetComponent<PlayerJoyVibration>().hurt();
            }
            if (collider.GetComponent<MonsterManager>())
            {
                collider.GetComponent<MonsterManager>().HP -= ATKforBoss;
                if (collider.GetComponent<MonsterManager>().HP <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
