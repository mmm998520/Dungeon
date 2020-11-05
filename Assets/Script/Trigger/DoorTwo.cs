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
        public Sprite non, p1, p2, p1_p2;

        void Start()
        {
            doorSprite = transform.GetChild(0);
            localX = localXMax = doorSprite.localPosition.x;
            doorCollider = transform.GetChild(1);
            doorCollider.localScale = new Vector3(localX, 1, 1);
        }

        void Update()
        {
            if (ButtonTwo.p1used && ButtonTwo.p2used)
            {
                localX -= Time.deltaTime * speed;
                spriteRenderer.sprite = p1_p2;
            }
            else
            {
                localX += Time.deltaTime * speed;
                if(ButtonTwo.p1used)
                {
                    spriteRenderer.sprite = p1;
                }
                else if(ButtonTwo.p2used)
                {
                    spriteRenderer.sprite = p2;
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