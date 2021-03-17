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
            var gamepad = Gamepad.current;//手柄
            var keyboard = Keyboard.current;//手柄
            if (gamepad.aButton.isPressed)
            {
                Debug.Log("A");
            }
            if (gamepad.bButton.isPressed)
            {
                Debug.Log("B");
            }
            if (gamepad.xButton.isPressed)
            {
                Debug.Log("X");
            }
            if (gamepad.yButton.isPressed)
            {
                Debug.Log("Y");
            }
            if (gamepad.leftShoulder.isPressed)
            {
                Debug.Log("leftShoulder");
            }
            if (gamepad.leftTrigger.isPressed)
            {
                Debug.Log("leftTrigger");
            }
            
            if (keyboard.escapeKey.isPressed || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
            
        }
    }
}