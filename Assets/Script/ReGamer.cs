﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ReGamer : MonoBehaviour
    {
        float timer;
        void Start()
        {

        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame || keyboard.allKeys[InputManager.p1KeyboardDashNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardDashNum].wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame))
            {
                ReGame();
                ReAbility();
                SwitchScenePanel.NextScene = "Game 1";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
            else if (keyboard.escapeKey.wasPressedThisFrame || keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame || keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame || (Gamepad.current != null && Gamepad.current.bButton.wasPressedThisFrame))
            {
                ReGame();
                ReAbility();
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
        }

        public static void ReGame()
        {
            Sensor.sensors = new List<GameObject>();
            Sensor.Exit = null;
            Sensor.exitSensor = null;
            poisonFog.playerList = new HashSet<PlayerManager>();
            PlayerManager.HP = PlayerManager.MaxHP;
            PlayerManager.Life = 2;
            PlayerManager.timerRecord = new List<float>();
            PlayerManager.recoveryRecord = new List<float>();
            PlayerJoyVibration.canVibration = true;
            GameManager.layers = 1;
            GameManager.level = 1;
            GameManager.DiedBecause = "Distance";
            ButtonOne.useButtonNum = 0;
            ButtonTwo.redUsed = false;
            ButtonTwo.blueUsed = false;
            DataSaver.Save();
            FireRainInser.insPoses.Clear();
            CrystalRainInser.insPoses.Clear();
        }

        public static void ReAbility()
        {
            try
            {
                for (int i = 0; i < AbilityManager.Abilitys.Length; i++)
                {
                    AbilityManager.AbilityCurrentLevel[AbilityManager.Abilitys[i].name] = 0;
                    AbilityData.setPlayerAbility(AbilityManager.Abilitys[i].name, 0);
                }
                AbilityShower.abilityNamesAndLevels.Clear();
            }
            catch
            {

            }
        }
    }
}