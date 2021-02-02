using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class LifeSeller : MonoBehaviour
    {
        public int price;

        private void Update()
        {
            for(int i = 0; i < GameManager.players.childCount; i++)
            {
                if (Vector2.Distance(GameManager.players.GetChild(i).position, transform.position) < 1.5f)
                {
                    if (Input.GetKeyDown(KeyCode.F1))
                    {
                        if (PlayerManager.money >= price && PlayerManager.Life < PlayerManager.MaxLife)
                        {
                            PlayerManager.money -= price;
                            PlayerManager.Life += 1;
                            break;
                        }
                    }
                }
            }
        }
    }
}