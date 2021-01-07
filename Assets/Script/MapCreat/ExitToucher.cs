using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ExitToucher : MonoBehaviour
    {
        public static int touchNum;
        public string playerName;
        public new Rigidbody2D rigidbody2D;
        public SpriteRenderer spriteRenderer;
        public Sprite Null, Red, Blue, Red_Blue;
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

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.name == playerName)
            {
                if (playerName == "Red")
                {
                    if (spriteRenderer.sprite == Null)
                    {
                        spriteRenderer.sprite = Red;
                    }
                    else if (spriteRenderer.sprite == Blue)
                    {
                        spriteRenderer.sprite = Red_Blue;
                    }
                }
                else
                {
                    if (spriteRenderer.sprite == Null)
                    {
                        spriteRenderer.sprite = Blue;
                    }
                    else if (spriteRenderer.sprite == Red)
                    {
                        spriteRenderer.sprite = Red_Blue;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.name == playerName)
            {
                if (playerName == "Red")
                {
                    if (spriteRenderer.sprite == Red_Blue)
                    {
                        spriteRenderer.sprite = Blue;
                    }
                    else if (spriteRenderer.sprite == Red)
                    {
                        spriteRenderer.sprite = Null;
                    }
                }
                else
                {
                    if (spriteRenderer.sprite == Red_Blue)
                    {
                        spriteRenderer.sprite = Red;
                    }
                    else if (spriteRenderer.sprite == Blue)
                    {
                        spriteRenderer.sprite = Null;
                    }
                }
            }
        }
    }
}