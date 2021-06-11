using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace com.DungeonPad
{
    public class GameManager : MonoBehaviour
    {
        public const bool staff = false, DEMO = false;
        public static string language = "_CH";

        public static GameManager gameManager;
        public static Transform players, monsters, triggers, UI, magneticFields;
        public static int layers = 1, level = 1, passLayerOneTimes = 0, passLayerThreeTimes = 0, layerOneCntinuousDideTimes = 0, layerThreeCntinuousDideTimes = 0, layerFourCntinuousWinTimes = 0;
        public static MazeCreater mazeCreater;
        public static SmallMap smallMap;
        public static float Gammar = 1;

        public static float DiedBecauseTimer;
        public static string DiedBecause = "Distance";
        public static float PlayTime;
        public static int KillSpider, KillSlime;
        public static int P1SpiderShooted, P1SpiderHit, P1SlimeHit, P1BubbleTimes;
        public static int P2SpiderShooted, P2SpiderHit, P2SlimeHit, P2BubbleTimes;

        float emptyRoomNumCountTimer;

        public GameObject reLifeParticle, money, moneyB;
        public bool haveFinalRoomStore;
        public static GameObject Hurted;
        public static ShowAbilityDetail showAbilityDetail;
        public static AbilityShower abilityShower;
        public static string CurrentSceneName;
        public static GameObject shopPanel, stopPanel, settingPanel;

        public static int AbilityNum;
        void Awake()
        {
            ReLifeParticle.Tracking = false;
            Debug.LogError("passLayerOneTimes : " + passLayerOneTimes + ", passLayerThreeTimes : " + passLayerThreeTimes);
            CurrentSceneName = SceneManager.GetActiveScene().name;
            MazeCreater.setTotalRowCol();
            gameManager = this;
            players = GameObject.Find("Players").transform;
            monsters = GameObject.Find("Monsters").transform;
            triggers = GameObject.Find("Triggers").transform;
            magneticFields = GameObject.Find("MagneticFields").transform;
            try
            {
                abilityShower = GameObject.Find("AbilityShower").GetComponent<AbilityShower>();
            }
            catch
            {

            }
            if (GameObject.Find("MazeCreater"))
            {
                mazeCreater = GameObject.Find("MazeCreater").GetComponent<MazeCreater>();
            }
            if (GameObject.Find("SmallMap"))
            {
                smallMap = GameObject.Find("SmallMap").GetComponent<SmallMap>();
                smallMap.start();
                UI = smallMap.transform.parent;
            }
            if (CurrentSceneName == "Game 0")
            {
                PlayerManager.Life = 1;
            }
            Hurted = Resources.Load<GameObject>("Prefabs/Hurted");
            try
            {
                showAbilityDetail = GameObject.Find("能力敘述文字").GetComponent<ShowAbilityDetail>();
            }
            catch
            {
                
            }
            if (CurrentSceneName.Contains("Select"))
            {
                ReGamer.ReGame();
            }
            shopPanel = GameObject.Find("shop").transform.GetChild(0).gameObject;
            try
            {
                stopPanel = GameObject.Find("stop").transform.GetChild(0).gameObject;
            }
            catch
            {
                stopPanel = GameObject.Find("setting").transform.GetChild(0).gameObject;
                Debug.LogError(CurrentSceneName + "的stopPanal要改名字喔!!!!!!!");
            }
            try
            {
                settingPanel = GameObject.Find("setting").transform.GetChild(0).gameObject;
            }
            catch
            {
                Debug.LogError(CurrentSceneName + "三小????!!!!!!!");
            }
            MonsterManager.genMoneyNum = 0;
        }

        void Update()
        {
            if (CurrentSceneName == "Game 1" || CurrentSceneName == "Game 2" || CurrentSceneName == "Game 4")
            {
                GameTimer.Timer += Time.deltaTime;
            }
            else
            {
                GameTimer.Timer = 0;
            }
            Keyboard keyboard = Keyboard.current;
            InputManager.currentGamepad = Gamepad.current;
            if (staff && keyboard.homeKey.wasPressedThisFrame)
            {
                shopPanel.SetActive(true);
                shopPanel.transform.GetChild(0).GetComponent<AbilityDatas>().start();
            }
            bool openStopPanel = false, closeSettingPanel = false;
            if (!settingPanel.activeSelf  && PlayerManager.HP0Timer < 0.2f && ((keyboard.escapeKey.wasPressedThisFrame || (InputManager.currentGamepad != null && InputManager.currentGamepad.startButton.wasPressedThisFrame))) && !SceneManager.GetActiveScene().name.Contains("Select"))
            {
                if (stopPanel.activeSelf)
                {
                    Time.timeScale = 1;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                }
                else if (settingPanel.activeSelf)
                {
                    settingPanel.SetActive(false);
                }
                else
                {
                    openStopPanel = true;
                    Time.timeScale = 0;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                }
                ButtonSelect.OnClicked();
                stopPanel.SetActive(!stopPanel.activeSelf);
                Debug.LogWarning("stopPanel.activeSelf : " + stopPanel.activeSelf);
            }
            if (settingPanel.activeSelf && InputManager.anyExit())
            {
                settingPanel.SetActive(false);
                stopPanel.SetActive(true);
                closeSettingPanel = true;
                ButtonSelect.OnClicked();
            }
            if (stopPanel.activeSelf && InputManager.anyExit() && !openStopPanel && !closeSettingPanel)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                stopPanel.SetActive(false);
                ButtonSelect.OnClicked();
            }

            //if (abilityStore)
            /*
            {
                if ((emptyRoomNumCountTimer += Time.deltaTime) >= 1)
                {
                    emptyRoomNumCountTimer = 0;
                    if((Sensor.WeAreInEmptyFinalRoom() && !haveFinalRoomStore))
                    {
                        haveFinalRoomStore = true;
                        Vector3 sellerPos;
                        for (int i = 0; i < 51; i++)
                        {
                            sellerPos = CameraManager.center + new Vector3(Random.Range(3, -3), Random.Range(3, -3), 10);
                            RaycastHit2D hit = Physics2D.BoxCast(sellerPos, Vector3.one, 0, Vector2.right, 0, 0 << 13);
                            if (!hit)
                            {
                                Instantiate(seller, sellerPos, Quaternion.identity);
                                break;
                            }
                            else
                            {
                                Debug.LogError("撞牆");
                            }
                            if (i >= 50)
                            {
                                Debug.LogError(sellerPos + hit.collider.name, hit.collider.gameObject);
                            }
                        }
                    }
                }
            }*/

            DiedBecauseTimer += Time.deltaTime;

            if (PlayerManager.HP <= 0)
            {
                if (GameObject.Find("lineAttacks"))
                {
                    Destroy(GameObject.Find("lineAttacks"));
                }
            }
        }

        public void OnApplicationPause(bool pause)
        {
            if (pause)
            {
                if (!(stopPanel.activeSelf || settingPanel.activeSelf) && !CurrentSceneName.Contains("Select") && !CurrentSceneName.Contains("Home"))
                {
                    Time.timeScale = 0;
                    Time.fixedDeltaTime = 0.02F * Time.timeScale;
                    stopPanel.SetActive(true);
                }
            }
        }
    }
}
