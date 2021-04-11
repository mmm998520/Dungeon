using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.DungeonPad
{
    public class GoBackToHome : MonoBehaviour
    {
        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (InputManager.anyExit())
            {
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
            
        }
    }
}