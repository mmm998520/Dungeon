using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Bullet : MonoBehaviour
    {
        public float speed;
        void Start()
        {
            Destroy(gameObject, 1);
        }

        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.layer == 9 || collider.gameObject.layer == 12)
            {
                if (collider.GetComponent<MonsterManager>())
                {
                    Debug.LogWarning("hitTimes");
                    PlayerManager.HP += 25;
                    Players.reTimer = 0;
                    collider.GetComponent<MonsterManager>().beforeDied();
                    Destroy(collider.gameObject);
                }
                Destroy(gameObject);
            }
        }
    }
}