using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class AlarmWordSetting : MonoBehaviour
    {
        public GameObject ConfusionText, StickText, ConfusionAndStickText;
        [SerializeField] Image[] ShowPressBs;

        void Start()
        {

        }

        void Update()
        {
            PlayerManager playerManager;
            bool Confusion = false, Stick = false;
            for (int i = 0; i < GameManager.players.childCount; i++)
            {
                bool showPressB = false;
                playerManager = GameManager.players.GetChild(i).GetComponent<PlayerManager>();
                if(playerManager.ConfusionTimer < 10)
                {
                    Confusion = true;
                    showPressB = true;
                }
                if (playerManager.StickTimer < 10)
                {
                    Stick = true;
                    showPressB = true;
                }
                if (ShowPressBs.Length > i)
                {
                    ShowPressBs[i].enabled = showPressB;
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