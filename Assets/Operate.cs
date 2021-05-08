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
        [SerializeField] RectTransform buttons;
        [SerializeField] Sprite[] Operates;
        [SerializeField] EventSystem eventSystem;
        [Space]
        [SerializeField] GameObject[] selectedButtons;
        [SerializeField] GameObject[] changingButtons;
        [SerializeField] Sprite[] selectedKeys;
        [SerializeField] Sprite[] unSelectedKeys;

        GameObject selected;
        Keyboard keyboard;

        bool changing = false, changingFrame = false;
        void Start()
        {
            setKeySprite();
        }


        void Update()
        {
            changing = false;
            keyboard = Keyboard.current;
            selected = eventSystem.currentSelectedGameObject;
            switchPanel();
            if (!changingFrame)
            {
                changeingKey();
            }
            else
            {
                changingFrame = false;
            }
            if (keyboard != null && selected.transform.parent.parent.name != "ChangingButtons")
            {
                if (InputManager.anyExit())
                {
                    ButtonSelect.OnClicked();
                    SwitchScenePanel.NextScene = "Home";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                }
            }
        }

        void setKeySprite()
        {
            InputManager.keyboardSetting();
            string[] keyboardNum = PlayerPrefs.GetString("Keyboard").Split(',');
            int i;
            for (i = 0; i < selectedButtons.Length; i++)
            {
                setButtonSprite(selectedButtons[i], int.Parse(keyboardNum[i]));
            }
        }

        void switchPanel()
        {
            switch (selected.transform.parent.name)
            {
                case "2PlayerGamepad":
                    panel.sprite = Operates[0];
                    buttons.anchoredPosition = Vector2.left * 10000 * 0;
                    break;
                case "1PlayerGamepad":
                    panel.sprite = Operates[1];
                    buttons.anchoredPosition = Vector2.left * 10000 * 1;
                    break;
                case "P1Keyboard":
                    panel.sprite = Operates[2];
                    buttons.anchoredPosition = Vector2.left * 10000 * 2;
                    break;
                case "P2Keyboard":
                    panel.sprite = Operates[3];
                    buttons.anchoredPosition = Vector2.left * 10000 * 3;
                    break;
            }
        }

        public void preChangeKey()
        {
            if (!changing)
            {
                ButtonSelect.OnClicked();
                selected = eventSystem.currentSelectedGameObject;
                int i;
                for (i = 0; i < selectedButtons.Length; i++)
                {
                    if (selectedButtons[i] == selected)
                    {
                        break;
                    }
                }
                selectedButtons[i].GetComponent<Image>().enabled = false;
                eventSystem.SetSelectedGameObject(changingButtons[i]);
                changingFrame = true;
            }
        }

        void changeingKey()
        {
            if (selected.transform.parent.parent.name == "ChangingButtons" && keyboard != null)
            {
                int i, j;
                for (j = 0; j < changingButtons.Length; j++)
                {
                    if (changingButtons[j] == selected)
                    {
                        break;
                    }
                }
                if (InputManager.anyExit())
                {
                    ButtonSelect.OnClicked();
                    changing = true;
                    setButtonSprite(selectedButtons[j], int.Parse(PlayerPrefs.GetString("Keyboard").Split(',')[j]));
                    selectedButtons[j].GetComponent<Image>().enabled = true;
                    eventSystem.SetSelectedGameObject(selectedButtons[j]);
                    return;
                }
                string[] keyboardNums = PlayerPrefs.GetString("Keyboard").Split(',');
                for (i = 0; i < Keyboard.KeyCount; i++)
                {
                    if (keyboard.allKeys[i].wasPressedThisFrame && i < unSelectedKeys.Length && unSelectedKeys[i] != null)
                    {
                        Debug.Log("test");
                        for(int k=0; k < selectedButtons.Length; k++)
                        {
                            if (k != j && int.Parse(keyboardNums[k]) == i)
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
                ButtonSelect.OnClicked();
                changing = true;
                setButtonSprite(selectedButtons[j], i);
                selectedButtons[j].GetComponent<Image>().enabled = true;
                eventSystem.SetSelectedGameObject(selectedButtons[j]);
                keyboardNums[j] = "" + i;
                string Keyboards = "";
                for (int k = 0; k < selectedButtons.Length; k++)
                {
                    Keyboards += keyboardNums[k] + ",";
                }
                PlayerPrefs.SetString("Keyboard", Keyboards);
                PlayerPrefs.Save();
                ButtonShower.setButtonSprites();
                Debug.Log("setButtonSprites");
            }
        }

        void setButtonSprite(GameObject button , int keyNum)
        {
            button.GetComponent<Image>().sprite = unSelectedKeys[keyNum];
            SpriteState spriteState = button.GetComponent<Button>().spriteState;
            spriteState.highlightedSprite = selectedKeys[keyNum];
            spriteState.pressedSprite = selectedKeys[keyNum];
            spriteState.selectedSprite = selectedKeys[keyNum];
            spriteState.disabledSprite = unSelectedKeys[keyNum];
            button.GetComponent<Button>().spriteState = spriteState;
        }

        public void restKey()
        {
            PlayerPrefs.DeleteKey("Keyboard");
            setKeySprite();
        }
    }
}