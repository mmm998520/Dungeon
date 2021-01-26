using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FireRain : MonoBehaviour
    {
        public float timer, stopeTimer, destoryTimer;
        public bool CanHit = false;
        public SpriteRenderer spriteRenderer;
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= stopeTimer + destoryTimer)
            {
                Destroy(gameObject);
            }
            else if(timer >= stopeTimer)
            {
                CanHit = true;
                spriteRenderer.color = Color.white;
            }
        }
    }
}