using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerStartPos : MonoBehaviour
    {
        static int playerNum = 1;
        public Transform player;
        void Start()
        {
            player = GameObject.Find("p"+playerNum++).transform;
            player.position = transform.position;
            if (playerNum > 2)
            {
                playerNum = 1;
            }
            Camera.main.transform.position = transform.position + Vector3.back * 10;
        }
    }
}
