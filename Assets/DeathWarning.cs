using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.BoardGameDungeon
{
    public class DeathWarning : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {
            bool ShowIt = false;
            foreach(Transform player in GameManager.Players)
            {
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                if(playerManager.Hurt > playerManager.HP[(int)playerManager.career,playerManager.level] / 3 * 2)
                {
                    ShowIt = true;
                    break;
                }
            }
            GetComponent<Image>().enabled = ShowIt;
        }
    }
}