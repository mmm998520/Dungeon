using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Slime : MonsterManager
    {
        float timer;
        void Start()
        {
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
            reTarget();
            ArmorBar = transform.GetChild(1);
        }

        void Update()
        {
            ArmorBar.localScale = new Vector3(Armor / MaxArmor, ArmorBar.localScale.y, ArmorBar.localScale.z);

            randomMove();

            StartCoroutine("findRoad");
            if (nextPos == null)
            {
                reTarget();
                StartCoroutine("findRoad");
            }
            moveToTarget();
            changeDirection();
            GetComponent<Rigidbody2D>().velocity = transform.right * 1f;
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
            if (nextPos != null)
            {
                if (nextPos.Length > 1)
                {
                    if (Vector3.Distance(new Vector3(nextPos[0], nextPos[1], 0), transform.position) < 0.5f)
                    {
                        timer = 0;
                        reTarget();
                    }
                }
                reTarget();
            }
            else
            {
                reTarget();
            }
        }
    }
}