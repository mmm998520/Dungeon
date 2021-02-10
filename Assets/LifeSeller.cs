using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class LifeSeller : MonoBehaviour
    {
        public int price;
        public int times;

        public List<Transform> brightOrder;
        Color gray = new Color(0.3f, 0.3f, 0.3f, 1);
        private void Update()
        {

        }
        public void buy()
        {
            if (PlayerManager.money >= 5 && PlayerManager.Life < PlayerManager.MaxLife)
            {
                for(int i = 0; i < brightOrder[times].childCount; i++)
                {
                    brightOrder[times].GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
                }
                times++;
                PlayerManager.money -= 5;
                if (times >= price / 5)
                {
                    times = 0;
                    PlayerManager.Life += 1;
                    for(int i = 0; i < brightOrder.Count; i++)
                    {
                        for(int j = 0; j < brightOrder[i].childCount; j++)
                        {
                            brightOrder[i].GetChild(j).GetComponent<SpriteRenderer>().color = gray;
                        }
                    }
                }
            }
        }
    }
}