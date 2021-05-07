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
        [SerializeField] GameObject[] Xs;

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
            for(int i = 0; i < 2; i++)
            {
                Xs[i].SetActive(text.sprite.name == "傳送Lv" + (i + 1) && text.enabled);
            }

            if (rotate != 0)
            {
                abilityRandomer();
            }

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
            InputManager.currentGamepad = Gamepad.current;
            if (keyboard.allKeys[InputManager.p1KeyboardLookskillKeyNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardLookskillKeyNum].wasPressedThisFrame || InputManager.currentGamepad != null && InputManager.currentGamepad.selectButton.wasPressedThisFrame)
            {
                if (showSelected || Time.timeScale <= 0.1f)
                {
                    showSelected = false;
                }
                else if (abilityNamesAndLevels.Count > 0)
                {
                    selectedNum = 0;
                    showSelected = true;
                    ButtonSelect.OnClicked();
                    eventSystem.SetSelectedGameObject(selects[0]);
                    for(int i = 0; i < selects.Length; i++)
                    {
                        selects[i].GetComponent<Image>().enabled = (i < abilityNamesAndLevels.Count);
                    }
                }
            }
            if((showSelected || Time.timeScale <= 0.1f) && InputManager.anyExit())
            {
                showSelected = false;
                ButtonSelect.OnClicked();
            }
            if (Time.timeScale <= 0.1f)
            {
                showSelected = false;
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
                if (!GameManager.shopPanel.activeSelf && !GameManager.stopPanel.activeSelf && !GameManager.settingPanel.activeSelf)
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

        public int rotate = 0;
        public string abilityName;
        float speed = 800;
        [SerializeField] Image randomer;
        [SerializeField] Sprite[] randomeSprites;
        float scaleTimer = -1f;
        public void abilityRandomer()
        {
            randomer.enabled = true;
            if (rotate == 1)
            {
                speed = Mathf.Lerp(speed, 10, Time.deltaTime / 2);
            }
            else if(rotate == 2)
            {
                speed = 160;
            }
            randomer.transform.Rotate(0, speed * Time.deltaTime, 0);
            if (rotate == 2 && (Mathf.Abs(randomer.transform.eulerAngles.y - 0) < 5 || Mathf.Abs(randomer.transform.eulerAngles.y - 180) < 5))
            {
                speed = 0;
                rotate = 3;
            }
            else if (rotate == 1 && randomer.transform.eulerAngles.y >= 90 && randomer.transform.eulerAngles.y < 270)
            {
                if(speed <= 160)
                {
                    rotate = 2;
                    randomer.transform.eulerAngles = new Vector3(0, -90, 0);
                    randomer.sprite = Resources.Load<Sprite>("UI/Ability/AbilityImage/" + abilityName + "Lv1");
                }
                else
                {
                    randomer.transform.eulerAngles = new Vector3(0, -90, 0);
                    randomer.sprite = randomeSprites[Random.Range(0, randomeSprites.Length)];
                 }
            }
            else if(rotate == 3)
            {
                scaleTimer += Time.deltaTime;
                if (scaleTimer < 0 && scaleTimer >= -0.5f)
                {
                    randomer.transform.localScale = Vector3.one * (1 - Mathf.Abs(scaleTimer) / 3);
                }
                else if(scaleTimer >= 0)
                {
                    randomer.transform.localScale = Vector3.one * (1 - Mathf.Abs(scaleTimer) * 2);
                }
                if (randomer.transform.localScale.x <= 0)
                {
                    scaleTimer = -1f;
                    randomer.transform.localScale = Vector3.one * (1 - Mathf.Abs(scaleTimer) / 3);
                    rotate = 0;
                    speed = 1000;
                    randomer.enabled = false;
                    try
                    {
                        AbilityData.setPlayerAbility(abilityName, ++AbilityManager.AbilityCurrentLevel[abilityName]);
                    }
                    catch
                    {
                        Debug.LogError("能力有問題");
                    }
                    try
                    {
                        GameManager.showAbilityDetail.showDetail(abilityName + "Lv" + AbilityManager.AbilityCurrentLevel[abilityName]);
                    }
                    catch
                    {
                        Debug.LogError("找不到 : 能力敘述文字");
                    }
                }
            }
        }
    }
}