using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class LifeSeller : MonoBehaviour
    {
        public int price;
        public int times;
        private void Update()
        {

        }
        public void buy()
        {
            if (PlayerManager.money >= 5 && PlayerManager.Life < PlayerManager.MaxLife)
            {
                times++;
                PlayerManager.money -= 5;
                if (times >= price / 5)
                {
                    times = 0;
                    PlayerManager.Life += 1;
                }
            }
        }
    }
}