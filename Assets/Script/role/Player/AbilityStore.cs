using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class AbilityStore : MonoBehaviour
    {
        List<string> storeAbility = new List<string>();
        List<string> canBuys;
        List<int> storeAbilityPrice = new List<int>();
        public Text[] storeAbilityText, storeAbilityPriceText;
        public Text totalCanChooseNumText, RefreshTimesText, RefreshCostText, reducesDamageText, criticalRateText, onSellText;
        public GameObject panel, RDButton, CRButton;
        int storeCanbuyNum = 3, totalCanChooseNum, RefreshTimes, RefreshCost = 5;
        public int appearRoomNum;
        bool Refreshed = false;
        public GameObject firstSelectButton;

        private void Awake()
        {
            appearRoomNum = Random.Range(6, 10);
        }

        void Start()
        {
            if (PlayerManager.reducesDamage >= 50)
            {
                RDButton.SetActive(false);
            }
            if (PlayerManager.criticalRate >= 50)
            {
                CRButton.SetActive(false);
            }
        }

        public void showStore()
        {
            setStoreText(999, 2);
            Time.timeScale = 0;
            Refreshed = false;
            onSellText.enabled = !Refreshed;
            panel.SetActive(true);
            setCanBuyAbilitys();
            randomAbility();
            setPrice();
            showOnStore();
            PlayerJoyVibration.canVibration = false;
            resetEvenSystem();
        }

        public void initialStore()
        {
            setStoreText(2,0);
            Time.timeScale = 0;
            Refreshed = true;
            onSellText.enabled = !Refreshed;
            panel.SetActive(true);
            storeAbility.Clear();
            storeAbility.Add("血量上限增加 LV0→LV1");
            storeAbility.Add("殺怪回血 LV0→LV1");
            storeAbility.Add("復活光球+1");
            storeAbilityPrice.Clear();
            storeAbilityPrice.Add(0);
            storeAbilityPrice.Add(0);
            storeAbilityPrice.Add(0);
            showOnStore();
            PlayerJoyVibration.canVibration = false;
            resetEvenSystem();
        }

        public void RefreshStore()
        {
            if (RefreshTimes > 0 /*&& PlayerManager.money - RefreshCost >= 0*/)
            {
                PlayerManager.money -= RefreshCost;
                setStoreText(totalCanChooseNum, --RefreshTimes);
                Time.timeScale = 0;
                Refreshed = true;
                onSellText.enabled = !Refreshed;
                panel.SetActive(true);
                setCanBuyAbilitys();
                randomAbility();
                setPrice();
                showOnStore();
            }
        }

        public void closeStore()
        {
            Time.timeScale = 1;
            panel.SetActive(false);
            PlayerJoyVibration.canVibration = true;
        }

        void setStoreText(int _totalCanChooseNum, int _RefreshTimes, int _RefreshCost)
        {
            totalCanChooseNum = _totalCanChooseNum;
            RefreshTimes = _RefreshTimes;
            RefreshCost = _RefreshCost;
            if (totalCanChooseNum < 500)
            {
                totalCanChooseNumText.text = "還可選" + totalCanChooseNum + "個";
            }
            else
            {
                totalCanChooseNumText.text = "";
            }
            RefreshTimesText.text = "剩" + RefreshTimes + "次";
            RefreshCostText.text = RefreshCost + "元";
        }
        void setStoreText(int _totalCanChooseNum, int _RefreshTimes)
        {
            totalCanChooseNum = _totalCanChooseNum;
            RefreshTimes = _RefreshTimes;
            if (totalCanChooseNum < 500)
            {
                totalCanChooseNumText.text = "還可選" + totalCanChooseNum + "個";
            }
            else
            {
                totalCanChooseNumText.text = "";
            }
            RefreshTimesText.text = "剩" + RefreshTimes + "次";
            RefreshCostText.text = RefreshCost + "元";
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
            canBuys = canBuyRemoveCantHave(canBuys, "血量上限增加 LV0→LV1", "血量上限增加 LV1→LV2");
            canBuys = canBuyRemoveCantHave(canBuys, "殺怪回血 LV0→LV1", "殺怪回血 LV1→LV2");
            canBuys = canBuyRemoveCantHave(canBuys, "降低衝刺冷卻 LV0→LV1", "降低衝刺冷卻 LV1→LV2");
            canBuys = canBuyRemoveCantHave(canBuys, "加快移動 LV1→LV2", "加快移動 LV1→LV2");
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
            for (int i = 0; i < storeAbility.Count; i++)
            {
                if (Refreshed)
                {
                    switch (storeAbility[i])
                    {
                        case "血量上限增加 LV0→LV1":
                        case "殺怪回血 LV0→LV1":
                        case "復活光球+1":
                            storeAbilityPrice.Add(Random.Range(15, 16));
                            break;
                        default:
                            storeAbilityPrice.Add(Random.Range(30, 45));
                            break;
                    }
                }
                else
                {
                    if (i < storeAbility.Count - 1)
                    {
                        switch (storeAbility[i])
                        {
                            case "血量上限增加 LV0→LV1":
                            case "殺怪回血 LV0→LV1":
                            case "復活光球+1":
                                storeAbilityPrice.Add(Random.Range(15, 16));
                                break;
                            default:
                                storeAbilityPrice.Add(Random.Range(30, 46));
                                break;
                        }
                    }
                    else if (i < storeAbility.Count)
                    {
                        switch (storeAbility[i])
                        {
                            case "血量上限增加 LV0→LV1":
                            case "殺怪回血 LV0→LV1":
                            case "復活光球+1":
                                storeAbilityPrice.Add(Random.Range(15, 16));
                                break;
                            default:
                                storeAbilityPrice.Add(Random.Range(20, 21));
                                break;
                        }
                    }
                    else
                    {
                        storeAbilityPrice.Add(Random.Range(0, 1));
                    }
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
            if (storeAbilityText[ButtonNum].text != "null"/* && PlayerManager.money - storeAbilityPrice[ButtonNum] >= 0*/ && totalCanChooseNum > 0)
            {
                setStoreText(--totalCanChooseNum, RefreshTimes);
                AbilityManager.myAbilitys.Add(storeAbility[ButtonNum]);
                PlayerManager.money -= storeAbilityPrice[ButtonNum];
                abilityGrowUp(storeAbility[ButtonNum]);
                storeAbility[ButtonNum] = "null";
                storeAbilityPrice[ButtonNum] = 0;
                storeAbilityText[ButtonNum].text = "null";
                storeAbilityPriceText[ButtonNum].text = "null";
            }
        }

        public void reducesDamageButton()
        {
            if(true/* && PlayerManager.money - 1 >= 0*/)
            {
                PlayerManager.reducesDamage += 0.5f;
                PlayerManager.money--;
                reducesDamageText.text = "現在" + PlayerManager.reducesDamage + "%";
                if (PlayerManager.reducesDamage >= 50)
                {
                    RDButton.SetActive(false);
                }
            }
        }
        public void criticalRateButton()
        {
            if (true/* && PlayerManager.money - 1 >= 0*/)
            {
                PlayerManager.criticalRate += 0.5f;
                PlayerManager.money--;
                criticalRateText.text = "現在" + PlayerManager.criticalRate + "%";
                if (PlayerManager.criticalRate >= 50)
                {
                    CRButton.SetActive(false);
                }
            }
        }

        void abilityGrowUp(string ability)
        {
            switch (ability)
            {
                case "血量上限增加 LV0→LV1":
                    PlayerManager.MaxHP = 60;
                    break;
                case "血量上限增加 LV1→LV2":
                    PlayerManager.MaxHP = 80;
                    break;
                case "殺怪回血 LV0→LV1":
                    PlayerAttackLineUnit.hpRecover = 10;
                    break;
                case "殺怪回血 LV1→LV2":
                    PlayerAttackLineUnit.hpRecover = 25;
                    break;
                case "復活光球+1":
                    if (PlayerManager.Life < PlayerManager.MaxLife)
                    {
                        PlayerManager.Life++;
                    }
                    break;
                case "復活上限+1(送1顆復活光球)":
                    if (PlayerManager.MaxLife <= 5)
                    {
                        PlayerManager.MaxLife++;
                    }
                    if (PlayerManager.Life < PlayerManager.MaxLife)
                    {
                        PlayerManager.Life++;
                    }
                    break;
                case "衝刺距離增加":
                    PlayerManager.DashSpeed = 15;
                    break;
                case "降低衝刺冷卻 LV0→LV1":
                case "降低衝刺冷卻 LV1→LV2":
                    PlayerManager.DashCD -= 0.1f;
                    break;
                case "加快移動 LV0→LV1":
                case "加快移動 LV1→LV2":
                    PlayerManager.moveSpeed ++;
                    break;
                case "按X傳送到隊友身邊(冷卻10秒)":
                    //直接在PlayerManager寫能力了，這邊不用再寫
                    break;
                default:
                    Debug.LogError("商品名稱錯誤 : \""+ ability + "\"");
                    break;
            }
        }

        void resetEvenSystem()
        {
            EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            eventSystem.SetSelectedGameObject(firstSelectButton);
        }
    }
}