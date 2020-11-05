using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

namespace com.DungeonPad
{
    public class PlayerJoyVibration : MonoBehaviour
    {
        public float weight = 1;
        PlayerIndex playerIndex;
        PlayerManager playerManager;
        public PlayerJoyVibration otherPlayerJoyVibration;
        public float HurtVibration_Main, HurtVibration_notMain;
        float HurtTimer_Main, HurtTimer_notMain;
        float HurtTimerStoper_Main, HurtTimerStoper_notMain;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            if (playerManager.p1)
            {
                playerIndex = SelectRole.P1PlayerIndex;
            }
            else
            {
                playerIndex = SelectRole.P2PlayerIndex;
            }
        }

        void Update()
        {
            if ((HurtVibration_Main -= Time.deltaTime) < 0)
            {
                HurtVibration_Main = 0;
            }
            if ((HurtVibration_notMain -= Time.deltaTime*2/3) < 0)
            {
                HurtVibration_notMain = 0;
            }
            float maxer = Mathf.Max(HurtVibration_Main, HurtVibration_notMain);
            GamePad.SetVibration(playerIndex, maxer * weight, maxer * weight);
        }

        public void hurt()
        {
            HurtVibration_Main = 0.1f;
            otherPlayerJoyVibration.HurtVibration_notMain = 0.2f;
        }
    }
}