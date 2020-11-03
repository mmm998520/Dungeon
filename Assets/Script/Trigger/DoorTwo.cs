using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class DoorTwo : MonoBehaviour
    {
        float localX, localXMax, speed = 1.5f;
        Transform doorSprite, doorCollider;
        SpriteRenderer spriteRenderer;
        public Sprite non, one, two;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            doorSprite = transform.GetChild(0);
            localX = localXMax = doorSprite.localPosition.x;
            doorCollider = transform.GetChild(1);
            doorCollider.localScale = new Vector3(localX, 1, 1);
        }

        void Update()
        {
            if (ButtonTwo.useButtonNum >= 2)
            {
                localX -= Time.deltaTime * speed;
                spriteRenderer.sprite = two;
            }
            else
            {
                localX += Time.deltaTime * speed;
                if(ButtonTwo.useButtonNum >= 1)
                {
                    spriteRenderer.sprite = one;
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