using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using XInputDotNetPure; // Required in C#

namespace com.DungeonPad
{
    public class PlayerManager : MonoBehaviour
    {
        public static float MaxHP = 60, HP = 60;
        public static int Life = 2, MaxLife = 4;
        public static bool lockedHP = false;
        public static float lockedHPTimer = 10, DiedTimer = 10;
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

        const float LightRangeMinSize = 2.5f, LightRangeMaxSize = 20;
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
        public GameObject Bullet, attackLine, magneticFieldPrefab;
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

        public void Start()
        {
            if(SceneManager.GetActiveScene().name == "Game 1")
            {
                if (root)
                {
                    GameObject.Find("Directional Light 2D").GetComponent<Light2D>().intensity = 0.8f;
                }
                else
                {
                    GameObject.Find("Directional Light 2D").GetComponent<Light2D>().intensity = 0;
                }
            }
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
                if (SelectMouse.playerColor == SelectMouse.PlayerColor.P1Blue_P2Red)
                {
                    if (transform.name == "Blue")
                    {
                        p1 = true;
                        if (SelectMouse.P1PlayerIndex == null)
                        {
                            GetComponent<PlayerJoyVibration>().enabled = false;
                            print(SelectMouse.P1PlayerIndex);
                        }
                        else
                        {
                            GetComponent<PlayerJoyVibration>().playerIndex = SelectMouse.P1PlayerIndex;
                            print(GetComponent<PlayerJoyVibration>().playerIndex + "p1");
                        }
                    }
                    else
                    {
                        p1 = false;
                        if (SelectMouse.P2PlayerIndex == null)
                        {
                            GetComponent<PlayerJoyVibration>().enabled = false;
                            print(SelectMouse.P1PlayerIndex);
                        }
                        else
                        {
                            GetComponent<PlayerJoyVibration>().playerIndex = SelectMouse.P2PlayerIndex;
                            print(GetComponent<PlayerJoyVibration>().playerIndex + "p2");
                        }
                    }
                }
                else
                {
                    if (transform.name == "Blue")
                    {
                        p1 = false;
                        if (SelectMouse.P2PlayerIndex == null)
                        {
                            GetComponent<PlayerJoyVibration>().enabled = false;
                            print(SelectMouse.P2PlayerIndex);
                        }
                        else
                        {
                            GetComponent<PlayerJoyVibration>().playerIndex = SelectMouse.P2PlayerIndex;
                            print(GetComponent<PlayerJoyVibration>().playerIndex + "p2");
                        }
                    }
                    else
                    {
                        p1 = true;
                        if (SelectMouse.P1PlayerIndex == null)
                        {
                            GetComponent<PlayerJoyVibration>().enabled = false;
                            print(SelectMouse.P1PlayerIndex);
                        }
                        else
                        {
                            GetComponent<PlayerJoyVibration>().playerIndex = SelectMouse.P1PlayerIndex;
                            print(GetComponent<PlayerJoyVibration>().playerIndex + "p1");
                        }
                    }
                }
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


        void Update()
        {
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
                if (HP <= 0)
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
                        }
                        else
                        {
                            DiedTimer = 0;
                        }
                    }
                }
                else
                {
                    float dis = Vector3.Distance(GameManager.players.GetChild(0).localPosition, GameManager.players.GetChild(1).localPosition);
                    float hpUpRate = 0;
                    if (dis < 2f)
                    {
                        if (batStickedNum <= 0)
                        {
                            hpUpRate = 20;//共回40;
                        }
                        Players.canTrack = false;
                    }
                    else if (dis < 4.5f)
                    {
                        hpUpRate = 0;
                        Players.canTrack = true;
                    }
                    else
                    {
                        hpUpRate = -6f;//共扣12;
                        Players.canTrack = true;
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
                    HPMaxCircleLight.color = new Color(1, 1, 1, 0.15f * Mathf.Clamp01(Mathf.InverseLerp(MaxHP*0.9f,MaxHP,HP)) + 0.07f);
                    HPMaxCircleLight.transform.Rotate(0, 0, Time.deltaTime * 7);
                    recoveryRate();
                    if (HP / MaxHP > 0.4f)//(HP > 20)
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
                        Vector3 size = Vector3.one * LightRangeMinSize;
                        transform.GetChild(5).localScale = size;
                        transform.GetChild(6).localScale = size;
                        HPMaxCircleLight.transform.localScale = size;
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

        bool canUseTriggerButton = true;

        #region//操作模式
        void OnePlayerBehavior()
        {
            #region//掙脫
            if (p1)
            {
                switch (SelectMouse.p1Joy)
                {
                    case "WASD":
                        if (Input.GetKeyDown(KeyCode.LeftAlt))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "ArrowKey":
                        if (Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        if (Input.GetAxis("RT")>0.1f)
                        {
                            if (canUseTriggerButton)
                            {
                                ConfusionTimer += 0.7f;
                                StickTimer += 0.7f;
                                lastDirRight = true;
                                canUseTriggerButton = false;
                            }
                        }
                        else
                        {
                            canUseTriggerButton = true;
                        }
                        break;
                }
            }
            else
            {
                switch (SelectMouse.p2Joy)
                {
                    case "WASD":
                        if (Input.GetKeyDown(KeyCode.LeftAlt))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "ArrowKey":
                        if (Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        if (Input.GetAxis("LT") > 0.1f)
                        {
                            if (canUseTriggerButton)
                            {
                                ConfusionTimer += 0.7f;
                                StickTimer += 0.7f;
                                lastDirRight = true;
                                canUseTriggerButton = false;
                            }
                        }
                        else
                        {
                            canUseTriggerButton = true;
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
                    switch (SelectMouse.p1Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalWASD") * moveSpeed * 2 / 3;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("HorizontalArrowKey") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalArrowKey") * moveSpeed * 2 / 3;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoyP1") * moveSpeed * 2 / 3;
                            v.y -= Input.GetAxis("VerticalJoyP1") * moveSpeed * 2 / 3;
                            break;
                    }
                }
                else
                {
                    switch (SelectMouse.p2Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalWASD") * moveSpeed * 2 / 3;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("HorizontalArrowKey") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalArrowKey") * moveSpeed * 2 / 3;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoyP2") * moveSpeed * 2 / 3;
                            v.y -= Input.GetAxis("VerticalJoyP2") * moveSpeed * 2 / 3;
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
                        switch (SelectMouse.p1Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.Space))
                                {
                                    StartCoroutine(dash(SelectMouse.p1Joy, 1));
                                    
                                }
                                break;
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad0))
                                {
                                    StartCoroutine(dash(SelectMouse.p1Joy, 1));
                                }
                                break;
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown(KeyCode.JoystickButton4))
                                {
                                    StartCoroutine(dash(SelectMouse.p1Joy, 1));
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (SelectMouse.p2Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.Space))
                                {
                                    StartCoroutine(dash(SelectMouse.p2Joy, 2));
                                }
                                break;
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad0))
                                {
                                    StartCoroutine(dash(SelectMouse.p2Joy, 2));
                                }
                                break;
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown(KeyCode.JoystickButton5))
                                {
                                    StartCoroutine(dash(SelectMouse.p2Joy, 2));
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
                        switch (SelectMouse.p1Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.M))
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
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad0))
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
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy) + 2)))
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
                        switch (SelectMouse.p2Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.M))
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
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad0))
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
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy) + 2)))
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
            GetComponent<Rigidbody2D>().velocity = v;
            transform.GetChild(8).transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, v, Vector3.forward) - transform.GetChild(8).GetComponent<ParticleSystem>().shape.arc / 2 + 180);

            #region//子彈
            /*
            if (BulletNum > 0)
            {
                if (p1)
                {
                    switch (SelectMouse.p1Joy)
                    {
                        case "WASD":
                            break;
                        case "ArrowKey":
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy) + 5)))
                            {
                                shootBullet();
                            }
                            break;
                    }
                }
                else
                {
                    switch (SelectMouse.p2Joy)
                    {
                        case "WASD":
                            break;
                        case "ArrowKey":
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy) + 5)))
                            {
                                shootBullet();
                            }
                            break;
                    }
                }
            }

            //loat x = Input.GetAxisRaw("HorizontalJoy" + SelectMouse.p1Joy + "R");
            */
            #endregion
        }
        void TwoPlayerBehavior()
        {
            #region//掙脫
            if (p1)
            {
                switch (SelectMouse.p1Joy)
                {
                    case "WASD":
                        if (Input.GetKeyDown(KeyCode.J))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "ArrowKey":
                        if (Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy) + 1)))
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
                switch (SelectMouse.p2Joy)
                {
                    case "WASD":
                        if (Input.GetKeyDown(KeyCode.J))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "ArrowKey":
                        if (Input.GetKeyDown(KeyCode.Keypad1))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy) + 1)))
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
                    switch (SelectMouse.p1Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalWASD") * moveSpeed * 2 / 3;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("HorizontalArrowKey") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalArrowKey") * moveSpeed * 2 / 3;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoy" + SelectMouse.p1Joy) * moveSpeed * 2 / 3;
                            v.y -= Input.GetAxis("VerticalJoy" + SelectMouse.p1Joy) * moveSpeed * 2 / 3;
                            break;
                    }
                }
                else
                {
                    switch (SelectMouse.p2Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalWASD") * moveSpeed * 2 / 3;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("HorizontalArrowKey") * moveSpeed * 2 / 3;
                            v.y += Input.GetAxis("VerticalArrowKey") * moveSpeed * 2 / 3;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoy" + SelectMouse.p2Joy) * moveSpeed * 2 / 3;
                            v.y -= Input.GetAxis("VerticalJoy" + SelectMouse.p2Joy) * moveSpeed * 2 / 3;
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
                        switch (SelectMouse.p1Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.J))
                                {
                                    StartCoroutine(dash(SelectMouse.p1Joy, 0));
                                }
                                break;
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad1))
                                {
                                    StartCoroutine(dash(SelectMouse.p1Joy, 0));
                                }
                                break;
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy))))
                                {
                                    StartCoroutine(dash(SelectMouse.p1Joy, 0));
                                }
                                break;
                        }
                    }
                    else
                    {
                        switch (SelectMouse.p2Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.J))
                                {
                                    StartCoroutine(dash(SelectMouse.p2Joy, 0));
                                }
                                break;
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad1))
                                {
                                    StartCoroutine(dash(SelectMouse.p2Joy, 0));
                                }
                                break;
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy))))
                                {
                                    StartCoroutine(dash(SelectMouse.p2Joy, 0));
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
                        switch (SelectMouse.p1Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.M))
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
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad0))
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
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy) + 2)))
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
                        switch (SelectMouse.p2Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.M))
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
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad0))
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
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy) + 2)))
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
            GetComponent<Rigidbody2D>().velocity = v;
            transform.GetChild(8).transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, v, Vector3.forward) - transform.GetChild(8).GetComponent<ParticleSystem>().shape.arc / 2 + 180);

            #region//子彈

            if (BulletNum > 0)
            {
                if (p1)
                {
                    switch (SelectMouse.p1Joy)
                    {
                        case "WASD":
                            break;
                        case "ArrowKey":
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy) + 5)))
                            {
                                shootBullet();
                            }
                            break;
                    }
                }
                else
                {
                    switch (SelectMouse.p2Joy)
                    {
                        case "WASD":
                            break;
                        case "ArrowKey":
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy) + 5)))
                            {
                                shootBullet();
                            }
                            break;
                    }
                }
            }

            //loat x = Input.GetAxisRaw("HorizontalJoy" + SelectMouse.p1Joy + "R");
            #endregion
        }
        #endregion

        static WaitForSeconds waitForPress = new WaitForSeconds(0.03f);
        static WaitForSeconds waitForMinut = new WaitForSeconds(0.01f);
        IEnumerator dash(string p1p2joy, int TwoPlayerModeORp1p2)
        {
            yield return waitForPress;
            switch (p1p2joy)
            {
                case "WASD":
                    DashA.x = Input.GetAxisRaw("HorizontalWASD");
                    DashA.y = Input.GetAxisRaw("VerticalWASD");
                    break;
                case "ArrowKey":
                    DashA.x = Input.GetAxisRaw("HorizontalArrowKey");
                    DashA.y = Input.GetAxisRaw("VerticalArrowKey");
                    break;
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                    if (TwoPlayerModeORp1p2 == 0)
                    {
                        DashA.x = Input.GetAxisRaw("HorizontalJoy" + p1p2joy);
                        DashA.y = -Input.GetAxisRaw("VerticalJoy" + p1p2joy);
                    }
                    else
                    {
                        DashA.x = Input.GetAxisRaw("HorizontalJoyP" + TwoPlayerModeORp1p2);
                        DashA.y = -Input.GetAxisRaw("VerticalJoyP" + TwoPlayerModeORp1p2);
                    }

                    break;
            }
            DashA = Vector3.Normalize(DashA) * DashSpeed;
            for(int i = 0; i < 5; i++)
            {
                if (DashA.magnitude <= 10)
                {
                    yield return waitForMinut;
                    switch (p1p2joy)
                    {
                        case "WASD":
                            DashA.x = Input.GetAxisRaw("HorizontalWASD");
                            DashA.y = Input.GetAxisRaw("VerticalWASD");
                            break;
                        case "ArrowKey":
                            DashA.x = Input.GetAxisRaw("HorizontalArrowKey");
                            DashA.y = Input.GetAxisRaw("VerticalArrowKey");
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            if (TwoPlayerModeORp1p2 == 0)
                            {
                                DashA.x = Input.GetAxisRaw("HorizontalJoy" + p1p2joy);
                                DashA.y = -Input.GetAxisRaw("VerticalJoy" + p1p2joy);
                            }
                            else
                            {
                                DashA.x = Input.GetAxisRaw("HorizontalJoyP" + TwoPlayerModeORp1p2);
                                DashA.y = -Input.GetAxisRaw("VerticalJoyP" + TwoPlayerModeORp1p2);
                            }

                            break;
                    }
                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                }
                if (DashA.magnitude > 10)
                {
                    break;
                }
            }

            /*if (DashA.magnitude <= 10)
            {

                if (head.flipX)
                {
                    DashA = Vector3.right * DashSpeed;
                }
                else
                {
                    DashA = Vector3.left * DashSpeed;
                }
            }*/
            if (DashA.magnitude > 10)
            {
                playerJoyVibration.DashVibration = 0.7f;
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

        private void FixedUpdate()
        {
            v *= 0.9f;
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
                        if (Vector2.Angle(taurenBoss.RecordDir, transform.position - collision.transform.position) < 90 && taurenBoss.punching)
                        {
                            HardStraightA = (Vector2)taurenBoss.RecordDir * 40;
                            if (HP <= MaxHP * 0.3f)
                            {
                                HP -= 20 * (100f - reducesDamage) / 100f;
                            }
                            else
                            {
                                HP -= 20;
                            }

                            Instantiate(GameManager.Hurted, transform.position, Quaternion.identity, transform);
                            HardStraightTimer = 0.1f;
                        }
                        else
                        {
                            HardStraightA = (Vector2)Vector3.Normalize(transform.position - collision.transform.position) * 10;
                            if (HP <= MaxHP * 0.3f)
                            {
                                HP -= 10 * (100f - reducesDamage) / 100f;
                            }
                            else
                            {
                                HP -= 10;
                            }
                            Instantiate(GameManager.Hurted, transform.position, Quaternion.identity, transform);
                            HardStraightTimer = 0;
                        }
                    }
                    else
                    {
                        HardStraightA = (Vector2)Vector3.Normalize(transform.position - collision.transform.position) * 10;
                        if (HP <= MaxHP * 0.3f)
                        {
                            HP -= 15 * (100f - reducesDamage) / 100f;
                        }
                        else
                        {
                            HP -= 15;
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
            if (collider.GetComponent<MonsterAttack>())
            {
                Players.fightingTimer = 0;
            }
            if (collider.GetComponent<Ammunition>())
            {

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