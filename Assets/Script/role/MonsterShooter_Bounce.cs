using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterShooter_Bounce : MonoBehaviour
    {
        public float speed, timer, timerStoper, destoryTime;
        public AudioSource hitSound;
        public Animator animator;
        public bool canRemoveByPlayerAttack = true;

        [SerializeField]Transform sprite;

        void Start()
        {

        }

        void Update()
        {
            //sprite.right = Vector3.right;
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

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == 12)
            {
                float angle = Vector3.SignedAngle(Vector3.right,Vector2.Reflect(transform.right, collision.GetContact(0).normal),Vector3.forward);
                transform.eulerAngles = new Vector3(0, 0, angle);
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
            }
        }
    }
}