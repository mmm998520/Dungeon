using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Money : MonoBehaviour
    {
        public Transform child;
        public Collider2D GetCollider;
        public Animator animator;
        float timer = 0;

        void Start()
        {
            transform.eulerAngles = new Vector3(0, Random.Range(0, 2) * 180, Random.Range(-20f,0f));
            transform.position += new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        }

        void Update()
        {
            if (GetCollider.enabled == true)
            {
                animator.enabled = false;
            }
            float distance = Vector3.Distance(child.position, MinDisPlayer().position);
            print(0);
            if (distance < 0.3f)
            {
                PlayerManager.money++;
                Destroy(gameObject);
                print(1);
            }
            else if (distance < 3)
            {
                child.position = child.position + Vector3.Normalize(MinDisPlayer().position - child.position) * 10 * Time.deltaTime;
                print(2);
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
                if (minDis > Vector3.Distance(player.position, child.position))
                {
                    minDis = Vector3.Distance(player.position, child.position);
                    minDisPlayer = player;
                }
            }
            return minDisPlayer;
        }
    }
}