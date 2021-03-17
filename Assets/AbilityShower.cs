using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class AbilityShower : MonoBehaviour
    {
        public Image[] abilityImages;
        public Image[] homeButtonTimerImages;
        [SerializeField] GameObject[] selects;
        [SerializeField] GameObject select_None;
        [SerializeField] Image text;
        public static bool showSelected;
        int selectedNum = 0;
        public static List<string> abilityNamesAndLevels = new List<string>();
        EventSystem eventSystem;
        void Start()
        {
            eventSystem = EventSystem.current;
            string[] ability;
            for (int i = 0; i < abilityNamesAndLevels.Count; i++)
            {
                ability = System.Text.RegularExpressions.Regex.Split(abilityNamesAndLevels[i], "Lv");
                Debug.LogError(ability[0] + ",,,," + ability[1]);
                setAbility(ability[0], int.Parse(ability[1]));
                if (i > 50)
                {
                    break;
                }
            }
        }

        void Update()
        {
            if (PlayerManager.homeButton)
            {
                for(int i = 0; i < homeButtonTimerImages.Length; i++)
                {
                    if (homeButtonTimerImages[i].enabled)
                    {
                        homeButtonTimerImages[i].fillAmount = PlayerManager.homeButtonTimer / (-PlayerManager.homeButtonTimerStoper * 2);
                        break;
                    }
                }
            }

            Keyboard keyboard = Keyboard.current;
            if (keyboard.allKeys[InputManager.p1KeyboardLookskillKeyNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardLookskillKeyNum].wasPressedThisFrame || Gamepad.current.selectButton.wasPressedThisFrame)
            {
                if (showSelected)
                {
                    showSelected = false;
                }
                else if (abilityNamesAndLevels.Count > 0)
                {
                    selectedNum = 0;
                    showSelected = true;
                    eventSystem.SetSelectedGameObject(selects[0]);
                    for(int i = 0; i < selects.Length; i++)
                    {
                        selects[i].GetComponent<Image>().enabled = (i < abilityNamesAndLevels.Count);
                    }
                }
            }
            text.enabled = showSelected;
            if (showSelected)
            {
                if (eventSystem.currentSelectedGameObject == selects[abilityNamesAndLevels.Count])
                {
                    eventSystem.SetSelectedGameObject(selects[0]);
                }
                else if(eventSystem.currentSelectedGameObject == select_None)
                {
                    eventSystem.SetSelectedGameObject(selects[abilityNamesAndLevels.Count - 1]);
                }
                for(int i=0; i < selects.Length; i++)
                {
                    if(eventSystem.currentSelectedGameObject == selects[i] && eventSystem.currentSelectedGameObject.GetComponent<Image>().enabled)
                    {
                        text.sprite = Resources.Load<Sprite>("UI/Ability/AbilityText/" + abilityImages[i].sprite.name);
                    }
                }
            }
            else
            {
                if (!GameManager.shopPanel.activeSelf && !GameManager.stopPanel.activeSelf)
                {
                    eventSystem.SetSelectedGameObject(select_None);
                }
            }
        }

        public void setAbility(string abilityName, int abilityLevel)
        {
            for (int i = 0; i < abilityImages.Length; i++)
            {
                if ((abilityImages[i].sprite && abilityImages[i].sprite.name.Contains(abilityName)) || !abilityImages[i].enabled)
                {
                    if(abilityNamesAndLevels.Count>i)
                    {
                        abilityNamesAndLevels.RemoveAt(i);
                    }
                    if (abilityLevel == 0)
                    {
                        abilityImages[i].enabled = false;
                        abilityImages[i].sprite = null;
                        if (abilityName == "傳送")
                        {
                            homeButtonTimerImages[i].enabled = false;
                            homeButtonTimerImages[i].sprite = null;
                        }
                        abilityOrder();
                        return;
                    }
                    else
                    {
                        abilityNamesAndLevels.Insert(i ,abilityName + "Lv" + abilityLevel);
                        abilityImages[i].enabled = true;
                        abilityImages[i].sprite = Resources.Load<Sprite>("UI/Ability/AbilityImage/" + abilityName + "Lv" + abilityLevel);
                        if (abilityName == "傳送")
                        {
                            homeButtonTimerImages[i].enabled = true;
                            homeButtonTimerImages[i].sprite = Resources.Load<Sprite>("UI/Ability/AbilityImage/" + abilityName + "Lv" + abilityLevel + "(黑)");
                        }
                        abilityOrder();
                        return;
                    }
                }
            }
            Debug.LogError("能力數量超過上限");
        }

        public void abilityOrder()
        {
            List<int> emptyAbilityOrder = new List<int>();
            for (int i = 0; i < abilityImages.Length; i++)
            {
                if (!abilityImages[i].enabled)
                {
                    emptyAbilityOrder.Add(i);
                }
                else if (emptyAbilityOrder.Count != 0)
                {
                    abilityImages[emptyAbilityOrder[0]].enabled = true;
                    abilityImages[emptyAbilityOrder[0]].sprite = abilityImages[i].sprite;
                    abilityImages[i].enabled = false;
                    abilityImages[i].sprite = null;
                    if (abilityImages[emptyAbilityOrder[0]].sprite.name.Contains("傳送"))
                    {
                        homeButtonTimerImages[emptyAbilityOrder[0]].enabled = true;
                        homeButtonTimerImages[emptyAbilityOrder[0]].sprite = homeButtonTimerImages[i].sprite;
                        homeButtonTimerImages[i].enabled = false;
                        homeButtonTimerImages[i].sprite = null;
                    }
                    emptyAbilityOrder.RemoveAt(0);
                    emptyAbilityOrder.Add(i);
                }
            }
        }
    }
}