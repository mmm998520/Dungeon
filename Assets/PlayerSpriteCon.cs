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
        public GameObject DashPicture;

        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            #region//正常走路時轉向
            if (playerManager.v.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            if (playerManager.v.x < 0)
            {
                spriteRenderer.flipX = false;
            }
            #endregion

            #region//根據行為換圖
            if (playerManager.DashTimer < 0.3)
            {
                animator.enabled = false;
                spriteRenderer.enabled = false;
                DashPicture.SetActive(true);
                DashPicture.transform.eulerAngles = new Vector3(0, 0, Vector3.SignedAngle(Vector3.right, playerManager.DashA, Vector3.forward));
            }
            else if (playerManager.v.magnitude < 0.5f)
            {
                animator.enabled = false;
                spriteRenderer.enabled = true;
                spriteRenderer.sprite = stop;
                DashPicture.SetActive(false);
            }
            else
            {
                animator.enabled = true;
                spriteRenderer.enabled = true;
                DashPicture.SetActive(false);
            }
            #endregion
        }
    }
}