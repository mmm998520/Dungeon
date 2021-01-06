using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class ReGamer : MonoBehaviour
    {
        float timer;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                Re();
            }
        }

        void Re()
        {
            PlayerManager.money /= 2;
            AbilityManager.myAbilitys.Clear();
            SceneManager.LoadScene("Game 1");
        }
    }
}

