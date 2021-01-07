using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class TutorialPPT : MonoBehaviour
    {
        int currentPPTNum = 0;
        public Image image;
        public Sprite[] PPTs;
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                if (++currentPPTNum >= PPTs.Length)
                {
                    SceneManager.LoadScene("SelectRole_Game 1");
                }
                else
                {
                    image.sprite = PPTs[currentPPTNum];
                }
            }
            else if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                if (--currentPPTNum < 0)
                {
                    SceneManager.LoadScene("Home");
                }
                else
                {
                    image.sprite = PPTs[currentPPTNum];
                }
            }
        }
    }
}
