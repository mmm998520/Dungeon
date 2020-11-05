using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ButtonOne : MonoBehaviour
    {
        public static int useButtonNum = 0;
        int usethisNum = 0;
        public SpriteRenderer spriteRenderer;
        public Sprite non, used;

        void Start()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (usethisNum++ == 0)
                {
                    useButtonNum++;
                    spriteRenderer.sprite = used;
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
                    spriteRenderer.sprite = non;
                }
            }
        }
    }
}