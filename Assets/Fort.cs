using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Fort : MonoBehaviour
    {
        static int playerTargetNum = 0;
        void Start()
        {
        }

        void Update()
        {
            lookAtPlayer();
        }

        void lookAtPlayer()
        {
            if (playerTargetNum++ > 2)
            {
                playerTargetNum = 0;
            }
            transform.LookAt(GameManager.players.GetChild(0));
            transform.Rotate(new Vector3(0, -90, 0));
        }
    }
}