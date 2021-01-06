using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowLockHP : MonoBehaviour
    {
        public Image image;
        public Color origialColor;
        //public static float hurtTimer;
        void Start()
        {

        }

        void Update()
        {
            float lockHPLight = 0.9f - PlayerManager.lockedHPTimer * 0.3f;
            //float hurtLight = 0.25f - hurtTimer;
            //hurtTimer += Time.deltaTime;
            if (lockHPLight > 0)
            {
                image.color = new Color(origialColor.r, origialColor.g, origialColor.b, lockHPLight);
            }
            /*
            else if (hurtLight > 0)
            {
                image.color = new Color(0.5f, 0.3f, 0.3f, hurtLight);
            }
            */
            else
            {
                image.color = Color.clear;
            }
        }
    }
}