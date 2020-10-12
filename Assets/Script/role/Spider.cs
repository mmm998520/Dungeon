using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spider : Navigate
    {
        public int[] nextPos;

        void Start()
        {
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
            retarget();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                arriveNewRoom(startRoomRow, startRoomRow);
                retarget();
            }
            nextPos = findRoad();
            transform.LookAt(new Vector3(nextPos[0], nextPos[1], 0));
        }

        void retarget()
        {
            int r = Random.Range(0, canGo.Count);
            endRow = new int[] { canGo[r] / MazeCreater.totalCol };
            endCol = new int[] { canGo[r] % MazeCreater.totalCol };
        }
    }
}