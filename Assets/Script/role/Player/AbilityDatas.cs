using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class AbilityDatas : MonoBehaviour
    {
        public GameObject firstSelected, BuyDetail, selectMouse;
        public Image[] TotalCosts;
        public Sprite Loaded, UnLoad;

        private void Start()
        {
            
        }

        public void start()
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
            EventSystem.current.currentSelectedGameObject.transform.parent.GetComponent<AbilityData>().setDetail();
            BuyDetail.SetActive(EventSystem.current.currentSelectedGameObject.name == "解鎖");
            for(int i = 0; i < TotalCosts.Length; i++)
            {
                TotalCosts[i].enabled = i < AbilityManager.TotalCost;
                if (i < AbilityManager.Costed)
                {
                    TotalCosts[i].sprite = Loaded;
                }
                else
                {
                    TotalCosts[i].sprite = UnLoad;
                }
            }
            if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                transform.parent.gameObject.SetActive(false);
                selectMouse.SetActive(true);
                GameManager.players.gameObject.SetActive(true);
            }
            else
            {
                transform.parent.gameObject.SetActive(true);
                selectMouse.SetActive(false);
                GameManager.players.gameObject.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.F1))
            {
                PlayerManager.money++;
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                PlayerManager.money--;
            }
            if (Input.GetKeyDown(KeyCode.F3))
            {
                PlayerManager.moneyB++;
            }
            if (Input.GetKeyDown(KeyCode.F4))
            {
                PlayerManager.moneyB--;
            }
            if (Input.GetKeyDown(KeyCode.F5))
            {
                PlayerManager.money+=10;
            }
            if (Input.GetKeyDown(KeyCode.F6))
            {
                PlayerManager.money-= 10;
            }
            if (Input.GetKeyDown(KeyCode.F12))
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}