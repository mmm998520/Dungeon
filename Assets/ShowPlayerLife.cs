using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ShowPlayerLife : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            for(int i = 0; i < PlayerManager.MaxLife; i++)
            {
                transform.GetChild(i).gameObject.SetActive(i < PlayerManager.Life);
            }
        }
    }
}