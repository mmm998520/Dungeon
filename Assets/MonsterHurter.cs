using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterHurter : MonoBehaviour
    {
        [SerializeField] SpriteRenderer mySpriteRenderer, targetSpriteRenderer;
        [HideInInspector] public bool hurt = false;
        void Start()
        {
            if (!mySpriteRenderer)
            {
                targetSpriteRenderer = GetComponent<SpriteRenderer>();
            }
            if (!targetSpriteRenderer)
            {
                try
                {
                    targetSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
                }
                catch
                {
                    Debug.LogError("沒有target", gameObject);
                }
            }
        }

        void Update()
        {
            if (hurt)
            {
                try
                {
                    mySpriteRenderer.sprite = Resources.Load<Sprite>("Pictures/MonsterHurt/" + targetSpriteRenderer.sprite.name);
                }
                catch
                {
                    Debug.LogError("沒有這張圖");
                }
            }
        }
    }
}