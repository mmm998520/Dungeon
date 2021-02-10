using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class AlarmWordSetting : MonoBehaviour
    {
        public GameObject ConfusionText, StickText, ConfusionAndStickText;

        void Start()
        {

        }

        void Update()
        {
            PlayerManager playerManager;
            bool Confusion = false, Stick = false;
            for (int i = 0; i < GameManager.players.childCount; i++)
            {
                playerManager = GameManager.players.GetChild(i).GetComponent<PlayerManager>();
                if(playerManager.ConfusionTimer < 10)
                {
                    Confusion = true;
                }
                if (playerManager.StickTimer < 10)
                {
                    Stick = true;
                }
            }
            if(Confusion && Stick)
            {
                ConfusionText.SetActive(false);
                StickText.SetActive(false);
                ConfusionAndStickText.SetActive(true);
            }
            else
            {
                ConfusionText.SetActive(Confusion);
                StickText.SetActive(Stick);
                ConfusionAndStickText.SetActive(false);
            }
        }
    }
}