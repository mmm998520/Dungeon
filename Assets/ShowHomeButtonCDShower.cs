using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class ShowHomeButtonCDShower : MonoBehaviour
    {
        public GameObject[] ShowHomeButtonCDs;
        void Update()
        {
            for(int i = 0; i < ShowHomeButtonCDs.Length; i++)
            {
                ShowHomeButtonCDs[i].SetActive(PlayerManager.homeButton);
            }
        }
    }
}
