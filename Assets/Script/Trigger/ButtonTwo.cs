using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ButtonTwo : MonoBehaviour
    {
        enum ButtonStat
        {
            non,
            p1,
            p2,
            p1_p2,
            p2_p1
        }
        ButtonStat buttonStat = ButtonStat.non;

        public static bool p1used = false, p2used = false;
        public SpriteRenderer spriteRenderer;
        public Sprite non, p1, p2;

        void Start()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (buttonStat == ButtonStat.non)
                {
                    if (collider.GetComponent<PlayerManager>().p1)
                    {
                        buttonStat = ButtonStat.p1;
                        spriteRenderer.sprite = p1;
                        p1used = true;
                    }
                    else
                    {
                        buttonStat = ButtonStat.p2;
                        spriteRenderer.sprite = p2;
                        p2used = true;
                    }
                }
                else if(buttonStat == ButtonStat.p1)
                {
                    buttonStat = ButtonStat.p1_p2;
                }
                else if(buttonStat == ButtonStat.p2)
                {
                    buttonStat = ButtonStat.p2_p1;
                }
                else
                {
                    Debug.LogError("ButtonTwo出錯" + buttonStat);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (buttonStat == ButtonStat.p1)
                {
                    buttonStat = ButtonStat.non;
                    spriteRenderer.sprite = non;
                    p1used = false;
                }
                else if (buttonStat == ButtonStat.p2)
                {
                    buttonStat = ButtonStat.non;
                    spriteRenderer.sprite = non;
                    p2used = false;
                }
                else if(buttonStat == ButtonStat.p1_p2 || buttonStat == ButtonStat.p2_p1)
                {
                    if (collider.GetComponent<PlayerManager>().p1)
                    {
                        buttonStat = ButtonStat.p2;
                        spriteRenderer.sprite = p2;
                        p1used = false;
                        p2used = true;
                    }
                    else
                    {
                        buttonStat = ButtonStat.p1;
                        spriteRenderer.sprite = p1;
                        p1used = true;
                        p2used = false;
                    }
                }
                else
                {
                    Debug.LogError("ButtonTwo出錯" + buttonStat);
                }
            }
        }
    }
}