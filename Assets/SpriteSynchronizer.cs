using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class SpriteSynchronizer : MonoBehaviour
    {
        [SerializeField] SpriteRenderer targetRenderer;
        SpriteRenderer myRenderner;
        void Start()
        {
            myRenderner = GetComponent<SpriteRenderer>();
        }
        void Update()
        {
            myRenderner.sprite = targetRenderer.sprite;
        }
    }
}
