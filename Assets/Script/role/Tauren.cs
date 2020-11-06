using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Tauren : MonsterManager
    {
        Rigidbody2D rigidbody;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            rigidbody.velocity = Vector3.Normalize((MinDisPlayer().position - transform.position) * Vector2.one);
            if (rigidbody.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            if (rigidbody.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.up * 180);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerManager>())
            {
                collision.GetComponent<PlayerManager>().HardStraightA += (Vector2)Vector3.Normalize((collision.transform.position - transform.position) * Vector2.one);
                print("player");
            }
        }
    }
}