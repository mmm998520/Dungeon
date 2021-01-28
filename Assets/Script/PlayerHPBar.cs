using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class PlayerHPBar : MonoBehaviour
    {
        public RectTransform mask, color;
        public Image HPBar_Color;
        public Sprite HPBar_Red, HPBar_Yellow, HPBar_Green;
        float hpRate, hpRateRecorder;
        void Update()
        {
            hpRate = PlayerManager.HP / PlayerManager.MaxHP;
            mask.localScale = new Vector3(hpRate, 1, 1);
            color.localScale = new Vector3(1/hpRate, 1, 1);
            if (hpRate > 0.9999f)
            {
                HPBar_Color.sprite = HPBar_Green;
            }
            else if(hpRate < hpRateRecorder - 0.001f)
            {
                HPBar_Color.sprite = HPBar_Red;
            }
            else if (hpRate > hpRateRecorder + 0.001f)
            {
                HPBar_Color.sprite = HPBar_Green;
            }
            else
            {
                HPBar_Color.sprite = HPBar_Yellow;
            }
            hpRateRecorder = hpRate;
        }
    }
}