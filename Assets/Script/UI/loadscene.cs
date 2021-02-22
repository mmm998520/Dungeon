using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class loadscene : MonoBehaviour
    {
        void Start()
        {

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
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }

    }

}
