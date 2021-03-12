using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ReGame();
                ReAbility();
                SwitchScenePanel.NextScene = "Game 1";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
            }
            else if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.JoystickButton1))
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
            ButtonTwo.p1used = false;
            ButtonTwo.p2used = false;
            DataSaver.Save();
        }

        public static void ReAbility()
        {
            for(int i=0;i< AbilityManager.Abilitys.Length; i++)
            {
                AbilityManager.AbilityCurrentLevel[AbilityManager.Abilitys[i].name] = 0;
                AbilityData.setPlayerAbility(AbilityManager.Abilitys[i].name, 0);
            }
            AbilityShower.abilityNamesAndLevels.Clear();
        }
    }
}