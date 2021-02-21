using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        public GameObject[] doors;
        // Start is called before the first frame update
        void Start()
        {
            textColor = texts[0].color;
        }

        // Update is called once per frame
        void Update()
        {
            Color color;
            for(int i = 0; i < texts.Length; i++)
            {
                color = new Color(textColor.r, textColor.g, textColor.b, 1 - Mathf.Abs(CameraManager.center.x - ShowTextCenters[i]) * 0.2f);
                texts[i].color = color;
                for(int j=0; j < texts[i].transform.childCount; j++)
                {
                    texts[i].transform.GetChild(j).GetComponent<Image>().color = color;
                }
            }
            for(int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, (ShowHPLine - CameraManager.center.x) / ShowHPLine);
            }
            if (GameManager.monsters.childCount <= 0)
            {
                for(int i = 0; i < doors.Length; i++)
                {
                    Destroy(doors[i]);
                }
            }
            if(CameraManager.center.x > 87.7f)
            {
                SceneManager.LoadScene("Home");
            }
        }
    }
}