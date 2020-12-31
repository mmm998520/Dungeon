﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class AbilityStore : MonoBehaviour
    {
        List<string> storeAbility = new List<string>();
        List<string> canBuys;
        List<int> storeAbilityPrice = new List<int>();
        public Text[] storeAbilityText;
        public Text[] storeAbilityPriceText;
        public GameObject panel;
        int storeCanbuyNum = 3;
        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                showStore();
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                closeStore();
            }
        }

        void showStore()
        {
            panel.SetActive(true);
            setCanBuyAbilitys();
            randomAbility();
            setPrice();
            showOnStore();
        }

        void closeStore()
        {
            panel.SetActive(false);
        }

        void setCanBuyAbilitys()
        {
            canBuys = new List<string>(AbilityManager.allAbility.Keys);
            foreach (string ability in AbilityManager.allAbility.Keys)
            {
                if (AbilityManager.myAbilitys.FindAll(x => x.Equals(ability)).Count >= AbilityManager.allAbility[ability])
                {
                    canBuys.Remove(ability);
                }
            }
            canBuys = canBuyRemoveCantHave(canBuys, "光儲存上限增(40→60)", "光儲存上限增(60→80)");
            canBuys = canBuyRemoveCantHave(canBuys, "殺怪回血10", "殺怪回血25");
        }

        List<string> canBuyRemoveCantHave(List<string> canBuys, string first, string next)
        {
            if (!AbilityManager.myAbilitys.Contains(first))
            {
                canBuys.Remove(next);
            }
            return canBuys;
        }

        void randomAbility()
        {
            storeAbility.Clear();
            for (int i = 0; i < storeCanbuyNum; i++)
            {
                int r = Random.Range(0, canBuys.Count);
                storeAbility.Add(canBuys[r]);
                canBuys.RemoveAt(r);
                if (canBuys.Count <= 0)
                {
                    Debug.LogError("沒東西可以賣了");
                    break;
                }
            }
        }

        void setPrice()
        {
            storeAbilityPrice.Clear();
            for (int i = 0; i < storeCanbuyNum; i++)
            {
                if (i < storeAbility.Count)
                {
                    switch (storeAbility[i])
                    {
                        case "光儲存上限增(40→60)":
                            storeAbilityPrice.Add(Random.Range(20, 21));
                            break;
                        case "光儲存上限增(60→80)":
                            storeAbilityPrice.Add(Random.Range(25, 26));
                            break;
                        case "殺怪回血10":
                            storeAbilityPrice.Add(Random.Range(20, 21));
                            break;
                        case "殺怪回血25":
                            storeAbilityPrice.Add(Random.Range(25, 26));
                            break;
                        case "原地復活光球+1":
                            storeAbilityPrice.Add(Random.Range(20, 21));
                            break;
                        case "原地復活上限+1(同時送1顆)":
                            storeAbilityPrice.Add(Random.Range(35, 36));
                            break;
                        case "衝刺距離增加 4變5.5":
                            storeAbilityPrice.Add(Random.Range(30, 41));
                            break;
                        case "衝刺冷卻時間降低0.1":
                            storeAbilityPrice.Add(Random.Range(30, 41));
                            break;
                        case "一般移動速度加快":
                            storeAbilityPrice.Add(Random.Range(30, 41));
                            break;
                        case "瞬移回夥伴身邊(冷卻10秒)":
                            storeAbilityPrice.Add(Random.Range(35, 46));
                            break;
                    }
                }
                else
                {
                    storeAbilityPrice.Add(Random.Range(0, 1));
                }
            }
        }

        void showOnStore()
        {
            for (int i = 0; i < storeCanbuyNum; i++)
            {
                if (i < storeAbility.Count)
                {
                    storeAbilityText[i].text = storeAbility[i];
                    storeAbilityPriceText[i].text = "" + storeAbilityPrice[i];
                }
                else
                {
                    storeAbilityText[i].text = "null";
                    storeAbilityPriceText[i].text = "null";
                }
            }
        }

        public void seletAbilityButton(int ButtonNum)
        {
            if (storeAbilityText[ButtonNum].text != "null"/* && PlayerManager.money - storeAbilityPrice[ButtonNum] > 0*/)
            {
                AbilityManager.myAbilitys.Add(storeAbility[ButtonNum]);
                PlayerManager.money -= storeAbilityPrice[ButtonNum];
                storeAbility[ButtonNum] = "null";
                storeAbilityPrice[ButtonNum] = 0;
                storeAbilityText[ButtonNum].text = "null";
                storeAbilityPriceText[ButtonNum].text = "null";
            }
        }
    }
}