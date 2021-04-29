using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ButtonShower : MonoBehaviour
    {
        float timer;
        public List<Sprite> buttons = new List<Sprite>();
        int buttonNum = 0;
        Image image;

        public enum Buttons
        {
            gamepadMove,
            gamepadA,
            gamepadB,
            gamepadLB,
            gamepadLT,
            gamepadRB,
            gamepadRT,
            keyboardMove,
            keyboardP1A,
            keyboardP1B,
            keyboardP2A,
            keyboardP2B,
        }
        public static Dictionary<Buttons, Sprite> buttonSprites = new Dictionary<Buttons, Sprite>();


        void Start()
        {
            image = GetComponent<Image>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer = 0;
                if (++buttonNum >= buttons.Count)
                {
                    buttonNum = 0;
                }
                image.sprite = buttons[buttonNum];
            }
        }

        public void addButtons(Sprite[] sprites)
        {
            for(int i = 0; i < sprites.Length; i++)
            {
                if (!buttons.Contains(sprites[i]))
                {
                    buttons.Add(sprites[i]);
                }
            }
        }

        public void reset()
        {
            timer = 0;
            buttonNum = 0;
            image.sprite = buttons[buttonNum];
        }
    }
}