using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class DoorTwo : MonoBehaviour
    {
        float localX, localXMax, speed = 1.5f;
        Transform doorSprite, doorCollider;
        public SpriteRenderer spriteRenderer;
        public Sprite non, blue, red, p1_p2;

        void Start()
        {
            doorSprite = transform.GetChild(0);
            localX = localXMax = doorSprite.localPosition.x;
            doorCollider = transform.GetChild(1);
            doorCollider.localScale = new Vector3(localX, 1, 1);
        }

        void Update()
        {
            if (ButtonTwo.redUsed && ButtonTwo.blueUsed)
            {
                localX -= Time.deltaTime * speed;
                spriteRenderer.sprite = p1_p2;
            }
            else
            {
                localX += Time.deltaTime * speed;
                if(ButtonTwo.redUsed)
                {
                    spriteRenderer.sprite = red;
                }
                else if(ButtonTwo.blueUsed)
                {
                    spriteRenderer.sprite = blue;
                }
                else
                {
                    spriteRenderer.sprite = non;
                }
            }
            localX = Mathf.Clamp(localX, 0, localXMax);
            doorSprite.localPosition = new Vector3(localX, 0, 0);
            doorCollider.localScale = new Vector3(localX, 1, 1);
        }
    }
}