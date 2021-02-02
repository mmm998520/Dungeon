using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class Tutorial : MonoBehaviour
    {
        Color textColor;
        public Text[] texts;
        public int[] ShowTextCenters;
        public int ShowHPLine;
        public Image[] images;
        // Start is called before the first frame update
        void Start()
        {
            textColor = texts[0].color;
        }

        // Update is called once per frame
        void Update()
        {
            for(int i = 0; i < texts.Length; i++)
            {
                texts[i].color = new Color(textColor.r, textColor.g, textColor.b, 1 - Mathf.Abs(CameraManager.center.x - ShowTextCenters[i]) * 0.2f);
            }
            for(int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, (ShowHPLine - CameraManager.center.x) / ShowHPLine);
            }
        }
    }
}