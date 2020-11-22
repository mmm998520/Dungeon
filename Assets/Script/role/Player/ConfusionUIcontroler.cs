using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ConfusionUIcontroler : MonoBehaviour
    {
        public Sprite[] ConfusionUISprites;
        int ConfusionUISpritesNum;
        float timer, timerStoper = 0.1f;
        SpriteRenderer spriteRenderer;
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if ((timer += Time.deltaTime) >= timerStoper)
            {
                timer = 0;
                spriteRenderer.sprite = ConfusionUISprites[++ConfusionUISpritesNum];
                if(ConfusionUISpritesNum>= ConfusionUISprites.Length-1)
                {
                    ConfusionUISpritesNum = -1;
                }
            }
        }
    }
}