using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Stab : MonoBehaviour
    {
        Animator animator;
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("idle"))
            {
                print("yes");
                follow();
            }
            else
            {
                print("no");
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

        void randomPos()
        {
            transform.position = new Vector2(Random.Range(1, 21), Random.Range(1, 11));
        }

        void follow()
        {
            Vector3 dir = MinDisPlayer().position - transform.position;
            if (Vector3.Magnitude(dir) > Time.deltaTime * 3)
            {
                transform.Translate(Vector3.Normalize(dir) * Time.deltaTime * 3);
            }
            else
            {
                transform.Translate(dir);
            }
        }
    }
}