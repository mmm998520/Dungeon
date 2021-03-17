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
            if (keyboard.escapeKey.wasPressedThisFrame || keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)
            {
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
            
        }
    }
}