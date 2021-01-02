using System.Collections;
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
        public GameObject panel, RDButton, CRButton;
        int storeCanbuyNum = 3;
        public int appearRoomNum;

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
            Time.timeScale = 0;
            panel.SetActive(true);
            setCanBuyAbilitys();
            randomAbility();
            setPrice();
            showOnStore();
        }

        public void initialStore()
        {
            Time.timeScale = 0;
            panel.SetActive(true);
            storeAbility.Clear();
            storeAbility.Add("光儲存上限增(40→60)");
            storeAbility.Add("殺怪回血10");
            storeAbility.Add("原地復活光球+1");
            storeAbilityPrice.Clear();
            storeAbilityPrice.Add(0);
            storeAbilityPrice.Add(0);
            storeAbilityPrice.Add(0);
            showOnStore();
        }

        public void closeStore()
        {
            Time.timeScale = 1;
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
                abilityGrowUp(storeAbility[ButtonNum]);
                storeAbility[ButtonNum] = "null";
                storeAbilityPrice[ButtonNum] = 0;
                storeAbilityText[ButtonNum].text = "null";
                storeAbilityPriceText[ButtonNum].text = "null";
            }
        }

        public void reducesDamageButton()
        {
            if(true/* && PlayerManager.money - 1 > 0*/)
            {
                PlayerManager.reducesDamage += 0.5f;
                PlayerManager.money--;
                if (PlayerManager.reducesDamage >= 50)
                {
                    RDButton.SetActive(false);
                }
            }
        }
        public void criticalRateButton()
        {
            if (true/* && PlayerManager.money - 1 > 0*/)
            {
                PlayerManager.criticalRate += 0.5f;
                PlayerManager.money--;
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
                case "光儲存上限增(40→60)":
                    PlayerManager.MaxHP = 60;
                    break;
                case "光儲存上限增(60→80)":
                    PlayerManager.MaxHP = 80;
                    break;
                case "殺怪回血10":
                    PlayerAttackLineUnit.hpRecover = 10;
                    break;
                case "殺怪回血25":
                    PlayerAttackLineUnit.hpRecover = 25;
                    break;
                case "原地復活光球+1":
                    if (PlayerManager.Life < PlayerManager.MaxLife)
                    {
                        PlayerManager.Life++;
                    }
                    break;
                case "原地復活上限+1(同時送1顆)":
                    if (PlayerManager.MaxLife <= 5)
                    {
                        PlayerManager.MaxLife++;
                    }
                    if (PlayerManager.Life < PlayerManager.MaxLife)
                    {
                        PlayerManager.Life++;
                    }
                    break;
                case "衝刺距離增加 4變5.5":
                    PlayerManager.DashSpeed = 15;
                    break;
                case "衝刺冷卻時間降低0.1":
                    PlayerManager.DashCD -= 0.1f;
                    break;
                case "一般移動速度加快":
                    PlayerManager.moveSpeed ++;
                    break;
                case "瞬移回夥伴身邊(冷卻10秒)":
                    //直接在PlayerManager寫能力了，這邊不用再寫
                    break;
            }
        }
    }
}