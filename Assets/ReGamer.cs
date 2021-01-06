using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ReGame();
            }
        }

        void ReGame()
        {
            Sensor.sensors = new List<GameObject>();
            Sensor.Exit = null;
            Sensor.exitSensor = null;
            poisonFog.playerList = new HashSet<PlayerManager>();
            PlayerManager.MaxHP = 40;
            PlayerManager.HP = 40;
            PlayerManager.Life = 2;
            PlayerManager.MaxLife = 2;
            PlayerManager.moveSpeed = 3;
            PlayerManager.DashSpeed = 11;
            PlayerManager.DashCD = 0.5f;
            PlayerManager.reducesDamage = 0f;
            PlayerManager.criticalRate = 0;
            PlayerManager.timerRecord = new List<float>();
            PlayerManager.recoveryRecord = new List<float>();
            PlayerJoyVibration.canVibration = true;
            PlayerAttackLineUnit.hpRecover = 0;
            GameManager.layers = 1;
            GameManager.level = 1;
            GameManager.DiedBecause = "Distance";
            ButtonOne.useButtonNum = 0;
            ButtonTwo.p1used = false;
            ButtonTwo.p2used = false;



            PlayerManager.money /= 2;
            AbilityManager.myAbilitys.Clear();
            SceneManager.LoadScene("Game 1");
        }
    }
}

