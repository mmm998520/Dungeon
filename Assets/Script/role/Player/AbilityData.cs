using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

namespace com.DungeonPad
{
    public class AbilityData : MonoBehaviour
    {
        public Sprite Loaded, Loading, UnLoad;

        [Header("個別輸入")]
        public int dataNum;
        public Text detailName, detail, detailMoneyA, detailMoneyB;
        public GameObject detailForever, detailCosts;

        [Header("統整輸入")]
        public Text Name;
        public GameObject forever, costs;
        public Text Num;

        Ability.ability ability;
        public string abilityName;

        public Button plusButton, minusButton, unlockButton;
        public Sprite canUsePlus, cantUsePlus, canUsePlusSelected, cantUsePlusSelected;
        public Sprite canUseMinus, cantUseMinus, canUseMinusSelected, cantUseMinusSelected;
        public Sprite canUseUnlock, cantUseUnlock, canUseUnlockSelected, cantUseUnlockSelected;

        public void Awake()
        {
            if (!SceneManager.GetActiveScene().name.Contains("SelectRole_Game 1"))
            {
                Debug.LogError("212311651");
                awake();
            }
        }

        public void awake()
        {
            if (dataNum >= AbilityManager.AbilityCurrentLevel.Count)
            {
                gameObject.SetActive(false);
                return;
            }
            ability = AbilityManager.Abilitys[AbilityManager.myAbilitys[dataNum]];
            abilityName = ability.name;
            setAbilityBar();
            setPlayerAbility(abilityName, AbilityManager.AbilityCurrentLevel[abilityName]);
        }

        public void Update()
        {
            setButtonSpriteState();
        }

        void setAbilityBar()
        {
            Name.text = ability.name;
            if (ability.forever)
            {
                forever.SetActive(true);
                costs.SetActive(false);
            }
            else
            {
                forever.SetActive(false);
                costs.SetActive(true);
                Transform costsTransform = costs.transform;
                int i, temp;
                int costNum = 0;
                for (i = 0; i <= AbilityManager.AbilityCurrentLevel[ability.name]; i++)
                {
                    costNum += ability.cost[i];
                }
                temp = i;
                for (i = 0; i < costsTransform.childCount; i++)
                {
                    if(i < costNum)
                    {
                        costsTransform.GetChild(i).GetComponent<Image>().sprite = Loaded;
                    }
                    else if(AbilityManager.AbilityCurrentLevel[ability.name] + 1 <= AbilityManager.AbilityCanUseLevel[ability.name] && i < costNum + ability.cost[AbilityManager.AbilityCurrentLevel[ability.name] + 1])
                    {
                        costsTransform.GetChild(i).GetComponent<Image>().sprite = Loading;
                    }
                    else
                    {
                        costsTransform.GetChild(i).GetComponent<Image>().sprite = UnLoad;
                    }
                }
                for (i = temp; i <= AbilityManager.AbilityCanUseLevel[ability.name]; i++)
                {
                    costNum += ability.cost[i];
                }
                for (i = 0; i < costsTransform.childCount; i++)
                {
                    costsTransform.GetChild(i).gameObject.SetActive(i < costNum);
                }

            }
            Debug.LogError(Num.text);
            Debug.LogError(ability.name);
            Debug.LogError(AbilityManager.AbilityCurrentLevel[ability.name]);
            Num.text = ability.Num[AbilityManager.AbilityCurrentLevel[ability.name]];
        }

        public void setDetail()
        {
            detailName.text = ability.name;
            detail.text = ability.detail[AbilityManager.AbilityCurrentLevel[ability.name]];
            if(AbilityManager.AbilityCanUseLevel[ability.name] + 1 <= AbilityManager.AbilityCanBuyLevel[ability.name])
            {
                detailMoneyA.transform.parent.gameObject.SetActive(true);
                detailMoneyB.transform.parent.gameObject.SetActive(true);
                detailMoneyA.text = "" + ability.moneyA[AbilityManager.AbilityCanUseLevel[ability.name] + 1];
                detailMoneyB.text = "" + ability.moneyB[AbilityManager.AbilityCanUseLevel[ability.name] + 1];
            }
            else
            {
                detailMoneyA.transform.parent.gameObject.SetActive(false);
                detailMoneyB.transform.parent.gameObject.SetActive(false);
            }
            if (ability.forever)
            {
                detailForever.SetActive(true);
                detailCosts.SetActive(false);
            }
            else
            {
                detailForever.SetActive(false);
                detailCosts.SetActive(true);
                Transform detailCostsTransform = detailCosts.transform;
                if (AbilityManager.AbilityCanUseLevel[ability.name] + 1 <= AbilityManager.AbilityCanBuyLevel[ability.name])
                {
                    for (int i = 0; i < detailCostsTransform.childCount; i++)
                    {
                        detailCostsTransform.GetChild(i).gameObject.SetActive(i < ability.cost[AbilityManager.AbilityCanUseLevel[ability.name] + 1]);
                    }
                }
            }
        }

        public void Add()
        {
            if(AbilityManager.AbilityCurrentLevel[abilityName] < AbilityManager.AbilityCanUseLevel[abilityName] && AbilityManager.Costed + ability.cost[AbilityManager.AbilityCurrentLevel[abilityName] + 1] <= AbilityManager.TotalCost)
            {
                AbilityManager.AbilityCurrentLevel[abilityName]++;
                //AbilityManager.Costed += ability.cost[AbilityManager.AbilityCurrentLevel[abilityName]];
            }
            setAbilityBar();
            setDetail();
            setPlayerAbility(abilityName, AbilityManager.AbilityCurrentLevel[abilityName]);
            DataSaver.Save();

        }

        public void Less()
        {
            if (AbilityManager.AbilityCurrentLevel[abilityName] > 0 && !ability.forever)
            {
                //AbilityManager.Costed -= ability.cost[AbilityManager.AbilityCurrentLevel[abilityName]];
                AbilityManager.AbilityCurrentLevel[abilityName]--;
            }
            setAbilityBar();
            setDetail();
            setPlayerAbility(abilityName, AbilityManager.AbilityCurrentLevel[abilityName]);
            DataSaver.Save();

        }

        public void Unlock()
        {
            if(AbilityManager.AbilityCanUseLevel[abilityName] < AbilityManager.AbilityCanBuyLevel[abilityName])
            {
                bool moneyA = PlayerManager.money >= ability.moneyA[AbilityManager.AbilityCanUseLevel[abilityName] + 1];
                bool moneyB = PlayerManager.moneyB >= ability.moneyB[AbilityManager.AbilityCanUseLevel[abilityName] + 1];
                //if (moneyA && moneyB)
                {
                    PlayerManager.money -= ability.moneyA[AbilityManager.AbilityCanUseLevel[abilityName] + 1];
                    PlayerManager.moneyB -= ability.moneyB[AbilityManager.AbilityCanUseLevel[abilityName] + 1];
                    AbilityManager.AbilityCanUseLevel[abilityName]++;
                    if (ability.forever)
                    {
                        AbilityManager.AbilityCurrentLevel[abilityName] = AbilityManager.AbilityCanUseLevel[abilityName];
                    }
                    setAbilityBar();
                    setDetail();
                    setPlayerAbility(abilityName, AbilityManager.AbilityCurrentLevel[abilityName]);
                    DataSaver.Save();
                }
            }
        }

        public void setButtonSpriteState()
        {
            SpriteState spriteState;
            if (AbilityManager.AbilityCurrentLevel[abilityName] < AbilityManager.AbilityCanUseLevel[abilityName] && AbilityManager.Costed + ability.cost[AbilityManager.AbilityCurrentLevel[abilityName] + 1] <= AbilityManager.TotalCost)
            {
                plusButton.image.sprite = canUsePlus;
                spriteState = plusButton.spriteState;
                spriteState.highlightedSprite = canUsePlusSelected;
                plusButton.spriteState = spriteState;
                plusButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            }
            else
            {
                plusButton.image.sprite = cantUsePlus;
                spriteState = plusButton.spriteState;
                spriteState.highlightedSprite = cantUsePlusSelected;
                plusButton.spriteState = spriteState;
                plusButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(128, 128, 128, 128);
            }
            if (AbilityManager.AbilityCurrentLevel[abilityName] > 0 && !ability.forever)
            {
                minusButton.image.sprite = canUseMinus;
                spriteState = minusButton.spriteState;
                spriteState.highlightedSprite = canUseMinusSelected;
                minusButton.spriteState = spriteState;
                minusButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            }
            else
            {
                minusButton.image.sprite = cantUseMinus;
                spriteState = minusButton.spriteState;
                spriteState.highlightedSprite = cantUseMinusSelected;
                minusButton.spriteState = spriteState;
                minusButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(128, 128, 128, 128);
            }
            if (AbilityManager.AbilityCanUseLevel[abilityName] < AbilityManager.AbilityCanBuyLevel[abilityName])
            {
                unlockButton.image.sprite = canUseUnlock;
                spriteState = unlockButton.spriteState;
                spriteState.highlightedSprite = canUseUnlockSelected;
                unlockButton.spriteState = spriteState;
                unlockButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(50, 50, 50, 255);
            }
            else
            {
                unlockButton.image.sprite = cantUseUnlock;
                spriteState = unlockButton.spriteState;
                spriteState.highlightedSprite = cantUseUnlockSelected;
                unlockButton.spriteState = spriteState;
                unlockButton.transform.GetChild(0).GetComponent<Text>().color = new Color32(128, 128, 128, 128);
            }
        }

        public static void setPlayerAbility(string abilityName, int abilityLevel)
        {
            try
            {
                GameManager.abilityShower.setAbility(abilityName, abilityLevel);
            }
            catch
            {

            }
            switch (abilityName)
            {
                case "暴擊":
                    if (abilityLevel <= 0)
                    {
                        PlayerManager.criticalRate = 0;
                    }
                    else if (abilityLevel <= 1)
                    {
                        PlayerManager.criticalRate = 5;
                    }
                    else
                    {
                        PlayerManager.criticalRate = 10;
                    }
                    break;
                case "不屈":
                    if (abilityLevel <= 0)
                    {
                        PlayerManager.reducesDamage = 0;
                    }
                    else
                    {
                        PlayerManager.reducesDamage = 50;
                    }
                    break;
                case "光炸":
                    CrystalScattering.scatteringLightCount = 6 + abilityLevel * 2;
                    break;
                case "強光":
                    CrystalScatteringLight.Large = (abilityLevel == 1);
                    break;
                case "守護":
                    PlayerManager.MaxHP = 60 + abilityLevel * 20;
                    break;
                case "突進":
                    PlayerManager.DashSpeed = 11 + abilityLevel * 4;
                    break;
                case "低耗":
                    PlayerManager.DashCD = 0.5f - abilityLevel * 0.1f;
                    break;
                case "疾行":
                    PlayerManager.moveSpeed = 3 + abilityLevel;
                    break;
                case "傳送":
                    if (abilityLevel == 0)
                    {
                        PlayerManager.homeButton = false;
                        PlayerManager.homeButtonTimer = 12;
                        PlayerManager.homeButtonTimerStoper = 12;
                    }
                    else if (abilityLevel == 1)
                    {
                        PlayerManager.homeButton = true;
                        PlayerManager.homeButtonTimer = 12;
                        PlayerManager.homeButtonTimerStoper = 12;
                    }
                    else
                    {
                        PlayerManager.homeButton = true;
                        PlayerManager.homeButtonTimer = 6;
                        PlayerManager.homeButtonTimerStoper = 6;
                    }
                    break;
                case "磁場":
                    PlayerManager.magneticField = (abilityLevel != 0);
                    break;
                case "濺射":
                    PlayerManager.circleAttack = (abilityLevel != 0);
                    break;
                case "根性":
                    string sceneName = SceneManager.GetActiveScene().name;
                    if (abilityLevel <= 0)
                    {
                        PlayerManager.root = false;
                        if (sceneName == "Game 0" || sceneName == "Game 1" || sceneName.Contains("Game 0_"))
                        {
                            GameObject.Find("Directional Light 2D").GetComponent<Light2D>().intensity = 1.2f;
                        }
                    }
                    else
                    {
                        PlayerManager.root = true;
                        if (sceneName == "Game 0" || sceneName == "Game 1" || sceneName.Contains("Game 0_"))
                        {
                            GameObject.Find("Directional Light 2D").GetComponent<Light2D>().intensity = 1.2f;
                        }
                    }
                    break;
                case "光鏢":
                    PlayerManager.trackBullet = (abilityLevel != 0);
                    break;
                case "免疫":
                    PlayerManager.immunity = (abilityLevel != 0);
                    break;
                default:
                    Debug.LogError("沒有這個能力 : " + abilityName);
                    break;
            }
        }
    }
}