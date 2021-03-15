using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FireRain : MonoBehaviour
    {
        public float timer, stopTimer, destoryTimer;
        public bool CanHit = false;
        public SpriteRenderer spriteRenderer;
        void Update()
        {
            timer += Time.deltaTime;
            if (timer >= stopTimer + destoryTimer)
            {
                Destroy(gameObject);
            }
            else if(timer >= stopTimer)
            {
                CanHit = true;
                spriteRenderer.color = Color.white;
            }
        }

        void OnDestroy()
        {
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    FireRainInser.insPoses.Remove(((int)transform.position.x + i) * MazeCreater.totalCol + ((int)transform.position.y + j));
                }
            }
        }
    }
}