using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace com.DungeonPad
{
    public class Operate : MonoBehaviour
    {
        [SerializeField] Image panel;
        [SerializeField] Sprite[] Operates;
        [SerializeField] EventSystem eventSystem;
        [Space]
        [SerializeField] GameObject[] selectedButtons;
        [SerializeField] GameObject[] changingButtons;
        [SerializeField] Sprite[] keys;

        GameObject selected;

        void Start()
        {
            string[] keyboardNum = PlayerPrefs.GetString("Keyboard").Split(',');
            for (int i = 0; i < selectedButtons.Length; i++)
            {
                selectedButtons[i].GetComponent<Image>().sprite = keys[int.Parse(keyboardNum[i])];
            }
        }

        void Update()
        {
            selected = eventSystem.currentSelectedGameObject;
            switchPanel();
            changeingKey();
        }

        void switchPanel()
        {
            switch (selected.name.Split('_')[0])
            {
                case "gamepad2Player":
                    panel.sprite = Operates[0];
                    break;
                case "gamepad1Player":
                    panel.sprite = Operates[1];
                    break;
                case "keyboardP1":
                    panel.sprite = Operates[2];
                    break;
                case "keyboardP2":
                    panel.sprite = Operates[3];
                    break;
            }
        }

        public void preChangeKey()
        {
            selected = eventSystem.currentSelectedGameObject;
            int i;
            for (i = 0; i < selectedButtons.Length; i++)
            {
                if(selectedButtons[i] == selected)
                {
                    break;
                }
            }
            selectedButtons[i].GetComponent<Image>().enabled = false;
            eventSystem.SetSelectedGameObject(changingButtons[i]);
        }

        void changeingKey()
        {
            Keyboard keyboard = Keyboard.current;
            if (selected.name.Contains("nullKey") && keyboard != null)
            {
                int i, j;
                string[] keyboardNums = PlayerPrefs.GetString("Keyboard").Split(',');
                for (i = 0; i < Keyboard.KeyCount; i++)
                {
                    if (keyboard.allKeys[i].wasPressedThisFrame && keys[i] != null)
                    {
                        for(int k=0; k < selectedButtons.Length; k++)
                        {
                            if (int.Parse(keyboardNums[k]) == i)
                            {
                                return;
                            }
                        }
                        break;
                    }
                    /*
                    if (keyboard.allKeys[i].wasPressedThisFrame)
                    {
                        if (i <= 104 && (i <= 55 || (i >= 60 && i <= 63) || i >= 77) && i != 2 && i != 82)
                        {
                            break;
                        }
                    }
                    */
                }
                if (i >= Keyboard.KeyCount)
                {
                    return;
                }

                for (j = 0; j < selectedButtons.Length; j++)
                {
                    if (selectedButtons[j] == selected)
                    {
                        break;
                    }
                }
                switch (j)
                {
                    #region//P1
                    case 0:
                        InputManager.p1KeyboardUpNum = i;
                        break;
                    case 1:
                        InputManager.p1KeyboardDownNum = i;
                        break;
                    case 2:
                        InputManager.p1KeyboardLeftNum = i;
                        break;
                    case 3:
                        InputManager.p1KeyboardRightNum = i;
                        break;
                    case 4:
                        InputManager.p1KeyboardDashNum = i;
                        break;
                    case 5:
                        InputManager.p1KeyboardBreakfreeKeyNum = i;
                        break;
                    case 6:
                        InputManager.p1KeyboardSkillKeyNum = i;
                        break;
                    case 7:
                        InputManager.p1KeyboardLookskillKeyNum = i;
                        break;
                    #endregion
                    #region//P2
                    case 8:
                        InputManager.p2KeyboardUpNum = i;
                        break;
                    case 9:
                        InputManager.p2KeyboardDownNum = i;
                        break;
                    case 10:
                        InputManager.p2KeyboardLeftNum = i;
                        break;
                    case 11:
                        InputManager.p2KeyboardRightNum = i;
                        break;
                    case 12:
                        InputManager.p2KeyboardDashNum = i;
                        break;
                    case 13:
                        InputManager.p2KeyboardBreakfreeKeyNum = i;
                        break;
                    case 14:
                        InputManager.p2KeyboardSkillKeyNum = i;
                        break;
                    case 15:
                        InputManager.p2KeyboardLookskillKeyNum = i;
                        break;
                        #endregion
                }
                selectedButtons[j].GetComponent<Image>().sprite = keys[i];
                selectedButtons[j].GetComponent<Image>().enabled = true;
                eventSystem.SetSelectedGameObject(selectedButtons[j]);
                keyboardNums[j] = "" + i;
                string Keyboards = "";
                for (int k = 0; k < 16; k++)
                {
                    Keyboards += keyboardNums[k] + ",";
                }
                PlayerPrefs.SetString("Keyboard", Keyboards);
                PlayerPrefs.Save();
            }
        }
    }
}