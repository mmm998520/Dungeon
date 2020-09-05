using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class UIManager : MonoBehaviour
    {
        PlayerManager[] players = new PlayerManager[4];
        void Start()
        {
            for (int i = 0; i < transform.GetChild(0).childCount / 2; i++)
            {
                players[i] = GameManager.Players.GetChild(i).GetComponent<PlayerManager>();
            }
        }

        void Update()
        {
            for(int i= 0;i< transform.GetChild(0).childCount / 2; i++)
            {
                if (players[i] != null)
                {
                    transform.GetChild(0).GetChild(i).localScale = new Vector3((players[i].HP[(int)players[i].career, players[i].level] - players[i].Hurt) / players[i].HP[(int)players[i].career, players[i].level], transform.localScale.y, transform.localScale.z);
                    transform.GetChild(1).GetChild(i).localScale = new Vector3(players[i].exp / PlayerManager.expToNextLevel[players[i].level], transform.localScale.y, transform.localScale.z);
                    if (transform.GetChild(0).GetChild(i).localScale.x < 0)
                    {
                        transform.GetChild(0).GetChild(i).localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
                    }
                    for(int j = 0; j < transform.GetChild(2).GetChild(i).childCount; j++)
                    {
                        if (players[i].level > j)
                        {
                            transform.GetChild(2).GetChild(i).GetChild(j).gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    Debug.LogError("你已經死了 ! ");
                }
            }
        }
    }
}