using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class HPPrompt : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        [SerializeField]Sprite Up, Down;
        void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            float dis = Vector3.Distance(GameManager.players.GetChild(0).position, GameManager.players.GetChild(1).position);
            if (dis < 2f)
            {
                spriteRenderer.sprite = Up;
            }
            else if (dis < 4.5f)
            {
                spriteRenderer.sprite = null;
            }
            else
            {
                spriteRenderer.sprite = Down;
            }
        }
    }
}
