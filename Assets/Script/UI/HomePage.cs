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
                GameManager.passLayerThreeTimes = 0;
                GameManager.layerOneCntinuousDideTimes = 0;
                GameManager.layerThreeCntinuousDideTimes = 0;
                GameManager.layerFourCntinuousWinTimes = 0;
                PlayerPrefs.SetInt("passLayerThreeTimes", GameManager.passLayerOneTimes);
                PlayerPrefs.SetInt("passLayerThreeTimes", GameManager.passLayerThreeTimes);
                PlayerPrefs.SetInt("layerOneCntinuousDideTimes", GameManager.layerOneCntinuousDideTimes);
                PlayerPrefs.SetInt("layerThreeCntinuousDideTimes", GameManager.layerThreeCntinuousDideTimes);
                PlayerPrefs.SetInt("layerFourCntinuousWinTimes", GameManager.layerFourCntinuousWinTimes);
                PlayerPrefs.DeleteKey("TaurenStat");
                PlayerPrefs.DeleteKey("DragonStat");
                PlayerPrefs.Save();
                SwitchScenePanel.NextScene = "GameWarning";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }
    }
}
