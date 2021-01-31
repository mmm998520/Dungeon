using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterShooter : MonoBehaviour
    {
        public float speed, timer, timerStoper, destoryTime;
        public AudioSource hitSound;
        public Animator animator;
        public bool canRemoveByPlayerAttack = true;
        public bool focusPlayer;

        void Start()
        {

        }

        void Update()
        {
            if (focusPlayer)
            {
                transform.Translate(Vector3.Normalize(MinDisPlayer().position - transform.position) * Time.deltaTime * speed);
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
            else
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
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8 || collider.gameObject.layer == 12)
            {
                speed = 0;
                try
                {
                    animator.SetTrigger("Hit");
                    hitSound.Play();
                    hitSound.transform.parent = null;
                    Destroy(hitSound.gameObject, 5);
                }
                catch
                {

                }
                Destroy(gameObject, destoryTime);
            }
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == 8 || collision.collider.gameObject.layer == 12)
            {
                speed = 0;
                try
                {
                    animator.SetTrigger("Hit");
                    hitSound.Play();
                    hitSound.transform.parent = null;
                    Destroy(hitSound.gameObject, 5);
                }
                catch
                {

                }
                Destroy(gameObject, destoryTime);
            }
        }

        public Transform MinDisPlayer()
        {
            int i;
            float minDis = float.MaxValue;
            Transform minDisPlayer = null;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                if (minDis > Vector3.Distance(player.position, transform.position))
                {
                    minDis = Vector3.Distance(player.position, transform.position);
                    minDisPlayer = player;
                }
            }
            return minDisPlayer;
        }
    }
}