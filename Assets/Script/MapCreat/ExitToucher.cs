using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ExitToucher : MonoBehaviour
    {
        public static int touchNum;
        public string playerName;
        public Rigidbody2D rigidbody2D;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (touchNum >= 2)
            {
                rigidbody2D.mass = 0.2f;
            }
            else
            {
                rigidbody2D.mass = 1000000;
                rigidbody2D.velocity = Vector3.zero;
                rigidbody2D.angularVelocity = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.name == playerName)
            {
                touchNum++;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.name == playerName)
            {
                touchNum--;
            }
        }
    }
}
