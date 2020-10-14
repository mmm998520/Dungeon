using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ExitToucher : MonoBehaviour
    {
        public static int touchNum;
        public string playerName;
        public GameObject joy;
        Rigidbody2D rigidbody2D;
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (touchNum >= 2)
            {
                joy.AddComponent(typeof(Rigidbody2D));
                rigidbody2D = joy.GetComponent<Rigidbody2D>();
                rigidbody2D.gravityScale = 0;
            }
            else
            {
                Destroy(rigidbody2D);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.name == playerName)
            {
                touchNum++;
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.name == playerName)
            {
                touchNum--;
            }
        }
    }
}
