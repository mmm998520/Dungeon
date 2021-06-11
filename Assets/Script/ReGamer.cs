using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class ReGamer : MonoBehaviour
    {
        float timer;
        void Start()
        {
            ReadOnlyArray<Gamepad> gamepads = Gamepad.all;
            for(int i = 0; i < gamepads.Count; i++)
            {
                gamepads[i].SetMotorSpeeds(0, 0);
            }
        }

        void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (InputManager.anyEnter())
            {
                if (SceneManager.GetActiveScene().name == "DEMO Thanks")
                {
                    Application.OpenURL("https://www.facebook.com/tmd10glim");
                }
                else
                {
                    ReGame();
                    ReAbility();
                    SwitchScenePanel.NextScene = "Game 1";
                    GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                    ButtonSelect.OnClicked();
                }
            }
            else if (InputManager.anyExit())
            {
                ReGame();
                ReAbility();
                SwitchScenePanel.NextScene = "Home";
                GameObject.Find("SwitchScenePanel").GetComponent<Animator>().SetTrigger("Loading");
                ButtonSelect.OnClicked();
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
                GameManager.AbilityNum = 0;
            }
            catch
            {

            }
        }
    }
}