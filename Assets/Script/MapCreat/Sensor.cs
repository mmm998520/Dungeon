using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Sensor : MonoBehaviour
    {
        public MazeCreater mazeCreater;
        public int row, col;
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(collider.gameObject.layer == 8)
            {
                mazeCreater.creat(row, col);
                foreach(Transform child in GameManager.monsters)
                {
                    child.GetComponent<Navigate>().arriveNewRoom(row, col);
                }
                Destroy(gameObject);
            }
        }
    }
}