using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ShowPlayerLife : MonoBehaviour
    {
        public Transform MaxLifes, Lifes;
        void Start()
        {

        }

        void Update()
        {
            for(int i = 0; i < 5; i++)
            {
                MaxLifes.GetChild(i).gameObject.SetActive(i < PlayerManager.MaxLife);
            }
            for (int i = 0; i < PlayerManager.MaxLife; i++)
            {
                Lifes.GetChild(i).gameObject.SetActive(i < PlayerManager.Life);
            }
        }
    }
}