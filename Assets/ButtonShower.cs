﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ButtonShower : MonoBehaviour
    {
        static float timer_1 = 0, timer_2 = 0;
        [SerializeField] float switchTime = 2;
        static bool neverUseThisFrame_1, neverUseThisFrame_2;
        public List<Sprite> buttons = new List<Sprite>();
        [HideInInspector] public int buttonNum = 0;
        [HideInInspector] public Image image;
        [SerializeField] bool AddAll;
        [SerializeField] bool AutoGetPlayerMode;
        [SerializeField] PlayerManager AutoGetPlayerMode_onlyThisPlayer;
        public enum Functions
        {
            Move,
            Enter,
            Back,
            BreakFree,
            Skill
        }
        public Functions function;
        public enum Buttons
        {
            gamepadMoveL,
            gamepadMoveR,
            gamepadA,
            gamepadB,
            gamepadX,
            gamepadLB,
            gamepadLT,
            gamepadRB,
            gamepadRT,
            keyboardMove,
            keyboardP1A,
            keyboardP1B,
            keyboardP1X,
            keyboardP2A,
            keyboardP2B,
            keyboardP2X,
        }
        public static Dictionary<Buttons, Sprite> buttonSprites = new Dictionary<Buttons, Sprite>();


        void Start()
        {
            if (buttonSprites.Count <= 0)
            {
                setButtonSprites();
            }
            image = GetComponent<Image>();
            if (AddAll)
            {
                addALL();
            }
            if (AutoGetPlayerMode && buttons.Count <= 0)
            {
                getUsingButton();
            }
            if (buttons.Count <= 0 && AutoGetPlayerMode_onlyThisPlayer != null)
            {
                if (AutoGetPlayerMode_onlyThisPlayer.p1)
                {
                    switch (InputManager.p1Mod)
                    {
                        case InputManager.PlayerMod.gamepadP1:
                        case InputManager.PlayerMod.gamepadP2:
                            addButtons(buttonSprites[Buttons.gamepadB]);
                            break;
                        case InputManager.PlayerMod.singleP1:
                            addButtons(buttonSprites[Buttons.gamepadLB]);
                            break;
                        case InputManager.PlayerMod.singleP2:
                            addButtons(buttonSprites[Buttons.gamepadRB]);
                            break;
                        case InputManager.PlayerMod.keyboardP1:
                            addButtons(buttonSprites[Buttons.keyboardP1B]);
                            break;
                        case InputManager.PlayerMod.keyboardP2:
                            addButtons(buttonSprites[Buttons.keyboardP2B]);
                            break;
                    }
                }
                else
                {
                    switch (InputManager.p2Mod)
                    {
                        case InputManager.PlayerMod.gamepadP1:
                        case InputManager.PlayerMod.gamepadP2:
                            addButtons(buttonSprites[Buttons.gamepadB]);
                            break;
                        case InputManager.PlayerMod.singleP1:
                            addButtons(buttonSprites[Buttons.gamepadLB]);
                            break;
                        case InputManager.PlayerMod.singleP2:
                            addButtons(buttonSprites[Buttons.gamepadRB]);
                            break;
                        case InputManager.PlayerMod.keyboardP1:
                            addButtons(buttonSprites[Buttons.keyboardP1B]);
                            break;
                        case InputManager.PlayerMod.keyboardP2:
                            addButtons(buttonSprites[Buttons.keyboardP2B]);
                            break;
                    }
                }
            }
            if (buttons.Count > 0)
            {
                image.sprite = buttons[buttonNum];
            }
        }

        void Update()
        {
            if (!neverUseThisFrame_1)
            {
                neverUseThisFrame_1 = true;
                timer_1 += Time.unscaledDeltaTime;
            }
            if (!neverUseThisFrame_2)
            {
                timer_2 += Time.unscaledDeltaTime;
                neverUseThisFrame_2 = true;
            }
            if (switchTime > 1.5f)
            {
                if (timer_2 >= switchTime)
                {
                    if (++buttonNum >= buttons.Count)
                    {
                        buttonNum = 0;
                    }
                    image.sprite = buttons[buttonNum];
                }
                image.color = new Color(1, 1, 1, ((switchTime / 2 * 0.8f) - Mathf.Pow(Mathf.Abs(timer_2 - (switchTime / 2)), switchTime * 1.5f)) / (switchTime / 2));
            }
            else
            {
                if (timer_1 >= switchTime)
                {
                    if (++buttonNum >= buttons.Count)
                    {
                        buttonNum = 0;
                    }
                    image.sprite = buttons[buttonNum];
                }
                image.color = new Color(1, 1, 1, ((switchTime / 2 * 0.8f) - Mathf.Pow(Mathf.Abs(timer_1 - (switchTime / 2)), switchTime * 1.5f)) / (switchTime / 2));
            }
        }


        private void LateUpdate()
        {
            if (timer_1 >= switchTime)
            {
                timer_1 = 0;
            }
            if (timer_2 >= switchTime)
            {
                timer_2 = 0;
            }
            neverUseThisFrame_1 = false;
            neverUseThisFrame_2 = false;
        }
        /*
        WaitForEndOfFrame waitForEnd = new WaitForEndOfFrame();

        IEnumerator Timer()
        {
            timer_2 += Time.unscaledDeltaTime;
            timer_1 += Time.unscaledDeltaTime;
            yield return waitForEnd;

            Debug.Log("front : " + neverUseThisFrame_1 + ", " + neverUseThisFrame_2);
            if (switchTime > 1.5f)
            {
                if (timer_2 >= switchTime)
                {
                    timer_2 = 0;
                }
                neverUseThisFrame_2 = false;
            }
            else
            {
                if (timer_1 >= switchTime)
                {
                    timer_1 = 0;
                }
                neverUseThisFrame_1 = false;
            }
            Debug.Log("hide");

        }
        */
        public void selected()
        {
            buttonNum = 0;
            image.sprite = buttons[buttonNum];
        }

        public void addButtons(Sprite sprite)
        {
            if (!buttons.Contains(sprite))
            {
                buttons.Add(sprite);
            }
        }

        public void reset()
        {
            timer_2 = 0;
            buttonNum = 0;
            image.sprite = buttons[buttonNum];
        }

        public static void setButtonSprites()
        {
            buttonSprites.Clear();
            buttonSprites[Buttons.gamepadMoveL] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadMoveL");
            buttonSprites[Buttons.gamepadMoveR] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadMoveR");
            buttonSprites[Buttons.gamepadA] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadA");
            buttonSprites[Buttons.gamepadB] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadB");
            buttonSprites[Buttons.gamepadX] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadX");
            buttonSprites[Buttons.gamepadLB] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadLB");
            buttonSprites[Buttons.gamepadLT] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadLT");
            buttonSprites[Buttons.gamepadRB] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadRB");
            buttonSprites[Buttons.gamepadRT] = Resources.Load<Sprite>("UI/ButtonSprites/gamepadRT");
            buttonSprites[Buttons.keyboardMove] = Resources.Load<Sprite>("UI/ButtonSprites/keyboardMove");
            buttonSprites[Buttons.keyboardP1A] = Resources.Load<Sprite>("UI/ButtonSprites/" + InputManager.p1KeyboardDashNum);
            buttonSprites[Buttons.keyboardP1B] = Resources.Load<Sprite>("UI/ButtonSprites/" + InputManager.p1KeyboardBreakfreeKeyNum);
            buttonSprites[Buttons.keyboardP1X] = Resources.Load<Sprite>("UI/ButtonSprites/" + InputManager.p1KeyboardSkillKeyNum);
            buttonSprites[Buttons.keyboardP2A] = Resources.Load<Sprite>("UI/ButtonSprites/" + InputManager.p2KeyboardDashNum);
            buttonSprites[Buttons.keyboardP2B] = Resources.Load<Sprite>("UI/ButtonSprites/" + InputManager.p2KeyboardBreakfreeKeyNum);
            buttonSprites[Buttons.keyboardP2X] = Resources.Load<Sprite>("UI/ButtonSprites/" + InputManager.p2KeyboardSkillKeyNum);
        }

        void getUsingButton()
        {
            if (InputManager.p1Mod == InputManager.PlayerMod.gamepadP1 || InputManager.p1Mod == InputManager.PlayerMod.gamepadP2 || InputManager.p2Mod == InputManager.PlayerMod.gamepadP1 || InputManager.p2Mod == InputManager.PlayerMod.gamepadP2)
            {
                switch (function)
                {
                    case Functions.Move:
                        addButtons(buttonSprites[Buttons.gamepadMoveL]);
                        break;
                    case Functions.Enter:
                        addButtons(buttonSprites[Buttons.gamepadA]);
                        break;
                    case Functions.Back:
                        addButtons(buttonSprites[Buttons.gamepadB]);
                        break;
                    case Functions.BreakFree:
                        addButtons(buttonSprites[Buttons.gamepadB]);
                        break;
                    case Functions.Skill:
                        addButtons(buttonSprites[Buttons.gamepadX]);
                        break;
                }
            }
            if (InputManager.p1Mod == InputManager.PlayerMod.singleP1 || InputManager.p2Mod == InputManager.PlayerMod.singleP1)
            {
                switch (function)
                {
                    case Functions.Move:
                        addButtons(buttonSprites[Buttons.gamepadMoveL]);
                        break;
                    case Functions.Enter:
                        addButtons(buttonSprites[Buttons.gamepadLB]);
                        break;
                    case Functions.Back:
                        addButtons(buttonSprites[Buttons.gamepadLT]);
                        break;
                    case Functions.BreakFree:
                        addButtons(buttonSprites[Buttons.gamepadLB]);
                        break;
                    case Functions.Skill:
                        addButtons(buttonSprites[Buttons.gamepadLT]);
                        break;
                }
            }
            if (InputManager.p1Mod == InputManager.PlayerMod.singleP2 || InputManager.p2Mod == InputManager.PlayerMod.singleP2)
            {
                switch (function)
                {
                    case Functions.Move:
                        addButtons(buttonSprites[Buttons.gamepadMoveR]);
                        break;
                    case Functions.Enter:
                        addButtons(buttonSprites[Buttons.gamepadRB]);
                        break;
                    case Functions.Back:
                        addButtons(buttonSprites[Buttons.gamepadRT]);
                        break;
                    case Functions.BreakFree:
                        addButtons(buttonSprites[Buttons.gamepadRB]);
                        break;
                    case Functions.Skill:
                        addButtons(buttonSprites[Buttons.gamepadRT]);
                        break;
                }
            }
            if (InputManager.p1Mod == InputManager.PlayerMod.keyboardP1 || InputManager.p2Mod == InputManager.PlayerMod.keyboardP1)
            {
                switch (function)
                {
                    case Functions.Move:
                        addButtons(buttonSprites[Buttons.keyboardMove]);
                        break;
                    case Functions.Enter:
                        addButtons(buttonSprites[Buttons.keyboardP1A]);
                        break;
                    case Functions.Back:
                        addButtons(buttonSprites[Buttons.keyboardP1B]);
                        break;
                    case Functions.BreakFree:
                        addButtons(buttonSprites[Buttons.keyboardP1B]);
                        break;
                    case Functions.Skill:
                        addButtons(buttonSprites[Buttons.keyboardP1X]);
                        break;
                }
            }
            if (InputManager.p1Mod == InputManager.PlayerMod.keyboardP2 || InputManager.p2Mod == InputManager.PlayerMod.keyboardP2)
            {
                switch (function)
                {
                    case Functions.Move:
                        addButtons(buttonSprites[Buttons.keyboardMove]);
                        break;
                    case Functions.Enter:
                        addButtons(buttonSprites[Buttons.keyboardP2A]);
                        break;
                    case Functions.Back:
                        addButtons(buttonSprites[Buttons.keyboardP2B]);
                        break;
                    case Functions.BreakFree:
                        addButtons(buttonSprites[Buttons.keyboardP2B]);
                        break;
                    case Functions.Skill:
                        addButtons(buttonSprites[Buttons.keyboardP2X]);
                        break;
                }
            }
        }

        public void addALL()
        {
            buttons.Clear();
            switch (function)
            {
                case Functions.Enter:
                    if (Gamepad.all.Count > 0)
                    {
                        if (InputManager.twoPlayerMode)
                        {
                            addButtons(buttonSprites[Buttons.gamepadA]);
                        }
                        else
                        {
                            addButtons(buttonSprites[Buttons.gamepadLB]);
                            addButtons(buttonSprites[Buttons.gamepadRB]);
                        }
                    }
                    addButtons(buttonSprites[Buttons.keyboardP1A]);
                    addButtons(buttonSprites[Buttons.keyboardP2A]);
                    break;
                case Functions.Back:
                    if (Gamepad.all.Count > 0)
                    {
                        if (InputManager.twoPlayerMode)
                        {
                            addButtons(buttonSprites[Buttons.gamepadB]);
                        }
                        else
                        {
                            addButtons(buttonSprites[Buttons.gamepadLT]);
                            addButtons(buttonSprites[Buttons.gamepadRT]);
                        }
                    }
                    addButtons(buttonSprites[Buttons.keyboardP1B]);
                    addButtons(buttonSprites[Buttons.keyboardP2B]);
                    break;
            }
            selected();
        }
    }
}