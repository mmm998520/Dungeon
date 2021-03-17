using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class AbilityDatas : MonoBehaviour
    {
        public GameObject firstSelected, BuyDetail, selectMouse;
        public Image[] TotalCosts;
        public Sprite Loaded, UnLoad;
        float AbilityTop, AbilityBottom, dis;

        private void Start()
        {
            AbilityTop = transform.GetChild(0).GetComponent<RectTransform>().position.y;
            AbilityBottom = transform.GetChild(9).GetComponent<RectTransform>().position.y;
            dis = (AbilityTop - AbilityBottom) / 9;
        }

        public void start()
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
            AbilityShower.showSelected = false;
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if(EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().position.y > AbilityTop)
                {
                    for(int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<RectTransform>().position += Vector3.down * dis;
                    }
                }
                if (EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>().position.y < AbilityBottom)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).GetComponent<RectTransform>().position += Vector3.up * dis;
                    }
                }
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
            if (keyboard.escapeKey.wasPressedThisFrame || keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame || Gamepad.current.bButton.wasPressedThisFrame)
            {
                transform.parent.gameObject.SetActive(false);
                if (selectMouse)
                {
                    selectMouse.SetActive(true);
                }
                GameManager.players.gameObject.SetActive(true);
            }
            else
            {
                transform.parent.gameObject.SetActive(true);
                if (selectMouse)
                {
                    selectMouse.SetActive(false);
                }
                GameManager.players.gameObject.SetActive(false);
            }
            if (keyboard.f1Key.wasPressedThisFrame)
            {
                PlayerManager.money++;
            }
            if (keyboard.f2Key.wasPressedThisFrame)
            {
                PlayerManager.money--;
            }
            if (keyboard.f3Key.wasPressedThisFrame)
            {
                PlayerManager.moneyB++;
            }
            if (keyboard.f4Key.wasPressedThisFrame)
            {
                PlayerManager.moneyB--;
            }
            if (keyboard.f5Key.wasPressedThisFrame)
            {
                PlayerManager.money+=10;
            }
            if (keyboard.f6Key.wasPressedThisFrame)
            {
                PlayerManager.money-= 10;
            }
            if (keyboard.f7Key.wasPressedThisFrame)
            {
                PlayerPrefs.DeleteAll();
            }
        }
    }
}