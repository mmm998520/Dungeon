using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

namespace com.DungeonPad
{
    public class PlayerJoyVibration : MonoBehaviour
    {
        public float weight = 1;
        public PlayerIndex playerIndex;
        PlayerManager playerManager;
        public PlayerJoyVibration otherPlayerJoyVibration;
        public float HurtVibration_Main, HurtVibration_notMain, ConfusionVibration, DashVibration;
        public static float LowHPVibration;

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
            CountHurtVibration();
            CountConfusionVibration();
            CountLowHPVibration();
            CountDashVibration();
            float maxer = Mathf.Max(HurtVibration_Main, HurtVibration_notMain, ConfusionVibration, LowHPVibration, DashVibration);
            GamePad.SetVibration(playerIndex, maxer * weight, maxer * weight);
            if (PlayerManager.HP <= 0)
            {
                GamePad.SetVibration(playerIndex, 0, 0);
            }
        }

        void CountHurtVibration()
        {
            if ((HurtVibration_Main -= Time.deltaTime) < 0)
            {
                HurtVibration_Main = 0;
            }
            if ((HurtVibration_notMain -= Time.deltaTime * 2 / 3) < 0)
            {
                HurtVibration_notMain = 0;
            }
        }

        void CountConfusionVibration()
        {
            if (playerManager.ConfusionTimer < 10)
            {
                ConfusionVibration += Time.deltaTime;
            }
            else
            {
                ConfusionVibration -= Time.deltaTime;
            }
            ConfusionVibration = Mathf.Clamp(ConfusionVibration, 0f, 0.6f);
        }

        void CountLowHPVibration()
        {
            LowHPVibration = Mathf.Clamp(PlayerManager.HP / 30, 0, 1);
            LowHPVibration -= 1;
            LowHPVibration *= -0.6f;
        }

        void CountDashVibration()
        {
            DashVibration -= Time.deltaTime * 2.5f;
        }

        public void hurt()
        {
            HurtVibration_Main = 0.6f;
            otherPlayerJoyVibration.HurtVibration_notMain = 0.15f;
        }
    }
}