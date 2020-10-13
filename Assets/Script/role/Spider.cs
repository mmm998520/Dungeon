using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spider : MonsterManager
    {
        float timer;
        void Start()
        {
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
            reTarget();
            hp = transform.GetChild(1);
        }

        void Update()
        {
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);

            randomMove();

            attackCD();
            if (prepare != 0)
            {
                Transform t = MinDisPlayer();
                nextPos = new int[] { Mathf.RoundToInt(t.position.x), Mathf.RoundToInt(t.position.y) };
                prepareAttack();
            }
            nextPos = findRoad();
            moveToTarget();
            changeDirection();
            GetComponent<Rigidbody2D>().velocity = transform.right*1f;
            if (HP <= 0)
            {
                Destroy(gameObject);
            }
        }

        void reTarget()
        {
            int r = Random.Range(0, canGo.Count);
            endRow = new int[] { canGo[r] / MazeCreater.totalCol };
            endCol = new int[] { canGo[r] % MazeCreater.totalCol };
        }

        void randomMove()
        {
            if ((timer += Time.deltaTime) > 3)
            {
                timer = 0;
                reTarget();
            }
            if (nextPos.Length > 1)
            {
                if (Vector3.Distance(new Vector3(nextPos[0], nextPos[1], 0), transform.position) < 0.5f)
                {
                    timer = 0;
                    reTarget();
                }
            }
        }
    }
}