﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        string abilityName;

        private void Awake()
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
                AbilityManager.Costed += ability.cost[AbilityManager.AbilityCurrentLevel[abilityName]];
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
                AbilityManager.Costed -= ability.cost[AbilityManager.AbilityCurrentLevel[abilityName]];
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
                if (moneyA && moneyB)
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

        public void setPlayerAbility(string abilityName, int abilityLevel)
        {
            switch (abilityName)
            {
                case "爆擊率":
                    PlayerManager.criticalRate = abilityLevel * 0.5f;
                    break;
                case "傷害減輕":
                    PlayerManager.reducesDamage = abilityLevel * 0.5f;
                    break;
                case "復活上限":
                    PlayerManager.MaxLife = 4 + abilityLevel;
                    break;
                case "負載上限":
                    AbilityManager.TotalCost = 2 + abilityLevel;
                    break;
                case "血量上限增加":
                    PlayerManager.MaxHP = 40 + abilityLevel * 20;
                    break;
                case "吸收":
                    if(abilityLevel == 0)
                    {
                        PlayerManager.killHpRecover = 0;
                    }
                    else if (abilityLevel == 1)
                    {
                        PlayerManager.killHpRecover = 10;
                    }
                    else
                    {
                        PlayerManager.killHpRecover = 25;
                    }
                    break;
                case "強力衝刺":
                    PlayerManager.DashSpeed = 11 + abilityLevel * 4;
                    break;
                case "連續衝刺":
                    PlayerManager.DashCD = 0.5f - abilityLevel * 0.1f;
                    break;
                case "疾行":
                    PlayerManager.moveSpeed = 3 + abilityLevel;
                    break;
                case "召回":
                    if (abilityLevel == 0)
                    {
                        PlayerManager.homeButton = false;
                        PlayerManager.homeButtonTimer = 12;
                    }
                    else if (abilityLevel == 1)
                    {
                        PlayerManager.homeButton = true;
                        PlayerManager.homeButtonTimer = 12;
                    }
                    else
                    {
                        PlayerManager.homeButton = true;
                        PlayerManager.homeButtonTimer = 6;
                    }
                    break;
                default:
                    Debug.LogError("沒有這個能力 : " + abilityName);
                    break;
            }
        }
    }
}