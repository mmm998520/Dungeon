using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Fort : MonoBehaviour
    {
        static int playerTargetNum = 0;
        public GameObject cannonball;

        void Start()
        {

        }

        void Update()
        {

        }

        public void lookAtPlayer()
        {
            if (playerTargetNum++ > 2)
            {
                playerTargetNum = 0;
            }
            float angle = Vector3.SignedAngle(Vector3.right, GameManager.players.GetChild(0).position - transform.position, Vector3.forward);
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angle);
        }

        public void shoot()
        {
            Instantiate(cannonball, transform.position, transform.GetChild(0).rotation);
        }
    }
}