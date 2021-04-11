﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public enum PlayerMod
    {
        none,
        keyboardP1,
        keyboardP2,
        gamepadP1,
        gamepadP2,
        singleP1,
        singleP2
    }
    public static PlayerMod p1Mod = PlayerMod.none, p2Mod = PlayerMod.none;

    public static int p1KeyboardUpNum = 36,
                                 p1KeyboardDownNum = 32,
                                 p1KeyboardLeftNum = 14,
                                 p1KeyboardRightNum = 17,
                                 p1KeyboardDashNum = 23,
                                 p1KeyboardBreakfreeKeyNum = 24,
                                 p1KeyboardSkillKeyNum = 26,
                                 p1KeyboardLookskillKeyNum = 3,
                                 p2KeyboardUpNum = 62,
                                 p2KeyboardDownNum = 63,
                                 p2KeyboardLeftNum = 60,
                                 p2KeyboardRightNum = 61,
                                 p2KeyboardDashNum = 84,
                                 p2KeyboardBreakfreeKeyNum = 85,
                                 p2KeyboardSkillKeyNum = 83,
                                 p2KeyboardLookskillKeyNum = 80;

    public static Gamepad p1Gamepad, p2Gamepad;
    public static Gamepad currentGamepad;
    /// <summary>
    /// Horizontal水平；Vertical垂直
    /// </summary>
    public static float p1KeyboardHorizontalValue, p1KeyboardVerticalValue, p2KeyboardHorizontalValue, p2KeyboardVerticalValue, p1GamepadHorizontalValue, p1GamepadVerticalValue, p2GamepadHorizontalValue, p2GamepadVerticalValue;
    public static bool twoPlayerMode = true;

    void Start()
    {
        keyboardSetting();
    }

    public static void keyboardSetting()
    {
        if (!PlayerPrefs.HasKey("Keyboard"))
        {
            //PlayerPrefs.SetString("Keyboard", "36,32,14,17,23,24,26,3,62,63,60,61,84,85,83,80,");
            PlayerPrefs.SetString("Keyboard", "36,32,14,17,23,24,26,29,62,63,60,61,84,85,83,92,");
            PlayerPrefs.Save();
        }
        string[] keyboardNum = PlayerPrefs.GetString("Keyboard").Split(',');
        p1KeyboardUpNum = int.Parse(keyboardNum[0]);
        p1KeyboardDownNum = int.Parse(keyboardNum[1]);
        p1KeyboardLeftNum = int.Parse(keyboardNum[2]);
        p1KeyboardRightNum = int.Parse(keyboardNum[3]);
        p1KeyboardDashNum = int.Parse(keyboardNum[4]);
        p1KeyboardBreakfreeKeyNum = int.Parse(keyboardNum[5]);
        p1KeyboardSkillKeyNum = int.Parse(keyboardNum[6]);
        p1KeyboardLookskillKeyNum = int.Parse(keyboardNum[7]);
        p2KeyboardUpNum = int.Parse(keyboardNum[8]);
        p2KeyboardDownNum = int.Parse(keyboardNum[9]);
        p2KeyboardLeftNum = int.Parse(keyboardNum[10]);
        p2KeyboardRightNum = int.Parse(keyboardNum[11]);
        p2KeyboardDashNum = int.Parse(keyboardNum[12]);
        p2KeyboardBreakfreeKeyNum = int.Parse(keyboardNum[13]);
        p2KeyboardSkillKeyNum = int.Parse(keyboardNum[14]);
        p2KeyboardLookskillKeyNum = int.Parse(keyboardNum[15]);
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            for (int i = 0; i < Keyboard.KeyCount; i++)
            {
                if (keyboard.allKeys[i].isPressed)
                {
                    Debug.Log(i);
                }
            }
        }
    }

    #region//Axes
    public static float keyboardAxes(int lessKeyNum, int addKeyNum, float value)
    {
        return keyboardAxes(lessKeyNum, addKeyNum, value, 2.5f, 0.1f, 3f);
    }

    public static float keyboardAxes(int lessKeyNum, int addKeyNum, float value, float addSpeed, float critical, float gravity)
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            bool lessKeyPress = false, addKeyPress = false;

            if (keyboard.allKeys[lessKeyNum].isPressed)
            {
                lessKeyPress = true;
                value -= Time.deltaTime * addSpeed;
            }
            if (keyboard.allKeys[addKeyNum].isPressed)
            {
                addKeyPress = true;
                value += Time.deltaTime * addSpeed;
            }
            value = Mathf.Clamp(value, -1, 1);

            if ((addKeyPress && lessKeyPress) || (!addKeyPress && !lessKeyPress))
            {
                if (value > critical)
                {
                    value -= gravity * Time.deltaTime;
                }
                else if (value < -critical)
                {
                    value += gravity * Time.deltaTime;
                }
                else
                {
                    value = 0;
                }
            }
        }
        return value;
    }

    public static float gamepadAxes(float gamepadValue, float value)
    {
        return gamepadAxes(gamepadValue, value, 0.2f, 2.5f, 0.1f, 1.5f);
    }

    public static float gamepadAxes(float gamepadStickValue, float value, float deadZone, float addSpeed, float critical, float gravity)
    {
        float absStickValue = Mathf.Abs(gamepadStickValue);
        if (gamepadStickValue > deadZone)
        {
            if (gamepadStickValue > deadZone * 3 && value < 0)
            {
                value = 0;
            }
            value += Time.deltaTime * addSpeed * absStickValue;
        }
        else if (gamepadStickValue < -deadZone)
        {
            if (gamepadStickValue < -deadZone * 3 && value > 0)
            {
                value = 0;
            }
            value -= Time.deltaTime * addSpeed * absStickValue;
        }
        value = Mathf.Clamp(value, -1, 1);
        value = Mathf.Clamp(value, -absStickValue - 0.5f, absStickValue + 0.5f);

        if (gamepadStickValue < deadZone && gamepadStickValue > -deadZone)
        {
            if (value > critical)
            {
                value -= gravity * Time.deltaTime;
            }
            else if (value < -critical)
            {
                value += gravity * Time.deltaTime;
            }
            else
            {
                value = 0;
            }
        }
        return value;
    }
    #endregion

    #region// EnterKey / ExitKey
    public static bool keyboardEnter()
    {
        return Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame;
    }

    public static bool playerKeyboardEnter()
    {
        return Keyboard.current != null && (Keyboard.current.allKeys[p1KeyboardDashNum].wasPressedThisFrame || Keyboard.current.allKeys[p2KeyboardDashNum].wasPressedThisFrame);
    }

    public static bool twoPlayerGamepadEnter()
    {
        return Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame;
    }

    public static bool onePlayerGamepadEnter()
    {
        return Gamepad.current != null && (Gamepad.current.leftShoulder.wasPressedThisFrame || Gamepad.current.rightShoulder.wasPressedThisFrame);
    }

    public static bool anyEnter()
    {
        return keyboardEnter() || playerKeyboardEnter() || twoPlayerGamepadEnter() || onePlayerGamepadEnter();
    }

    public static bool keyboardExit()
    {
        return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
    }

    public static bool playerKeyboardExit()
    {
        return Keyboard.current != null && (Keyboard.current.allKeys[p1KeyboardBreakfreeKeyNum].wasPressedThisFrame || Keyboard.current.allKeys[p2KeyboardBreakfreeKeyNum].wasPressedThisFrame);
    }

    public static bool twoPlayerGamepadExit()
    {
        return Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame;
    }

    public static bool onePlayerGamepadExit()
    {
        return Gamepad.current != null && (Gamepad.current.leftTrigger.wasPressedThisFrame || Gamepad.current.rightTrigger.wasPressedThisFrame);
    }

    public static bool anyExit()
    {
        return keyboardExit() || playerKeyboardExit() || twoPlayerGamepadExit() || onePlayerGamepadExit();
    }
    #endregion
}
