using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerStartPos : MonoBehaviour
    {
        static int playerNum = 1;
        public Transform player;
        public void Start()
        {
            if(playerNum++ == 1)
            {
                player = GameObject.Find("Red").transform;
            }
            else
            {
                player = GameObject.Find("Blue").transform;
            }
            player.position = transform.position;
            if (playerNum > 2)
            {
                playerNum = 1;
            }
            Camera.main.transform.position = transform.position + Vector3.back * 10;
        }
    }
}
