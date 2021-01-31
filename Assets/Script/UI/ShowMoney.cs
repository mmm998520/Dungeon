using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowMoney : MonoBehaviour
    {
        public bool moneyB;
        void Start()
        {

        }

        void Update()
        {
            if (!moneyB)
            {
                GetComponent<Text>().text = "" + PlayerManager.money;
            }
            else
            {
                GetComponent<Text>().text = "" + PlayerManager.moneyB;
            }
        }
    }
}