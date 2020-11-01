using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ButtonTwo : MonoBehaviour
    {
        public static int useButtonNum = 0;
        int usethisNum = 0;
        void Start()
        {

        }

        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (usethisNum++ == 0)
                {
                    useButtonNum++;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (--usethisNum == 0)
                {
                    useButtonNum--;
                }
            }
        }
    }
}