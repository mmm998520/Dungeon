using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowHomeButtonCD : MonoBehaviour
    {
        RectTransform rectTransform;
        float totalHeight;

        void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            totalHeight = rectTransform.sizeDelta.y;
        }

        private void OnGUI()
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, PlayerManager.homeButtonTimer * totalHeight / (-PlayerManager.homeButtonTimerStoper * 2));
        }
    }
}