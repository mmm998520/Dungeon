using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerSpriteCon : MonoBehaviour
    {
        public PlayerManager playerManager;
        public Sprite stop;
        SpriteRenderer spriteRenderer;
        Animator animator;
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (playerManager.v.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            if (playerManager.v.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            if (playerManager.v.magnitude < 0.5f)
            {
                animator.enabled = false;
                spriteRenderer.sprite = stop;
            }
            else
            {
                animator.enabled = true;
            }
        }
    }
}