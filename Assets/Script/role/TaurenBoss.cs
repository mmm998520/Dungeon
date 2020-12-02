using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenBoss : MonsterManager
    {
        Transform minDisPlayer;
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            addCanGoByHand();
            endRow = new int[1];
            endCol = new int[1];
        }

        private void Update()
        {
            resetRoad();
            move();
            speed = 1;
        }

        void resetRoad()
        {
            minDisPlayer = MinDisPlayer();
            endRow[0] = Mathf.RoundToInt(minDisPlayer.position.x);
            endCol[0] = Mathf.RoundToInt(minDisPlayer.position.y);
            findRoad();
        }

        void move()
        {
            Vector3 nextPos = new Vector3(roads[roads.Count - 2][0], roads[roads.Count - 2][1]);
            rigidbody.velocity = Vector3.Normalize(nextPos - transform.position) * speed;
            if (Vector3.Distance(transform.position, nextPos) < 0.5f)
            {
                roads.RemoveAt(roads.Count - 1);
            }
            if (rigidbody.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (rigidbody.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}