using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.DungeonPad
{
    public class HomePage : MonoBehaviour
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

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
        }

        public void selectSetting()
        {
            SwitchScenePanel.NextScene = "Setting";
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
        }

        public void selectOperate()
        {
            SwitchScenePanel.NextScene = "Operate";
            GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            /*if (keyboard.escapeKey.wasPressedThisFrame || keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)
            {
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }*/

            if (keyboard.escapeKey.isPressed)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
