using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class loadscene : MonoBehaviour
    {
        void Start()
        {
            ReGamer.ReAbility();
        }


        public void selestgame()
        {
            SwitchScenePanel.NextScene = "SelectRole_Game 1";
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
        }

        public void selecttutorial()
        {
            SwitchScenePanel.NextScene = "SelectRole_Game 0";
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
        }

        public void selectSetting()
        {
            SwitchScenePanel.NextScene = "Setting";
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard.escapeKey.wasPressedThisFrame || keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)
            {
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }

    }

}
