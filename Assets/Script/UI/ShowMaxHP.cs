using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ShowMaxHP : MonoBehaviour
    {
        public SpriteRenderer MaxHPSpriteRenderer, HeadSpriteRenderer;
        public Sprite[] walks;
        int speed = 5;
        void Start()
        {
            MaxHPSpriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            if(PlayerManager.MaxHP - PlayerManager.HP < 1)
            {
                MaxHPSpriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(MaxHPSpriteRenderer.color.a, 0.5f, Time.deltaTime * speed));
            }
            else
            {
                MaxHPSpriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(MaxHPSpriteRenderer.color.a, 0, Time.deltaTime * speed));
            }
            if (HeadSpriteRenderer.sprite.name.Contains("walk0"))
            {
                MaxHPSpriteRenderer.sprite = walks[0];
            }
            else if (HeadSpriteRenderer.sprite.name.Contains("walk1"))
            {
                MaxHPSpriteRenderer.sprite = walks[1];
            }
            else if (HeadSpriteRenderer.sprite.name.Contains("walk2"))
            {
                MaxHPSpriteRenderer.sprite = walks[2];
            }
            MaxHPSpriteRenderer.flipX = HeadSpriteRenderer.flipX;
            MaxHPSpriteRenderer.enabled = HeadSpriteRenderer.enabled;
        }
    }
}