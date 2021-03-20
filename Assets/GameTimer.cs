using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class GameTimer : MonoBehaviour
    {
        public static float Timer;
        [SerializeField] Image[] timerImage;
        [SerializeField] Sprite[] number;
        [SerializeField] Sprite win;

        void Start()
        {
            int timer, H, M, S;
            timer = (int)Timer;
            H = timer / 60 / 60;
            M = (timer / 60) % 60;
            S = timer % 60 % 60;
            Debug.Log(H + " : " + M + " : " + S);
            if (H <= 99)
            {
                timerImage[0].sprite = number[H / 10];
                timerImage[1].sprite = number[H % 10];
                timerImage[2].sprite = number[M / 10];
                timerImage[3].sprite = number[M % 10];
                timerImage[4].sprite = number[S / 10];
                timerImage[5].sprite = number[S % 10];
            }
            else
            {
                timerImage[0].sprite = number[9];
                timerImage[1].sprite = number[9];
                timerImage[2].sprite = number[5];
                timerImage[3].sprite = number[9];
                timerImage[4].sprite = number[5];
                timerImage[5].sprite = number[9];
            }
            Timer = 0;
        }
    }
}
