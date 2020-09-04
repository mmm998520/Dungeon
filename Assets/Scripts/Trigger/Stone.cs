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

        void Update()
        {
            transform.Translate(dir * Time.deltaTime * speed);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, transform.lossyScale.x,1<<8);
            if (hit)
            {
                if (hit.collider.tag == "wall")
                {
                    Destroy(hit.collider.gameObject);
                }
                else if (hit.collider.tag == "side")
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<ValueSet>())
            {
                collider.GetComponent<ValueSet>().Hurt += ATK;
            }
            else if(collider.tag == "wall")
            {
                Destroy(collider.gameObject);
            }
            else if(collider.tag == "side")
            {
                Destroy(gameObject);
            }
            MonsterManager.addHurtMe(collider, user);
        }
    }
}
