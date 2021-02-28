using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class SpriteSynchronizer : MonoBehaviour
    {
        [SerializeField] SpriteRenderer targetRenderer;
        SpriteRenderer myRenderner;
        [SerializeField]bool alphaChanger;
        float targetRendererA, myRendernerA;
        void Start()
        {
            myRenderner = GetComponent<SpriteRenderer>();
            targetRendererA = targetRenderer.color.a;
            myRendernerA = myRenderner.color.a;
            if (alphaChanger)
            {
                myRenderner.color = new Color(myRenderner.color.r, myRenderner.color.g, myRenderner.color.b, 0);
            }
        }
        void Update()
        {
            myRenderner.sprite = targetRenderer.sprite;
            if (alphaChanger)
            {
                myRenderner.color = new Color(myRenderner.color.r, myRenderner.color.g, myRenderner.color.b, myRendernerA * targetRenderer.color.a / targetRendererA);
            }
        }
    }
}
