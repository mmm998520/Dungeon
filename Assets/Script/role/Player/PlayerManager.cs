using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;

namespace com.DungeonPad
{
    public class PlayerManager : MonoBehaviour
    {
        public static float MaxHP = 60, HP = 60;
        public static int Life = 2, MaxLife = 4;
        public static bool lockedHP = true;
        public static float lockedHPTimer = 10, DiedTimer = 10, HP0Timer = 0;
        public float ATK, hand, atkTime;
        public bool continued = false;
        public float CD, CDTimer;
        public static int killHpRecover = 0;
        public GameObject attack, regener, ridiculeWind;
        Vector3 lastPos;
        public bool locked = true, flash = false;
        public float beganTouchedTimer, flashTimer, flashTimerStoper;
        public static float homeButtonTimer = 0, homeButtonTimerStoper = 12;
        public static float moveSpeed = 3f, DashSpeed = 11, DashCD = 0.5f, reducesDamage = 0, criticalRate = 0;
        public static bool homeButton = false, magneticField = false, circleAttack = false, poison = false, trackBullet = false, immunity = false, root = false;
        public List<Vector3> startRayPoss;

        public float reTimer = 10;

        public static int batStickedNum = 0;

        public bool p1;
        public bool lastDirRight;
        public Vector2 v = Vector2.zero, HardStraightA = Vector2.zero, DashA = Vector2.zero;

        Transform otherPlayer;

        public PlayerJoyVibration playerJoyVibration;

        public float StickTimer = 10, HardStraightTimer = 10, DashTimer = 10, SleepTimer = 10;
        public float ConfusionTimer = 100;
        public SpriteRenderer ConfusionUIRenderer;
        public ConfusionUIcontroler ConfusionUIcontroler;

        public const float LightRangeMinSize = 3f, LightRangeMaxSize = 20;
        float lastUpdateHp;
        public static List<float> timerRecord = new List<float>(), recoveryRecord = new List<float>();
        public float lightRotateTimer, lightRotateTimerStoper;
        public Sprite[] lightSprites;

        public Transform nextHoleSide;

        public Animator TutorialAnimator;

        public GameObject reStatUI, sleepingStatUI, confusionStatUI, stickStatUI;
        public SpriteRenderer HPMaxCircleLight;

        SpriteRenderer head;

        public int MaxBulletNum = 5, BulletNum = 5;
        public GameObject Bullet, attackLine, magneticFieldPrefab, playerTrack;
        public Animator playerAttackLineAnimator;

        public static int money = 0, moneyB = 0;

        public enum PlayerStat
        {
            UnSelect,
            CantMove,
            Move
        }
        public PlayerStat playerStat;
        public InsAfterImages insAfterImages;
        Animator playerStatAnimator;

        public Vector2? batPos = null;
        public Transform sticksBat;
        string SceneName;

        static int dashSFXNum = 0;
        [SerializeField] GameObject[] dashSFX;

        static bool Died = false;

        public void Start()
        {
            Died = false;
            v = Vector3.zero;
            batStickedNum = 0;
            for (int i = 0; i < GameManager.players.childCount; i++)
            {
                if (GameManager.players.GetChild(i).GetComponent<PlayerManager>() != this)
                {
                    otherPlayer = GameManager.players.GetChild(i);
                }
            }
            head = transform.GetChild(0).GetComponent<SpriteRenderer>();
            SceneName = SceneManager.GetActiveScene().name;
            playerStatAnimator = GetComponent<Animator>();

            if (!SceneManager.GetActiveScene().name.Contains("SelectRole"))
            {
                playerStat = PlayerStat.Move;
                HP = MaxHP;
            }
            else
            {
                HP = MaxHP;
            }
            playerJoyVibration = GetComponent<PlayerJoyVibration>();
            lastPos = transform.position;
            lastUpdateHp = HP;
        }

        Keyboard keyboard;

        void Update()
        {
            keyboard = Keyboard.current;
            if (playerStatAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
            {
                if (lockedHP || lockedHPTimer <= 2)
                {
                    HP = MaxHP;
                }
                if (playerStat == PlayerStat.Move)
                {
                    TwoPlayerBehavior();
                }
                if (HP <= 0 && HPMaxCircleLight.color.r <= 0.8f)
                {
                    HP0Timer += Time.deltaTime / 2;
                    if (HP0Timer > 0.2f)
                    {
                        Players.DiedAudioSource.Play();

                        if (SceneName.Contains("SelectRole"))
                        {
                            HP = MaxHP;
                            lockedHPTimer = 0;
                        }
                        else
                        {
                            if (--Life > 0)
                            {
                                HP = MaxHP;
                                lockedHPTimer = 0;
                                for (int i = 0; i < GameManager.players.childCount; i++)
                                {
                                    Transform player = GameManager.players.GetChild(i);
                                    PlayerManager playerManager = player.GetComponent<PlayerManager>();
                                    Vector3 size = Vector3.one * ((HP / MaxHP - 0.4f) / 0.6f * (LightRangeMaxSize - LightRangeMinSize) + LightRangeMinSize);
                                    player.GetChild(5).localScale = size;
                                    player.GetChild(6).localScale = size;
                                    playerManager.HPMaxCircleLight.transform.localScale = size;
                                    playerManager.HPMaxCircleLight.color = new Color(1, 1, 1, 0.12f * Mathf.Clamp01(Mathf.InverseLerp(MaxHP * 0.9f, MaxHP, HP)) + 0.15f);
                                }
                            }
                            else if(!Died)
                            {
                                Died = true;
                                DiedTimer = 0;
                                GameObject.Find("DiedLight").GetComponent<Animator>().SetBool("Died", true);
                                if (PlayerPrefs.HasKey("layerFourCntinuousWinTimes") && PlayerPrefs.GetInt("layerFourCntinuousWinTimes") < 3)
                                {
                                    GameManager.layerFourCntinuousWinTimes = 0;
                                    PlayerPrefs.SetInt("layerFourCntinuousWinTimes", 0);
                                    PlayerPrefs.Save();
                                }
                                
                                if(GameManager.CurrentSceneName == "Game 1")
                                {
                                    if(GameManager.layers == 1)
                                    {
                                        if (++GameManager.layerOneCntinuousDideTimes >= 2)
                                        {
                                            if (GameManager.passLayerOneTimes >= 7)
                                            {
                                                GameManager.passLayerOneTimes = 5;
                                                GameManager.layerOneCntinuousDideTimes = 0;
                                            }
                                            else if (GameManager.passLayerOneTimes >= 5)
                                            {
                                                GameManager.passLayerOneTimes = 4;
                                                GameManager.layerOneCntinuousDideTimes = 0;
                                            }
                                        }
                                    }
                                    else if (GameManager.layers == 2)
                                    {
                                        if (++GameManager.layerThreeCntinuousDideTimes >= 2)
                                        {
                                            if (GameManager.passLayerThreeTimes >= 3)
                                            {
                                                GameManager.passLayerThreeTimes = 2;
                                                GameManager.layerThreeCntinuousDideTimes = 0;
                                            }
                                            else if (GameManager.passLayerThreeTimes >= 2)
                                            {
                                                GameManager.passLayerThreeTimes = 1;
                                                GameManager.layerThreeCntinuousDideTimes = 0;
                                            }
                                        }
                                    }
                                    PlayerPrefs.SetInt("passLayerOneTimes", GameManager.passLayerOneTimes);
                                    PlayerPrefs.SetInt("passLayerThreeTimes", GameManager.passLayerThreeTimes);
                                    PlayerPrefs.SetInt("layerOneCntinuousDideTimes", GameManager.layerOneCntinuousDideTimes);
                                    PlayerPrefs.SetInt("layerThreeCntinuousDideTimes", GameManager.layerThreeCntinuousDideTimes);
                                    PlayerPrefs.Save();
                                }
                                else if (GameManager.CurrentSceneName == "Game 2")
                                {
                                    PlayerPrefs.SetString("TaurenStat", "TaurenTalk_PlayerDied");
                                }
                                else if (GameManager.CurrentSceneName == "Game 4")
                                {
                                    PlayerPrefs.SetString("DragonStat", "DragonTalk_PlayerDied");
                                }
                            }
                        }
                    }
                }
                else
                {
                    HP0Timer = 0;
                    float dis = Vector3.Distance(GameManager.players.GetChild(0).localPosition, GameManager.players.GetChild(1).localPosition);
                    float hpUpRate = 0;
                    if (dis < 2f)
                    {
                        if (batStickedNum <= 0)
                        {
                            hpUpRate = 20;//共回40;
                        }
                    }
                    else if (dis < 4.5f)
                    {
                        hpUpRate = 0;
                    }
                    else
                    {
                        hpUpRate = -6f;//共扣12;
                    }

                    if (!SceneName.Contains("SelectRole"))
                    {
                        HP += hpUpRate * Time.deltaTime;
                        if (root && HP<=1)
                        {
                            HP = 1;
                        }
                    }
                    if (HP > MaxHP)
                    {
                        HP = MaxHP;
                    }
                    HPMaxCircleLight.color = new Color(1, 1, 1, 0.12f * Mathf.Clamp01(Mathf.InverseLerp(MaxHP*0.9f,MaxHP,HP)) + 0.15f);
                    HPMaxCircleLight.transform.Rotate(0, 0, Time.deltaTime * 7);
                    recoveryRate();
                    if (HP / MaxHP > 0.4f)//(HP > 20)   //後面的數字是提示的時間，如果要有提示，我推薦0.4，不要的話我推薦0.1
                    {
                        Vector3 size = Vector3.one * ((HP / MaxHP - 0.4f) / 0.6f * (LightRangeMaxSize - LightRangeMinSize) + LightRangeMinSize);
                        transform.GetChild(5).localScale = size;
                        transform.GetChild(6).localScale = size;
                        HPMaxCircleLight.transform.localScale = size;
                        //transform.GetChild(5).localScale = Vector3.one * (HP - 15) / 1.5f;//50 : 23.3333
                        //transform.GetChild(6).localScale = Vector3.one * (HP - 15) / 1.5f;
                    }
                    else
                    {
                        //Vector3 size = Vector3.one * ((LightRangeMinSize -3f) * (HP / MaxHP * 10 / 4) + 3f);
                        Vector3 size = Vector3.one * LightRangeMinSize;
                        transform.GetChild(5).localScale = size;
                        transform.GetChild(6).localScale = size;
                        HPMaxCircleLight.transform.localScale = size;
                        HPMaxCircleLight.color = new Color(0.7f,0.1f, 0.1f);

                        //transform.GetChild(5).localScale = Vector3.one * (5) / 1.5f;//20 : 3.3333
                        //transform.GetChild(6).localScale = Vector3.one * (5) / 1.5f;
                    }
                    if (lightRotateTimer >= lightRotateTimerStoper)
                    {
                        lightRotateTimer = 0;
                        transform.GetChild(6).rotation = transform.GetChild(5).rotation;
                        transform.GetChild(5).rotation = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
                        transform.GetChild(6).GetChild(0).GetComponent<Light2D>().lightCookieSprite = transform.GetChild(5).GetChild(0).GetComponent<Light2D>().lightCookieSprite;//要用Bug處理器解決
                        transform.GetChild(5).GetChild(0).GetComponent<Light2D>().lightCookieSprite = lightSprites[Random.Range(0, lightSprites.Length)];//要用Bug處理器解決
                    }
                    float transition = lightRotateTimer / lightRotateTimerStoper, /*brightness = HP / MaxHP*/brightness = GameManager.Gammar * 1.5f;
                    //brightness *= GameManager.Gammar;

                    if (StickTimer < 10)
                    {
                        if ((int)(StickTimer * 10) % 6 < 1)
                        {
                            brightness *= 1f;
                        }
                        else if ((int)(StickTimer * 10) % 6 < 2)
                        {
                            brightness *= 0.6f;
                        }
                        else if ((int)(StickTimer * 10) % 6 < 3)
                        {
                            brightness *= 0.4f;
                        }
                        else if ((int)(StickTimer * 10) % 6 < 4)
                        {
                            brightness *= 0.4f;
                        }
                        else if ((int)(StickTimer * 10) % 6 < 5)
                        {
                            brightness *= 0.6f;
                        }
                        else if ((int)(StickTimer * 10) % 6 < 6)
                        {
                            brightness *= 1f;
                        }
                    }


                    //reStatUI.SetActive(Players.reTimer < 1);
                    reStatUI.GetComponent<Animator>().SetBool("re", reTimer < 0.5f);
                    sleepingStatUI.SetActive(SleepTimer < 0);
                    confusionStatUI.SetActive(ConfusionTimer < 10);
                    stickStatUI.SetActive(StickTimer < 10);

                    transform.GetChild(5).GetChild(0).GetComponent<Light2D>().intensity = transition * brightness;
                    transform.GetChild(6).GetChild(0).GetComponent<Light2D>().intensity = (1 - transition) * brightness;
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    transform.GetChild(7).GetComponent<Light2D>().pointLightOuterRadius = brightness * 4f;
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
            }
            else
            {
                Debug.LogError("????????????????????");
                v = Vector3.zero;
                GetComponent<Rigidbody2D>().velocity = v;
            }
            timer();
        }

        #region//操作模式

        void TwoPlayerBehavior()
        {
            #region//掙脫
            if (p1)
            {
                switch (InputManager.p1Mod)
                {
                    case InputManager.PlayerMod.keyboardP1:
                        if (keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.keyboardP2:
                        if (keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.gamepadP1:
                        if (InputManager.p1Gamepad.bButton.wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.singleP1:
                        if (Gamepad.current.leftShoulder.wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.singleP2:
                        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                }
            }
            else
            {
                switch (InputManager.p2Mod)
                {
                    case InputManager.PlayerMod.keyboardP1:
                        if (keyboard.allKeys[InputManager.p1KeyboardBreakfreeKeyNum].wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.keyboardP2:
                        if (keyboard.allKeys[InputManager.p2KeyboardBreakfreeKeyNum].wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.gamepadP2:
                        if (InputManager.p2Gamepad.bButton.wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.singleP1:
                        if (Gamepad.current.leftShoulder.wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case InputManager.PlayerMod.singleP2:
                        if (Gamepad.current.rightShoulder.wasPressedThisFrame)
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                }
            }
            #endregion
            if (ConfusionTimer < 10 || StickTimer < 10)
            {
                ConfusionUIRenderer.enabled = true;
                ConfusionUIcontroler.enabled = true;
            }
            else
            {
                ConfusionUIRenderer.enabled = false;
                ConfusionUIcontroler.enabled = false;
            }
            if (ConfusionTimer < 10)
            {
                float tempVX = v.x + Random.Range(-6f, 6f);
                float tempVY = v.y + Random.Range(-6f, 6f);
                v.x += (tempVX + v.x) / 3;
                v.y += (tempVY + v.y) / 3;
            }
            else
            {
                #region//操作移動
                if (p1)
                {
                    switch (InputManager.p1Mod)
                    {
                        case InputManager.PlayerMod.keyboardP1:
                            InputManager.p1KeyboardHorizontalValue = InputManager.keyboardAxes(InputManager.p1KeyboardLeftNum, InputManager.p1KeyboardRightNum, InputManager.p1KeyboardHorizontalValue);
                            v.x = InputManager.p1KeyboardHorizontalValue * moveSpeed;
                            InputManager.p1KeyboardVerticalValue = InputManager.keyboardAxes(InputManager.p1KeyboardDownNum, InputManager.p1KeyboardUpNum, InputManager.p1KeyboardVerticalValue);
                            v.y = InputManager.p1KeyboardVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.keyboardP2:
                            InputManager.p2KeyboardHorizontalValue = InputManager.keyboardAxes(InputManager.p2KeyboardLeftNum, InputManager.p2KeyboardRightNum, InputManager.p2KeyboardHorizontalValue);
                            v.x = InputManager.p2KeyboardHorizontalValue * moveSpeed;
                            InputManager.p2KeyboardVerticalValue = InputManager.keyboardAxes(InputManager.p2KeyboardDownNum, InputManager.p2KeyboardUpNum, InputManager.p2KeyboardVerticalValue);
                            v.y = InputManager.p2KeyboardVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.gamepadP1:
                            Vector2 stickValue = InputManager.p1Gamepad.leftStick.ReadValue();
                            InputManager.p1GamepadHorizontalValue = InputManager.gamepadAxes(stickValue.x, InputManager.p1GamepadHorizontalValue);
                            v.x = InputManager.p1GamepadHorizontalValue * moveSpeed;
                            InputManager.p1GamepadVerticalValue = InputManager.gamepadAxes(stickValue.y, InputManager.p1GamepadVerticalValue);
                            v.y = InputManager.p1GamepadVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.singleP1:
                            stickValue = Gamepad.current.leftStick.ReadValue();
                            InputManager.p1GamepadHorizontalValue = InputManager.gamepadAxes(stickValue.x, InputManager.p1GamepadHorizontalValue);
                            v.x = InputManager.p1GamepadHorizontalValue * moveSpeed;
                            InputManager.p1GamepadVerticalValue = InputManager.gamepadAxes(stickValue.y, InputManager.p1GamepadVerticalValue);
                            v.y = InputManager.p1GamepadVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.singleP2:
                            stickValue = Gamepad.current.rightStick.ReadValue();
                            InputManager.p2GamepadHorizontalValue = InputManager.gamepadAxes(stickValue.x, InputManager.p2GamepadHorizontalValue);
                            v.x = InputManager.p2GamepadHorizontalValue * moveSpeed;
                            InputManager.p2GamepadVerticalValue = InputManager.gamepadAxes(stickValue.y, InputManager.p2GamepadVerticalValue);
                            v.y = InputManager.p2GamepadVerticalValue * moveSpeed;
                            break;
                    }
                }
                else
                {
                    switch (InputManager.p2Mod)
                    {
                        case InputManager.PlayerMod.keyboardP1:
                            InputManager.p1KeyboardHorizontalValue = InputManager.keyboardAxes(InputManager.p1KeyboardLeftNum, InputManager.p1KeyboardRightNum, InputManager.p1KeyboardHorizontalValue);
                            v.x = InputManager.p1KeyboardHorizontalValue * moveSpeed;
                            InputManager.p1KeyboardVerticalValue = InputManager.keyboardAxes(InputManager.p1KeyboardDownNum, InputManager.p1KeyboardUpNum, InputManager.p1KeyboardVerticalValue);
                            v.y = InputManager.p1KeyboardVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.keyboardP2:
                            InputManager.p2KeyboardHorizontalValue = InputManager.keyboardAxes(InputManager.p2KeyboardLeftNum, InputManager.p2KeyboardRightNum, InputManager.p2KeyboardHorizontalValue);
                            v.x = InputManager.p2KeyboardHorizontalValue * moveSpeed;
                            InputManager.p2KeyboardVerticalValue = InputManager.keyboardAxes(InputManager.p2KeyboardDownNum, InputManager.p2KeyboardUpNum, InputManager.p2KeyboardVerticalValue);
                            v.y = InputManager.p2KeyboardVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.gamepadP2:
                            Vector2 stickValue = InputManager.p2Gamepad.leftStick.ReadValue();
                            InputManager.p2GamepadHorizontalValue = InputManager.gamepadAxes(stickValue.x, InputManager.p2GamepadHorizontalValue);
                            v.x = InputManager.p2GamepadHorizontalValue * moveSpeed;
                            InputManager.p2GamepadVerticalValue = InputManager.gamepadAxes(stickValue.y, InputManager.p2GamepadVerticalValue);
                            v.y = InputManager.p2GamepadVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.singleP1:
                            stickValue = Gamepad.current.leftStick.ReadValue();
                            InputManager.p1GamepadHorizontalValue = InputManager.gamepadAxes(stickValue.x, InputManager.p1GamepadHorizontalValue);
                            v.x = InputManager.p1GamepadHorizontalValue * moveSpeed;
                            InputManager.p1GamepadVerticalValue = InputManager.gamepadAxes(stickValue.y, InputManager.p1GamepadVerticalValue);
                            v.y = InputManager.p1GamepadVerticalValue * moveSpeed;
                            break;
                        case InputManager.PlayerMod.singleP2:
                            stickValue = Gamepad.current.rightStick.ReadValue();
                            InputManager.p2GamepadHorizontalValue = InputManager.gamepadAxes(stickValue.x, InputManager.p2GamepadHorizontalValue);
                            v.x = InputManager.p2GamepadHorizontalValue * moveSpeed;
                            InputManager.p2GamepadVerticalValue = InputManager.gamepadAxes(stickValue.y, InputManager.p2GamepadVerticalValue);
                            v.y = InputManager.p2GamepadVerticalValue * moveSpeed;
                            break;
                    }
                }
                #endregion
            }
            if ((StickTimer) < 10)
            {
                if (v.magnitude > 0.5f)
                {
                    v = v.normalized * 0.5f;
                }
            }
            else if (v.magnitude > moveSpeed)
            {
                v = v.normalized * moveSpeed;
            }
            if (HardStraightTimer < 0.3f)
            {
                v = HardStraightA;
            }
            if (SleepTimer < 0f)
            {
                v = Vector3.zero;
            }
            //玩家解除無敵狀態
            else if (HardStraightTimer > 0.5f)
            {
                gameObject.layer = 8;
            }

            #region//衝刺
            if (HardStraightTimer >= 0.3f && ConfusionTimer >= 10 && SleepTimer >= 0 && StickTimer >= 10)
            {
                if (DashTimer > DashCD)
                {
                    if (p1)
                    {
                        switch (InputManager.p1Mod)
                        {
                            case InputManager.PlayerMod.keyboardP1:
                                if (keyboard.allKeys[InputManager.p1KeyboardDashNum].wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p1Mod));
                                }
                                break;
                            case InputManager.PlayerMod.keyboardP2:
                                if (keyboard.allKeys[InputManager.p2KeyboardDashNum].wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p1Mod));
                                }
                                break;
                            case InputManager.PlayerMod.gamepadP1:
                                if (InputManager.p1Gamepad.aButton.wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p1Mod));
                                }
                                break;
                            case InputManager.PlayerMod.singleP1:
                                if (Gamepad.current.leftShoulder.wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p1Mod));
                                }
                                break;
                            case InputManager.PlayerMod.singleP2:
                                if (Gamepad.current.rightShoulder.wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p1Mod));
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (InputManager.p2Mod)
                        {
                            case InputManager.PlayerMod.keyboardP1:
                                if (keyboard.allKeys[InputManager.p1KeyboardDashNum].wasPressedThisFrame)
                                {
                                    Debug.LogError("tryDash");
                                    StartCoroutine(dash(InputManager.p2Mod));
                                }
                                break;
                            case InputManager.PlayerMod.keyboardP2:
                                if (keyboard.allKeys[InputManager.p2KeyboardDashNum].wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p2Mod));
                                }
                                break;
                            case InputManager.PlayerMod.gamepadP2:
                                if (InputManager.p2Gamepad.aButton.wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p2Mod));
                                }
                                break;
                            case InputManager.PlayerMod.singleP1:
                                if (Gamepad.current.leftShoulder.wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p2Mod));
                                }
                                break;
                            case InputManager.PlayerMod.singleP2:
                                if (Gamepad.current.rightShoulder.wasPressedThisFrame)
                                {
                                    StartCoroutine(dash(InputManager.p2Mod));
                                }
                                break;
                        }
                    }
                }
                if (DashTimer < 0.3f)
                {
                    v = DashA;
                }
            }
            #endregion
            
            #region//傳送
            if (homeButton)
            {
                if (HardStraightTimer >= 0.3f && ConfusionTimer >= 10 && SleepTimer >= 0 && StickTimer >= 10 && DashTimer > DashCD)
                {
                    if (p1)
                    {
                        switch (InputManager.p1Mod)
                        {
                            case InputManager.PlayerMod.keyboardP1:
                                if (keyboard.allKeys[InputManager.p1KeyboardSkillKeyNum].wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.keyboardP2:
                                if (keyboard.allKeys[InputManager.p2KeyboardSkillKeyNum].wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.gamepadP1:
                                if (InputManager.p1Gamepad.xButton.wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.singleP1:
                                if (Gamepad.current.leftTrigger.wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.singleP2:
                                if (Gamepad.current.rightTrigger.wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (InputManager.p2Mod)
                        {
                            case InputManager.PlayerMod.keyboardP1:
                                if (keyboard.allKeys[InputManager.p1KeyboardSkillKeyNum].wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.keyboardP2:
                                if (keyboard.allKeys[InputManager.p2KeyboardSkillKeyNum].wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.gamepadP2:
                                if (InputManager.p2Gamepad.xButton.wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.singleP1:
                                if (Gamepad.current.leftTrigger.wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                            case InputManager.PlayerMod.singleP2:
                                if (Gamepad.current.rightTrigger.wasPressedThisFrame)
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -homeButtonTimerStoper * 2;
                                        for (int i = 0; i < GameManager.players.childCount; i++)
                                        {
                                            if (GameManager.players.GetChild(i) != transform)
                                            {
                                                transform.position = GameManager.players.GetChild(i).position;
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }
            #endregion
            if (AbilityShower.showSelected)
            {
                v = Vector3.zero;
            }
            GetComponent<Rigidbody2D>().velocity = v;

            transform.GetChild(8).transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, v, Vector3.forward) - transform.GetChild(8).GetComponent<ParticleSystem>().shape.arc / 2 + 180);
        }
        #endregion

        static WaitForSeconds waitForPress = new WaitForSeconds(0.03f);
        static WaitForSeconds waitForMinut = new WaitForSeconds(0.01f);
        IEnumerator dash(InputManager.PlayerMod playerMod)
        {
            yield return waitForPress;

            dashCount(playerMod);
            DashA = Vector3.Normalize(DashA) * DashSpeed;
            for(int i = 0; i < 37; i++)
            {
                if (DashA.magnitude <= 10)
                {
                    yield return waitForMinut;
                    dashCount(playerMod);
                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                }
                if (DashA.magnitude > 10)
                {
                    break;
                }
            }
            if (DashA.magnitude > 10)
            {
                playerJoyVibration.DashVibration = 0.6f;
                if (++dashSFXNum >= dashSFX.Length)
                {
                    dashSFXNum = 0;
                }
                Destroy(Instantiate(dashSFX[dashSFXNum], CameraManager.center, Quaternion.identity), 2);
                TrackBullet();
            }
            else
            {
                yield break;
            }
            DashTimer = 0;
            LineAttack();
            insAfterImages.timer = 0;

            if (magneticField)
            {
                MagneticField magneticField = Instantiate(magneticFieldPrefab, GameManager.magneticFields).GetComponent<MagneticField>();
                magneticField.PosA.position = transform.position;
                magneticField.PosB.position = transform.position;
                magneticField.PosC.position = otherPlayer.position;
                magneticField.InsPlayer = transform;
            }
        }

        void dashCount(InputManager.PlayerMod playerMod)
        {
            bool up, down, left, right;
            switch (playerMod)
            {
                case InputManager.PlayerMod.keyboardP1:
                    up = keyboard.allKeys[InputManager.p1KeyboardUpNum].isPressed;
                    down = keyboard.allKeys[InputManager.p1KeyboardDownNum].isPressed;
                    left = keyboard.allKeys[InputManager.p1KeyboardLeftNum].isPressed;
                    right = keyboard.allKeys[InputManager.p1KeyboardRightNum].isPressed;
                    if (left && right || !left && !right)
                    {
                        DashA.x = 0;
                    }
                    else if (left)
                    {
                        DashA.x = -1;
                    }
                    else
                    {
                        DashA.x = 1;
                    }
                    if (up && down || !up && !down)
                    {
                        DashA.y = 0;
                    }
                    else if (up)
                    {
                        DashA.y = 1;
                    }
                    else
                    {
                        DashA.y = -1;
                    }
                    break;
                case InputManager.PlayerMod.keyboardP2:
                    up = keyboard.allKeys[InputManager.p2KeyboardUpNum].isPressed;
                    down = keyboard.allKeys[InputManager.p2KeyboardDownNum].isPressed;
                    left = keyboard.allKeys[InputManager.p2KeyboardLeftNum].isPressed;
                    right = keyboard.allKeys[InputManager.p2KeyboardRightNum].isPressed;
                    if (left && right || !left && !right)
                    {
                        DashA.x = 0;
                    }
                    else if (left)
                    {
                        DashA.x = -1;
                    }
                    else
                    {
                        DashA.x = 1;
                    }
                    if (up && down || !up && !down)
                    {
                        DashA.y = 0;
                    }
                    else if (up)
                    {
                        DashA.y = 1;
                    }
                    else
                    {
                        DashA.y = -1;
                    }
                    break;
                case InputManager.PlayerMod.gamepadP1:
                    DashA = InputManager.p1Gamepad.leftStick.ReadValue();
                    break;
                case InputManager.PlayerMod.gamepadP2:
                    DashA = InputManager.p2Gamepad.leftStick.ReadValue();
                    break;
                case InputManager.PlayerMod.singleP1:
                    DashA = Gamepad.current.leftStick.ReadValue();
                    break;
                case InputManager.PlayerMod.singleP2:
                    DashA = Gamepad.current.rightStick.ReadValue();
                    break;
            }
        }
        private void FixedUpdate()
        {
            //v *= 0.9f;
            if (HardStraightA.magnitude > 0.8f)
            {
                HardStraightA -= (Vector2)Vector3.Normalize(HardStraightA) * 0.8f;
            }
            else
            {
                HardStraightA = Vector3.zero;
            }
            if (DashA.magnitude > 0.5f)
            {
                DashA -= (Vector2)Vector3.Normalize(DashA) * 0.4f;
            }
            else
            {
                DashA = Vector3.zero;
            }
        }

        void TrackBullet()
        {
            if (trackBullet)
            {
                Transform minDisMonster = null;
                float minDis = 5;//距離至少要5以下才會觸發攻擊
                for (int i = 0; i < GameManager.monsters.childCount; i++)
                {
                    Transform monster = GameManager.monsters.GetChild(i);
                    if (monster.gameObject.activeSelf)
                    {
                        if (Vector2.Distance(monster.position, transform.position) < minDis)
                        {
                            float Dis = Vector2.Distance(monster.position, transform.position);
                            minDisMonster = monster;
                            minDis = Dis;
                        }
                    }
                }
                if (minDisMonster != null)
                {
                    Instantiate(playerTrack, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<PlayerTrack>().Target = minDisMonster;
                }
            }
        }

        void timer()
        {
            CDTimer += Time.deltaTime;
            beganTouchedTimer += Time.deltaTime;
            homeButtonTimer += Time.deltaTime;
            DashTimer += Time.deltaTime;
            HardStraightTimer += Time.deltaTime;
            SleepTimer += Time.deltaTime;
            ConfusionTimer += Time.deltaTime;
            StickTimer += Time.deltaTime;
            lightRotateTimer += Time.deltaTime;
            reTimer += Time.deltaTime;

            if (flash)
            {
                if ((flashTimer += Time.deltaTime) > flashTimerStoper)
                {
                    flash = false;
                    flashTimer = 0;
                }
            }
        }

        void recoveryRate()
        {
            timerRecord.Add(Time.time);
            if (HP == MaxHP)
            {
                recoveryRecord.Add(0.3f);
            }
            else
            {
                recoveryRecord.Add(HP - lastUpdateHp);
            }
            lastUpdateHp = HP;
            while (Time.time - timerRecord[0] > 0.2f)
            {
                timerRecord.RemoveAt(0);
                recoveryRecord.RemoveAt(0);
            }
            lightRotateTimerStoper = (countAverage(recoveryRecord) + 0.1f) * 5;
            if (lightRotateTimerStoper < 0.05f + HP * 0.005f)
            {
                lightRotateTimerStoper = 0.05f + HP * 0.005f;
            }
        }

        public static float countAverage(List<float> counted)
        {
            float total = 0;
            for (int i = 0; i < counted.Count; i++)
            {
                total += counted[i];
            }
            return total / counted.Count;
        }

        public Transform minDisMonster()
        {
            Vector3 myPos = transform.position;
            Transform monsters = GameManager.monsters;
            float minDis = 20;
            Transform minDisMonster = null;
            for (int i = 0; i < monsters.childCount; i++)
            {
                float dis = Vector3.Distance(myPos, monsters.GetChild(i).position);
                if (minDis > dis)
                {
                    RaycastHit2D hit = Physics2D.Raycast(myPos, monsters.GetChild(i).position - myPos, dis, 1 << 12);
                    if (!hit)
                    {
                        minDisMonster = monsters.GetChild(i);
                        minDis = dis;
                    }
                }
            }
            return minDisMonster;
        }

        void shootBullet()
        {
            Transform minDisMonster = this.minDisMonster();
            float angle;
            if (minDisMonster != null)
            {
                angle = Vector3.SignedAngle(transform.right, minDisMonster.position - transform.position, Vector3.forward);
                Debug.LogFormat(minDisMonster.gameObject.name);
                Debug.Log(angle);
            }
            else
            {
                angle = 0;
            }
            Instantiate(Bullet, transform.position, Quaternion.Euler(0, 0, angle));
            BulletNum--;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<MonsterManager>() && !collision.collider.GetComponent<Slime>() && !collision.collider.GetComponent<Bat>())
            {
                if (HardStraightTimer > 0.3f)
                {
                    //ShowLockHP.hurtTimer = 0;
                    DashA = Vector3.zero;
                    if (collision.collider.GetComponent<TaurenBoss>())
                    {
                        TaurenBoss taurenBoss = collision.collider.GetComponent<TaurenBoss>();
                        if (taurenBoss.punching && Vector2.Angle(taurenBoss.RecordDir, transform.position - collision.transform.position) < 90)
                        {
                            HardStraightA = (Vector2)taurenBoss.RecordDir * 40;
                            if (HP / MaxHP <= 0.4f)
                            {
                                HP -= 20 * (100f - reducesDamage) / 100f;
                            }
                            else
                            {
                                HP -= 20;
                            }
                            try
                            {
                                Camera.main.GetComponent<Animator>().SetTrigger("Hit");
                            }
                            catch
                            {
                                Debug.LogError("這場景忘了放畫面抖動");
                            }
                            Instantiate(GameManager.Hurted, transform.position, Quaternion.identity, transform);
                            HardStraightTimer = 0.1f;
                        }
                        else
                        {
                            HardStraightA = (Vector2)Vector3.Normalize(transform.position - collision.transform.position) * 10;
                            if (HP / MaxHP <= 0.4f)
                            {
                                HP -= 10 * (100f - reducesDamage) / 100f;
                            }
                            else
                            {
                                HP -= 10;
                            }
                            try
                            {
                                Camera.main.GetComponent<Animator>().SetTrigger("Hit");
                            }
                            catch
                            {
                                Debug.LogError("這場景忘了放畫面抖動");
                            }
                            Instantiate(GameManager.Hurted, transform.position, Quaternion.identity, transform);
                            HardStraightTimer = 0;
                        }
                    }
                    else
                    {
                        HardStraightA = (Vector2)Vector3.Normalize(transform.position - collision.transform.position) * 10;
                        if (HP / MaxHP <= 0.4f)
                        {
                            HP -= 15 * (100f - reducesDamage) / 100f;
                        }
                        else
                        {
                            HP -= 15;
                        }
                        try
                        {
                            Camera.main.GetComponent<Animator>().SetTrigger("Hit");
                        }
                        catch
                        {
                            Debug.LogError("這場景忘了放畫面抖動");
                        }
                        Instantiate(GameManager.Hurted,transform.position,Quaternion.identity, transform);
                        HardStraightTimer = 0;
                    }
                    playerJoyVibration.hurt();

                    //玩家進入無敵狀態
                    gameObject.layer = 16;

                    if (collision.collider.GetComponent<Spider>())
                    {
                        if (p1)
                        {
                            GameManager.P1SpiderHit++;
                        }
                        else
                        {
                            GameManager.P2SpiderHit++;
                        }
                        GameManager.DiedBecause = "SpiderHit";
                        GameManager.DiedBecauseTimer = 0;
                    }
                }
            }
            else if (collision.collider.GetComponent<Slime>())
            {
                if (p1)
                {
                    GameManager.P1SlimeHit++;
                }
                else
                {
                    GameManager.P2SlimeHit++;
                }
            }
            if (collision.collider.GetComponent<MonsterManager>())
            {
                Players.fightingTimer = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.name == "NormalAttack")
            {
                HardStraightA = (Vector2)(transform.position - collider.transform.position).normalized * 10;
                HardStraightTimer = 0.1f;
            }
        }

        void LineAttack()
        {
            playerAttackLineAnimator.SetTrigger("LineAttack");
        }

        public void getOutHole()
        {
            Debug.LogError(nextHoleSide.parent.parent.name, nextHoleSide.gameObject);
            Debug.LogError(nextHoleSide.position);
            if (nextHoleSide)
            {
                transform.position = nextHoleSide.position;
            }
        }
    }
}