using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class AbilityDatas : MonoBehaviour
    {
        public GameObject firstSelected, BuyDetail;
        public Image[] TotalCosts;
        public Sprite Loaded, UnLoad;

        private void Start()
        {
            start();
        }

        public void start()
        {
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }

        void Update()
        {
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
        }
    }
}