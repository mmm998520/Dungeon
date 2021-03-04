using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class AbilityShower : MonoBehaviour
    {
        public Image[] abilityImages;
        public Image[] homeButtonTimerImages;

        public static List<string> abilityNamesAndLevels = new List<string>();

        void Start()
        {
            string[] ability;
            /*
            for (int i = 0; i < abilityNamesAndLevels.Count; i++)
            {
                ability = System.Text.RegularExpressions.Regex.Split(abilityNamesAndLevels[i], "Lv");
                setAbility(ability[0], int.Parse(ability[1]));
                if (i > 50)
                {
                    break;
                }
            }*/
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
        }

        public void setAbility(string abilityName, int abilityLevel)
        {
            for (int i = 0; i < abilityImages.Length; i++)
            {
                if ((abilityImages[i].sprite && abilityImages[i].sprite.name.Contains(abilityName)) || !abilityImages[i].enabled)
                {
                    for (int j = 0; j < 5; i++)
                    {
                        abilityNamesAndLevels.Remove(abilityName + "Lv" + j);
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
                        abilityNamesAndLevels.Add(abilityName + "Lv" + abilityLevel);
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