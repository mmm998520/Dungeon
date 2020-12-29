using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ShowMoney : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            GetComponent<Text>().text = "" + PlayerManager.money;
        }
    }
}