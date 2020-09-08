using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Stone : MonoBehaviour
    {
        public Vector3 dir;
        float speed = 3;
        float ATK = 5;
        
        public PlayerManager user;
        public AudioSource stoneAudio;

        void Start()
        {
            stoneAudio.Play();
        }

        void Update()
        {
            transform.Translate(dir * Time.deltaTime * speed);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, transform.lossyScale.x / 2, 1<<8);
            Vector3 tempDir = Quaternion.Euler(0, 0, 90) * dir.normalized;
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position + tempDir, dir, transform.lossyScale.x / 10, 1<<8);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position - tempDir, dir, transform.lossyScale.x / 10, 1<<8);
            if (hit)
            {
                if (hit.collider.tag == "side")
                {
                    Destroy(gameObject);
                }
                else if (hit.collider.tag == "wall")
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            if (hit1)
            {
                if (hit1.collider.tag == "wall")
                {
                    Destroy(hit1.collider.gameObject);
                }
            }
            if (hit2)
            {
                if (hit2.collider.tag == "wall")
                {
                    Destroy(hit2.collider.gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<ValueSet>())
            {
                collider.GetComponent<ValueSet>().Hurt += ATK;
            }
            MonsterManager.hurtMe(collider, user, false);
        }
    }
}