using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class DoorTwo : MonoBehaviour
    {
        float localX, localXMax, speed = 1.5f;
        [SerializeField] Transform doorSpriteTop, doorSpriteFront, doorCollider;
        public SpriteRenderer spriteRenderer;
        public Sprite non, blue, red, p1_p2;
        [SerializeField] Transform SFXManagersTransform;
        SFXManager[] SFXManagers;

        void Start()
        {
            localX = localXMax = doorSpriteTop.localPosition.x;
            doorCollider.localScale = new Vector3(localX, 1, 1);
            SFXManagers = SFXManagersTransform.GetComponents<SFXManager>();
        }

        void Update()
        {
            float tempLocalX = localX;
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
            doorSpriteTop.localPosition = new Vector3(localX, 0, 0);
            doorSpriteFront.localPosition = new Vector3(localX, 0, 0);
            doorCollider.localScale = new Vector3(localX, 1, 1);
            for (int i = 0; i < SFXManagers.Length; i++)
            {
                SFXManagers[i].DoorTwoUse = (Mathf.Abs(tempLocalX - localX) > 0.001f);
            }
        }
    }
}