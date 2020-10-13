using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Spider : Navigate
    {
        public int[] nextPos;
        public float rotateSpeed = 200;
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
            if ((timer+=Time.deltaTime)>9)
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

        protected void changeDirection()
        {
            Vector3 myPos = transform.position;
            Vector3 endPos = new Vector3(nextPos[0], nextPos[1], 0);
            // 讓z軸沒有前後誤差值，以免面向錯方向
            endPos.z = myPos.z;
            //計算將要朝向的方向，定義其為物件的x軸方向
            Vector3 vectorToTarget = endPos - myPos;
            //將物件的x軸延z軸旋轉90度尋找y軸
            Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * vectorToTarget;
            // LookRotation需要給予向前軸與向上軸，物件z軸將指向向前軸，y軸指向向上軸
            //所以只要讓物件z軸面向世界座標z軸，y軸指向面向方向轉90度即可讓物件指向x軸
            Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);
            // 讓物件朝指定方向轉指定角度(rotateSpeed * Time.deltaTime讓他變定速旋轉)
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }
    }
}