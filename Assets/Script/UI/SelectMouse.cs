﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

namespace com.DungeonPad
{
    public class SelectMouse : MonoBehaviour
    {
        [SerializeField] GameObject[] P1UnSelectLight, P2UnSelectLight, p1Light, p2Light;
        [SerializeField] string NextScene;
        [Space]
        [SerializeField] Image selectBackGround;
        [SerializeField] Image p1select, p2select;
        [SerializeField] GameObject p1selectKeyboard, p1selectGamepad, p2selectKeyboard, p2selectGamepad;
        [SerializeField] Sprite SelectAll, UnSelectAll, p1selectTrue, p1selectFalse, p2selectTrue, p2selectFalse;
        float p1KeyboardTimer, p2KeyboardTimer, p1GamepadTimer, p2GamepadTimer, p1SingleTimer, p2SingleTimer;

        private void Start()
        {
            ReGamer.ReAbility();
            if (GameManager.CurrentSceneName == "SelectRole_Game 0")
            {
                PlayerManager.Life = 1;
            }
            InputManager.p1Mod = InputManager.PlayerMod.none;
            InputManager.p2Mod = InputManager.PlayerMod.none;
            InputManager.p1Gamepad = null;
            InputManager.p2Gamepad = null;
        }

        void Update()
        {
            InputManager.currentGamepad = Gamepad.current;
            if (GameManager.CurrentSceneName == "SelectRole_Game 0")
            {
                for (int i = 0; i < GameManager.players.childCount; i++)
                {
                    GameManager.players.GetChild(i).GetComponent<PlayerManager>().DashTimer = 0.3f;
                }
            }
            selectRole();
        }

        #region//selectRole
        void selectRole()
        {
            RemoveDevice();
            InputManager.p1Mod = back(InputManager.p1Mod);
            InputManager.p2Mod = back(InputManager.p2Mod);
            if (InputManager.p1Mod == InputManager.PlayerMod.none)
            {
                InputManager.p1Mod = select(true);
            }
            else if (InputManager.p2Mod == InputManager.PlayerMod.none)
            {
                InputManager.p2Mod = select(false);
            }

            Transform p1 = GameManager.players.GetChild(0), p2 = GameManager.players.GetChild(1);
            if (InputManager.p1Mod != InputManager.PlayerMod.none)
            {
                p1.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.Move;
                //p1.GetComponent<PlayerManager>().p1 = true;
                p1.GetComponent<PlayerJoyVibration>().enabled = true;
                p1select.sprite = p1selectTrue;
                if(InputManager.p1Mod == InputManager.PlayerMod.keyboardP1 || InputManager.p1Mod == InputManager.PlayerMod.keyboardP2)
                {
                    p1selectKeyboard.SetActive(true);
                    p1selectGamepad.SetActive(false);
                }
                else
                {
                    p1selectKeyboard.SetActive(false);
                    p1selectGamepad.SetActive(true);
                }
            }
            else
            {
                //p1.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                p1.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                p1.GetComponent<PlayerJoyVibration>().enabled = false;
                p1.GetComponent<PlayerManager>().v = Vector3.zero;
                p1.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                p1select.sprite = p1selectFalse;
                p1selectKeyboard.SetActive(false);
                p1selectGamepad.SetActive(false);
            }
            if (InputManager.p2Mod != InputManager.PlayerMod.none)
            {
                p2.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.Move;
                //p2.GetComponent<PlayerManager>().p1 = false;
                p2.GetComponent<PlayerJoyVibration>().enabled = true;
                p2select.sprite = p2selectTrue;
                if (InputManager.p2Mod == InputManager.PlayerMod.keyboardP1 || InputManager.p2Mod == InputManager.PlayerMod.keyboardP2)
                {
                    p2selectKeyboard.SetActive(true);
                    p2selectGamepad.SetActive(false);
                }
                else
                {
                    p2selectKeyboard.SetActive(false);
                    p2selectGamepad.SetActive(true);
                }
            }
            else
            {
                //p2.GetComponent<PlayerManager>().playerStat = PlayerManager.PlayerStat.UnSelect;
                p2.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                p2.GetComponent<PlayerJoyVibration>().enabled = false;
                p2.GetComponent<PlayerManager>().v = Vector3.zero;
                p2.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
                p2select.sprite = p2selectFalse;
                p2selectKeyboard.SetActive(false);
                p2selectGamepad.SetActive(false);
            }
            Debug.Log("p1 : " + InputManager.p1Mod.ToString() + "        " + "p2 : " + InputManager.p2Mod.ToString());
            if(InputManager.p1Mod != InputManager.PlayerMod.none && InputManager.p2Mod != InputManager.PlayerMod.none)
            {
                selectBackGround.sprite = SelectAll;
                switch (InputManager.p1Mod)
                {
                    case InputManager.PlayerMod.keyboardP1:
                        if (Keyboard.current.allKeys[InputManager.p1KeyboardDashNum].isPressed)
                        {
                            p1KeyboardTimer += Time.deltaTime;
                        }
                        else
                        {
                            p1KeyboardTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.keyboardP2:
                        if (Keyboard.current.allKeys[InputManager.p2KeyboardDashNum].isPressed)
                        {
                            p2KeyboardTimer += Time.deltaTime;
                        }
                        else
                        {
                            p2KeyboardTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.gamepadP1:
                        if (InputManager.p1Gamepad.aButton.isPressed)
                        {
                            p1GamepadTimer += Time.deltaTime;
                        }
                        else
                        {
                            p1GamepadTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.gamepadP2:
                        if (InputManager.p2Gamepad.aButton.isPressed)
                        {
                            p2GamepadTimer += Time.deltaTime;
                        }
                        else
                        {
                            p2GamepadTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.singleP1:
                        if (InputManager.currentGamepad.leftShoulder.isPressed)
                        {
                            p1SingleTimer += Time.deltaTime;
                        }
                        else
                        {
                            p1SingleTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.singleP2:
                        if (InputManager.currentGamepad.rightShoulder.isPressed)
                        {
                            p2SingleTimer += Time.deltaTime;
                        }
                        else
                        {
                            p2SingleTimer = 0;
                        }
                        break;
                }
                switch (InputManager.p2Mod)
                {
                    case InputManager.PlayerMod.keyboardP1:
                        if (Keyboard.current.allKeys[InputManager.p1KeyboardDashNum].isPressed)
                        {
                            p1KeyboardTimer += Time.deltaTime;
                        }
                        else
                        {
                            p1KeyboardTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.keyboardP2:
                        if (Keyboard.current.allKeys[InputManager.p2KeyboardDashNum].isPressed)
                        {
                            p2KeyboardTimer += Time.deltaTime;
                        }
                        else
                        {
                            p2KeyboardTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.gamepadP1:
                        if (InputManager.p1Gamepad.aButton.isPressed)
                        {
                            p1GamepadTimer += Time.deltaTime;
                        }
                        else
                        {
                            p1GamepadTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.gamepadP2:
                        if (InputManager.p2Gamepad.aButton.isPressed)
                        {
                            p2GamepadTimer += Time.deltaTime;
                        }
                        else
                        {
                            p2GamepadTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.singleP1:
                        if (InputManager.currentGamepad.leftShoulder.isPressed)
                        {
                            p1SingleTimer += Time.deltaTime;
                        }
                        else
                        {
                            p1SingleTimer = 0;
                        }
                        break;
                    case InputManager.PlayerMod.singleP2:
                        if (InputManager.currentGamepad.rightShoulder.isPressed)
                        {
                            p2SingleTimer += Time.deltaTime;
                        }
                        else
                        {
                            p2SingleTimer = 0;
                        }
                        break;
                }
                if (p1KeyboardTimer >= 0.5f || p2KeyboardTimer >= 0.5f || p1GamepadTimer >= 0.5f || p2GamepadTimer >= 0.5f || p1SingleTimer >= 0.5f || p2SingleTimer >= 0.5f)
                {
                    if (NextScene == "Game 3")
                    {
                        GameManager.layers = 2;
                        NextScene = "Game 1";
                    }
                    SwitchScenePanel.NextScene = NextScene;
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
            }
            else
            {
                selectBackGround.sprite = UnSelectAll;
                p1KeyboardTimer = 0;
                p2KeyboardTimer = 0;
                p1GamepadTimer = 0;
                p2GamepadTimer = 0;
                p1SingleTimer = 0;
                p2SingleTimer = 0;
            }
            for (int i = 0; i < P1UnSelectLight.Length; i++)
            {
                P1UnSelectLight[i].SetActive(InputManager.p1Mod == InputManager.PlayerMod.none);
            }
            for (int i = 0; i < P2UnSelectLight.Length; i++)
            {
                P2UnSelectLight[i].SetActive(InputManager.p2Mod == InputManager.PlayerMod.none);
            }
            for (int i = 0; i < p1Light.Length; i++)
            {
                p1Light[i].SetActive(InputManager.p1Mod != InputManager.PlayerMod.none);
            }
            for (int i = 0; i < p2Light.Length; i++)
            {
                p2Light[i].SetActive(InputManager.p2Mod != InputManager.PlayerMod.none);
            }
            if(InputManager.p1Mod == InputManager.PlayerMod.none)
            {
                p1.position = new Vector3(6.7f, 4.6f, p1.position.z);
            }
            if (InputManager.p2Mod == InputManager.PlayerMod.none)
            {
                p2.position = new Vector3(13.7f, 4.6f, p1.position.z);
            }
        }

        void RemoveDevice()
        {
            InputSystem.onDeviceChange += (device, change) =>
            {
                if(change == InputDeviceChange.Removed && device != null)
                {
                    if (device == InputManager.p1Gamepad)
                    {
                        InputManager.p1Mod = InputManager.PlayerMod.none;
                        InputManager.p1Gamepad = null;
                    }
                    if (device == InputManager.p2Gamepad)
                    {
                        InputManager.p2Mod = InputManager.PlayerMod.none;
                        InputManager.p2Gamepad = null;
                    }
                }
                /*
                switch (change)
                {
                    case InputDeviceChange.Added:
                        //Debug.Log("New device added: " + device);
                        break;

                    case InputDeviceChange.Removed:
                        //Debug.Log("Device removed: " + device);
                        if (device == p1Gamepad)
                        {
                            p1Mod = PlayerMod.none;
                            p1Gamepad = null;
                        }
                        if (device == p2Gamepad)
                        {
                            p2Mod = PlayerMod.none;
                            p2Gamepad = null;
                        }
                        break;
                }
                */
            };

        }

        InputManager.PlayerMod select(bool selectP1)
        {
            Gamepad pad = Gamepad.current, temp = null;
            InputManager.PlayerMod playerMod = InputManager.PlayerMod.none;
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.allKeys[InputManager.p1KeyboardDashNum].wasPressedThisFrame)
            {
                playerMod = InputManager.PlayerMod.keyboardP1;
            }
            else if (keyboard != null && keyboard.allKeys[InputManager.p2KeyboardDashNum].wasPressedThisFrame)
            {
                playerMod = InputManager.PlayerMod.keyboardP2;
            }
            else if (pad != null && pad.leftShoulder.wasPressedThisFrame && !InputManager.twoPlayerMode)
            {
                playerMod = InputManager.PlayerMod.singleP1;
            }
            else if (pad != null && pad.rightShoulder.wasPressedThisFrame && !InputManager.twoPlayerMode)
            {
                playerMod = InputManager.PlayerMod.singleP2;
            }
            else if (pad != null && pad.aButton.wasPressedThisFrame && InputManager.twoPlayerMode)
            {
                if (selectP1)
                {
                    playerMod = InputManager.PlayerMod.gamepadP1;
                    temp = pad;
                }
                else
                {
                    playerMod = InputManager.PlayerMod.gamepadP2;
                    temp = pad;
                }
                /*
                for (int i = 0; i < Gamepad.all.Count; i++)
                {
                    if (Gamepad.all[i].aButton.wasPressedThisFrame)
                    {
                        if(gamepad == p1Gamepad && Gamepad.all[i] != p2Gamepad)
                        {
                            playerMod = PlayerMod.gamepadP1;
                            p1Gamepad = Gamepad.all[i];
                        }
                        else if (gamepad == p2Gamepad && Gamepad.all[i] != p1Gamepad)
                        {
                            playerMod = PlayerMod.gamepadP2;
                            p2Gamepad = Gamepad.all[i];
                        }
                    }
                }
                */
            }
            if (InputManager.p1Mod == playerMod || InputManager.p2Mod == playerMod || (temp != null && (InputManager.p1Gamepad == temp || temp != null && (InputManager.p2Gamepad == temp))))
            {
                return InputManager.PlayerMod.none;
            }
            else
            {
                if (temp != null)
                {
                    if (selectP1)
                    {
                        InputManager.p1Gamepad = temp;
                    }
                    else
                    {
                        InputManager.p2Gamepad = temp;
                    }
                }
                return playerMod;
            }
        }

        InputManager.PlayerMod back(InputManager.PlayerMod playerMod)
        {
            Keyboard keyboard = Keyboard.current;
            Gamepad pad = Gamepad.current;
            switch (playerMod)
            {
                case InputManager.PlayerMod.keyboardP1:
                    if (keyboard != null && keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame)
                    {
                        playerMod = InputManager.PlayerMod.none;
                    }
                    break;
                case InputManager.PlayerMod.keyboardP2:
                    if (keyboard != null && keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame)
                    {
                        playerMod = InputManager.PlayerMod.none;
                    }
                    break;
                case InputManager.PlayerMod.gamepadP1:
                    if (InputManager.p1Gamepad != null && InputManager.p1Gamepad.bButton.wasPressedThisFrame)
                    {
                        playerMod = InputManager.PlayerMod.none;
                        InputManager.p1Gamepad = null;
                    }
                    break;
                case InputManager.PlayerMod.gamepadP2:
                    if (InputManager.p2Gamepad != null && InputManager.p2Gamepad.bButton.wasPressedThisFrame)
                    {
                        playerMod = InputManager.PlayerMod.none;
                        InputManager.p2Gamepad = null;
                    }
                    break;
                case InputManager.PlayerMod.singleP1:
                    if (pad != null && pad.leftTrigger.wasPressedThisFrame)
                    {
                        playerMod = InputManager.PlayerMod.none;
                    }
                    break;
                case InputManager.PlayerMod.singleP2:
                    if (pad != null && pad.rightTrigger.wasPressedThisFrame)
                    {
                        playerMod = InputManager.PlayerMod.none;
                    }
                    break;
            }
            return playerMod;
        }
        #endregion
    }
}