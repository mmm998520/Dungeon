using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace com.DungeonPad
{
    public class PlayerJoyVibration : MonoBehaviour
    {
        public float weight = 1;
        PlayerManager playerManager;
        public PlayerJoyVibration otherPlayerJoyVibration;
        public float HurtVibration_Main, HurtVibration_notMain, StickVibration, ConfusionVibration, DashVibration;
        public static float LowHPVibration;

        public static bool canVibration = true;

        Gamepad gamepad;
        void OnEnable()
        {
            playerManager = GetComponent<PlayerManager>();
            if (playerManager.p1)
            {
                if (InputManager.p1Gamepad != null)
                {
                    gamepad = InputManager.p1Gamepad;
                }
                else
                {
                    this.enabled = false;
                }
            }
            else
            {
                if (InputManager.p2Gamepad != null)
                {
                    gamepad = InputManager.p2Gamepad;
                }
                else
                {
                    this.enabled = false;
                }
            }
        }

        void Update()
        {
            if (canVibration)
            {
                CountHurtVibration();
                CountStickVibration();
                CountConfusionVibration();
                //CountLowHPVibration();
                CountDashVibration();
                float maxer = Mathf.Max(HurtVibration_Main, HurtVibration_notMain, StickVibration, ConfusionVibration, LowHPVibration, DashVibration);
                if(gamepad != null)
                {
                    gamepad.SetMotorSpeeds(maxer * weight / 4, maxer * weight);
                }
                else
                {
                    playerManager = GetComponent<PlayerManager>();
                    if (playerManager.p1)
                    {
                        if (InputManager.p1Gamepad != null)
                        {
                            gamepad = InputManager.p1Gamepad;
                        }
                        else
                        {
                            this.enabled = false;
                        }
                    }
                    else
                    {
                        if (InputManager.p2Gamepad != null)
                        {
                            gamepad = InputManager.p2Gamepad;
                        }
                        else
                        {
                            this.enabled = false;
                        }
                    }
                }
                if (PlayerManager.HP <= 0)
                {
                    gamepad.SetMotorSpeeds(0, 0);
                }
            }
            else
            {
                gamepad.SetMotorSpeeds(0, 0);
                HurtVibration_Main = 0;
                HurtVibration_notMain = 0;
                StickVibration = 0;
                ConfusionVibration = 0;
                LowHPVibration = 0;
                DashVibration = 0;
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

        void CountStickVibration()
        {
            if (playerManager.StickTimer < 10)
            {
                StickVibration += Time.deltaTime;
            }
            else
            {
                StickVibration -= Time.deltaTime;
            }
            StickVibration = Mathf.Clamp(StickVibration, 0f, 0.3f);
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
            ConfusionVibration = Mathf.Clamp(ConfusionVibration, 0f, 0.3f);
        }

        void CountLowHPVibration()
        {
            LowHPVibration = Mathf.Clamp(PlayerManager.HP / 30, 0, 1);
            LowHPVibration -= 1;
            LowHPVibration *= -0.2f;

            LowHPVibration += Mathf.Clamp(PlayerManager.countAverage(PlayerManager.recoveryRecord) * -2, 0, 0.4f) / 2;
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