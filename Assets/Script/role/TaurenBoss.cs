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
            speed = 1;
            rigidbody = GetComponent<Rigidbody2D>();
            addCanGoByHand();
            endRow = new int[1];
            endCol = new int[1];
        }
        Vector3 RecordDir;

        private void Update()
        {
            minDisPlayer = MinDisPlayer();
            if (Vector3.Distance(minDisPlayer.position, transform.position) > 3)
            {
                resetRoad();
                move();
            }
            else
            {

            }
        }

        void prePunch()
        {
            rigidbody.velocity = Vector3.zero;
        }

        void punch()
        {
            rigidbody.velocity = RecordDir * 3;
        }

        void resetRoad()
        {
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
            RecordDir = rigidbody.velocity;
        }
    }
}