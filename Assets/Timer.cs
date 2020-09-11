using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.BoardGameDungeon
{
    public class Timer : MonoBehaviour
    {
        public static float timer = 180;
        void Start()
        {

        }

        void Update()
        {
            timer -= Time.deltaTime;
            if (timer >= 177 || (timer <= 140 && timer >=137) || (timer <= 100 && timer >= 97) || (timer <= 60 && timer >= 57) || timer <= 20)
            {
                foreach(Transform text in transform)
                {
                    text.GetComponent<Text>().text = "" + (int)timer;
                    text.gameObject.SetActive(true);
                }
            }
            else
            {
                foreach (Transform text in transform)
                {
                    text.gameObject.SetActive(false);
                }
            }
        }
    }
}