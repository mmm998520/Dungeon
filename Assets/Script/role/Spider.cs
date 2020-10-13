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
        }

        void Update()
        {
            if ((timer+=Time.deltaTime)>3)
            {
                timer = 0;
                reTarget();
            }
            if (nextPos.Length > 1)
            {
                if (Vector3.Distance(new Vector3(nextPos[0], nextPos[1], 0), transform.position) < 0.5f)
                {
                    reTarget();
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                arriveNewRoom(startRoomRow, startRoomRow);
                reTarget();
            }
            nextPos = findRoad();
            changeDirection();
            GetComponent<Rigidbody2D>().velocity = transform.right*1f;
        }

        void reTarget()
        {
            int r = Random.Range(0, canGo.Count);
            endRow = new int[] { canGo[r] / MazeCreater.totalCol };
            endCol = new int[] { canGo[r] % MazeCreater.totalCol };
        }
    }
}