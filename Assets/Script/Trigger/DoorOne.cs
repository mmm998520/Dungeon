using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class DoorOne : MonoBehaviour
    {
        float localX, localXMax, speed = 1.5f;
        [SerializeField] Transform doorSpriteTop, doorSpriteFront, doorCollider;
        public SpriteRenderer spriteRenderer;
        public Sprite non, used;
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
            doorSpriteTop.localPosition = new Vector3(localX, 0, 0);
            doorSpriteFront.localPosition = new Vector3(localX, 0, 0);
            doorCollider.localScale = new Vector3(localX, 1, 1);
            for(int i = 0; i < SFXManagers.Length; i++)
            {
                SFXManagers[i].DoorOneUse = (Mathf.Abs(tempLocalX - localX) > 0.001f);
            }
        }
    }
}