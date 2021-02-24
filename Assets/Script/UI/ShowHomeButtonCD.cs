using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowHomeButtonCD : MonoBehaviour
    {
        Image image;

        void Start()
        {
            image = GetComponent<Image>();
        }

        private void OnGUI()
        {
            image.fillAmount = PlayerManager.homeButtonTimer / (-PlayerManager.homeButtonTimerStoper * 2);
        }
    }
}