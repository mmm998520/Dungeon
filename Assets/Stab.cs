using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Stab : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

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
            bool up = false, right = false, down = false, left = false;
            if((int)MinDisPlayer().position.x < (int)transform.position.x)
            {
                left = true;
            }
            else if((int)MinDisPlayer().position.x > (int)transform.position.x)
            {
                right = true;
            }
            if ((int)MinDisPlayer().position.y < (int)transform.position.y)
            {
                down = true;
            }
            else if ((int)MinDisPlayer().position.y > (int)transform.position.y)
            {
                up = true;
            }

            int r = Random.Range(0, 2);
            if (up && right)
            {
                if (r < 1)
                {
                    transform.position += Vector3.up;
                }
                else
                {
                    transform.position += Vector3.right;
                }
            }
            else if(up && left)
            {
                if (r < 1)
                {
                    transform.position += Vector3.up;
                }
                else
                {
                    transform.position += Vector3.left;
                }
            }
            else if (down && right)
            {
                if (r < 1)
                {
                    transform.position += Vector3.down;
                }
                else
                {
                    transform.position += Vector3.right;
                }
            }
            else if (down && left)
            {
                if (r < 1)
                {
                    transform.position += Vector3.down;
                }
                else
                {
                    transform.position += Vector3.left;
                }
            }
            else if (up)
            {
                transform.position += Vector3.up;
            }
            else if (right)
            {
                transform.position += Vector3.right;
            }
            else if (left)
            {
                transform.position += Vector3.left;
            }
            else if (down)
            {
                transform.position += Vector3.down;
            }
        }
    }
}