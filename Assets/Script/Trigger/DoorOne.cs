using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class DoorOne : MonoBehaviour
    {
        float localX, localXMax, speed = 1.5f;
        Transform doorSprite, doorCollider;
        SpriteRenderer spriteRenderer;
        public Sprite non, used;

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
            if (ButtonOne.useButtonNum >= 1)
            {
                localX -= Time.deltaTime * speed;
                spriteRenderer.sprite = used;
            }
            else
            {
                localX += Time.deltaTime * speed;
                spriteRenderer.sprite = non;
            }
            localX = Mathf.Clamp(localX, 0, localXMax);
            doorSprite.localPosition = new Vector3(localX, 0, 0);
            doorCollider.localScale = new Vector3(localX, 1, 1);
        }
    }
}