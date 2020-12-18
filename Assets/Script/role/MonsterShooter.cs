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
            if (collider.gameObject.layer == 8 || collider.gameObject.layer == 12)
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
}