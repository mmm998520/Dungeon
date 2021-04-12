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
            if ((Keyboard.current != null && Keyboard.current.f12Key.wasPressedThisFrame) || (Gamepad.current != null && Gamepad.current.selectButton.isPressed && Gamepad.current.startButton.isPressed))
            {
                GameManager.passLayerOneTimes = 0;
                GameManager.passLayerTwoTimes = 0;
                GameManager.layerOneCntinuousDideTimes = 0;
                GameManager.layerTwoCntinuousDideTimes = 0;
                PlayerPrefs.SetInt("passLayerTwoTimes", GameManager.passLayerOneTimes);
                PlayerPrefs.SetInt("passLayerTwoTimes", GameManager.passLayerTwoTimes);
                PlayerPrefs.SetInt("layerOneCntinuousDideTimes", GameManager.layerOneCntinuousDideTimes);
                PlayerPrefs.SetInt("layerTwoCntinuousDideTimes", GameManager.layerTwoCntinuousDideTimes);
                PlayerPrefs.Save();
                SwitchScenePanel.NextScene = "GameWarning";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }
    }
}
