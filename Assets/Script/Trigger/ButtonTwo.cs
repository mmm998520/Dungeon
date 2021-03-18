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
            red,
            blue,
            red_blue,
            blue_red
        }
        ButtonStat buttonStat = ButtonStat.non;

        public static bool redUsed = false, blueUsed = false;
        public SpriteRenderer spriteRenderer;
        public Sprite non, blue, red;

        void Start()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if (buttonStat == ButtonStat.non)
                {
                    if (collider.gameObject.name == "Red")
                    {
                        buttonStat = ButtonStat.red;
                        spriteRenderer.sprite = red;
                        redUsed = true;
                    }
                    else
                    {
                        buttonStat = ButtonStat.blue;
                        spriteRenderer.sprite = blue;
                        blueUsed = true;
                    }
                }
                else if(buttonStat == ButtonStat.red && collider.gameObject.name == "Blue")
                {
                    buttonStat = ButtonStat.red_blue;
                }
                else if(buttonStat == ButtonStat.blue && collider.gameObject.name == "Red")
                {
                    buttonStat = ButtonStat.blue_red;
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
                if (buttonStat == ButtonStat.red)
                {
                    buttonStat = ButtonStat.non;
                    spriteRenderer.sprite = non;
                    redUsed = false;
                }
                else if (buttonStat == ButtonStat.blue)
                {
                    buttonStat = ButtonStat.non;
                    spriteRenderer.sprite = non;
                    blueUsed = false;
                }
                else if(buttonStat == ButtonStat.red_blue || buttonStat == ButtonStat.blue_red)
                {
                    if (collider.gameObject.name == "Red")
                    {
                        buttonStat = ButtonStat.blue;
                        spriteRenderer.sprite = blue;
                        redUsed = false;
                        blueUsed = true;
                    }
                    else
                    {
                        buttonStat = ButtonStat.red;
                        spriteRenderer.sprite = red;
                        blueUsed = false;
                        redUsed = true;
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