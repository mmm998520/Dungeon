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
        public static float MaxHP = 40, HP = 40;
        public static int Life = 2, MaxLife = 4;
        public static bool lockedHP = true;
        public static float lockedHPTimer = 10;
        public float ATK, hand, atkTime;
        public bool continued = false;
        public float CD, CDTimer;
        public GameObject attack, regener, ridiculeWind;
        Vector3 lastPos;
        public bool locked = true, flash = false;
        public float beganTouchedTimer, flashTimer, flashTimerStoper;
        public static float moveSpeed = 3f, DashSpeed = 11, DashCD = 0.5f, reducesDamage = 0, criticalRate=0;
        public List<Vector3> startRayPoss;

        public bool p1;
        public bool lastDirRight;
        public Vector2 v = Vector2.zero, HardStraightA = Vector2.zero, DashA = Vector2.zero;

        public PlayerJoyVibration playerJoyVibration;

        public float StickTimer = 10, HardStraightTimer = 10, DashTimer = 10, SleepTimer = 10;
        public float ConfusionTimer = 100;
        public SpriteRenderer ConfusionUIRenderer;
        public ConfusionUIcontroler ConfusionUIcontroler;

        float lastUpdateHp;
        public static List<float> timerRecord = new List<float>(), recoveryRecord = new List<float>();
        public float lightRotateTimer, lightRotateTimerStoper;
        public Sprite[] lightSprites;

        public List<Vector3> nextPosBeforeIntoHole = new List<Vector3>();
        public List<float> nextPosBeforeIntoHoleTimer = new List<float>();
        public bool IntoHole = false;

        public Animator TutorialAnimator;

        public GameObject reStatUI, sleepingStatUI, confusionStatUI, stickStatUI;

        public int MaxBulletNum = 5, BulletNum = 5;
        public GameObject Bullet, attackLine;

        public static int money = 0;

        public enum PlayerStat
        {
            UnSelect,
            CantMove,
            Move
        }
        public PlayerStat playerStat;
        public InsAfterImages insAfterImages;

        private void Start()
        {
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
            nextPosBeforeIntoHole = new List<Vector3>();
            nextPosBeforeIntoHoleTimer = new List<float>();
    }

        public static float homeButtonTimer = 0;

        void Update()
        {
            if (lockedHP || (lockedHPTimer += Time.deltaTime/2) <= 2)
            {
                HP = MaxHP;
            }
            if(playerStat == PlayerStat.Move)
            {
                Behavior();
            }
            if (HP <= 0)
            {
                if (--Life <= 0)
                {
                    string SceneName = SceneManager.GetActiveScene().name;
                    if (SceneName.Contains("SelectRole"))
                    {

                    }
                    else if (SceneName == "Tutorial1" || SceneName == "Tutorial2" || SceneName == "Tutorial3")
                    {
                        Debug.LogError("a");
                        GameObject.Find("MonsterAnimator").GetComponent<Animator>().SetBool("Died", true);
                        for (int i = 0; i < 4; i++)
                        {
                            GamePad.SetVibration((PlayerIndex)i, 0, 0);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            GamePad.SetVibration((PlayerIndex)i, 0, 0);
                        }
                        GameManager.PlayTime = Time.time;
                        SceneManager.LoadScene("Died");
                    }
                }
                else
                {
                    HP = MaxHP;
                    lockedHPTimer = 0;
                }
            }
            else
            {
                float dis = Vector3.Distance(GameManager.players.GetChild(0).localPosition, GameManager.players.GetChild(1).localPosition);
                float hpUpRate;
                /*= (2f - dis) * 2;
                if (hpUpRate > 0 && Players.fightingTimer >= 5)
                {
                    hpUpRate *= 6;
                }
                */
                if (dis < 2f)
                {
                    /*if (Players.fightingTimer >= 5)
                    {
                        hpUpRate = 20;
                    }
                    else
                    {
                        hpUpRate = 8;
                    }*/
                    hpUpRate = 20;
                }
                else if (dis < 4.5f)
                {
                    hpUpRate = 0;
                }
                else
                {
                    hpUpRate = -6f;
                }
                HP += hpUpRate * Time.deltaTime;
                if (HP > MaxHP)
                {
                    HP = MaxHP;
                }
                recoveryRate();
                transform.GetChild(5).localScale = Vector3.one * (HP + 5) / 3;
                transform.GetChild(6).localScale = Vector3.one * (HP + 5) / 3;
                if ((lightRotateTimer += Time.deltaTime) >= lightRotateTimerStoper)
                {
                    lightRotateTimer = 0;
                    transform.GetChild(6).rotation = transform.GetChild(5).rotation;
                    transform.GetChild(5).rotation = Quaternion.Euler(Vector3.forward * Random.Range(0, 360));
                    transform.GetChild(6).GetChild(0).GetComponent<Light2D>().lightCookieSprite = transform.GetChild(5).GetChild(0).GetComponent<Light2D>().lightCookieSprite;
                    transform.GetChild(5).GetChild(0).GetComponent<Light2D>().lightCookieSprite = lightSprites[Random.Range(0, lightSprites.Length)];
                }
                float transition = lightRotateTimer / lightRotateTimerStoper, brightness = HP / MaxHP;
                brightness *= GameManager.Gammar;

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
                reStatUI.gameObject.SetActive(Players.reTimer < 1);
                sleepingStatUI.gameObject.SetActive(SleepTimer<0);
                confusionStatUI.gameObject.SetActive(ConfusionTimer < 10);
                stickStatUI.gameObject.SetActive(StickTimer < 10);

                transform.GetChild(5).GetChild(0).GetComponent<Light2D>().intensity = transition * brightness;
                transform.GetChild(6).GetChild(0).GetComponent<Light2D>().intensity = (1 - transition) * brightness;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                transform.GetChild(7).GetComponent<Light2D>().pointLightOuterRadius = brightness * 4f;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            }
            timer();
        }

        private void LateUpdate()
        {
            if (!IntoHole)
            {
                nextPosBeforeIntoHole.Add(transform.position);
                nextPosBeforeIntoHoleTimer.Add(Time.time);
            }
            else
            {
                IntoHole = false;
                Debug.LogWarning("");
            }
            while (nextPosBeforeIntoHoleTimer[0] < Time.time - 0.3f && nextPosBeforeIntoHoleTimer.Count > 0)
            {
                nextPosBeforeIntoHole.RemoveAt(0);
                nextPosBeforeIntoHoleTimer.RemoveAt(0);
            }
        }

        void Behavior()
        {
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
                        if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy) +1 )))
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
                        if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy) + 1 )))
                        {
                            ConfusionTimer += 0.7f;
                            StickTimer += 0.7f;
                            lastDirRight = true;
                        }
                        break;
                }
            }
            if ((ConfusionTimer += Time.deltaTime) < 10 || (StickTimer += Time.deltaTime) < 10)
            {
                ConfusionUIRenderer.enabled = true;
                ConfusionUIcontroler.enabled = true;
            }
            else
            {
                ConfusionUIRenderer.enabled = false;
                ConfusionUIcontroler.enabled = false;
            }
            if ((ConfusionTimer += Time.deltaTime) < 10)
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
            if((HardStraightTimer+=Time.deltaTime) < 0.3f)
            {
                v = HardStraightA;
            }
            if ((SleepTimer += Time.deltaTime) < 0f)
            {
                v = Vector3.zero;
            }
            //玩家解除無敵狀態
            else if (HardStraightTimer > 0.5f)
            {
                gameObject.layer = 8;
            }

            #region//衝刺
            if (HardStraightTimer >= 0.3f && ConfusionTimer>= 10 && SleepTimer >= 0 && StickTimer >= 10)
            {
                if(DashTimer > DashCD)
                {
                    if (p1)
                    {
                        switch (SelectMouse.p1Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.J))
                                {
                                    DashA.x = Input.GetAxisRaw("HorizontalWASD");
                                    DashA.y = Input.GetAxisRaw("VerticalWASD");
                                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                                    if (DashA.magnitude > 10)
                                    {
                                        DashTimer = 0;
                                        LineAttack();
                                        insAfterImages.timer = 0;
                                    }
                                }
                                break;
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad1))
                                {
                                    DashA.x = Input.GetAxisRaw("HorizontalArrowKey");
                                    DashA.y = Input.GetAxisRaw("VerticalArrowKey");
                                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                                    if (DashA.magnitude > 10)
                                    {
                                        DashTimer = 0;
                                        LineAttack();
                                        insAfterImages.timer = 0;
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
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p1Joy))))
                                {
                                    DashA.x = Input.GetAxisRaw("HorizontalJoy" + SelectMouse.p1Joy);
                                    DashA.y = -Input.GetAxisRaw("VerticalJoy" + SelectMouse.p1Joy);
                                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                                    if (DashA.magnitude > 10)
                                    {
                                        DashTimer = 0;
                                        LineAttack();
                                        playerJoyVibration.DashVibration = 0.8f;
                                        insAfterImages.timer = 0;
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
                                if (Input.GetKeyDown(KeyCode.J))
                                {
                                    DashA.x = Input.GetAxisRaw("HorizontalWASD");
                                    DashA.y = Input.GetAxisRaw("VerticalWASD");
                                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                                    if (DashA.magnitude > 10)
                                    {
                                        DashTimer = 0;
                                        LineAttack();
                                        insAfterImages.timer = 0;
                                    }
                                }
                                break;
                            case "ArrowKey":
                                if (Input.GetKeyDown(KeyCode.Keypad1))
                                {
                                    DashA.x = Input.GetAxisRaw("HorizontalArrowKey");
                                    DashA.y = Input.GetAxisRaw("VerticalArrowKey");
                                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                                    if (DashA.magnitude > 10)
                                    {
                                        DashTimer = 0;
                                        LineAttack();
                                        insAfterImages.timer = 0;
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
                                if (Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectMouse.p2Joy))))
                                {
                                    DashA.x = Input.GetAxisRaw("HorizontalJoy" + SelectMouse.p2Joy);
                                    DashA.y = -Input.GetAxisRaw("VerticalJoy" + SelectMouse.p2Joy);
                                    DashA = Vector3.Normalize(DashA) * DashSpeed;
                                    if (DashA.magnitude > 10)
                                    {
                                        DashTimer = 0;
                                        LineAttack();
                                        playerJoyVibration.DashVibration = 0.8f;
                                        insAfterImages.timer = 0;
                                    }
                                }
                                break;
                        }
                    }
                }
                if ((DashTimer += Time.deltaTime) < 0.3f)
                {
                    v = DashA;
                }
            }
            #endregion

            #region//傳送
            if (AbilityManager.myAbilitys.Contains("按X傳送到隊友身邊(冷卻10秒)"))
            {
                homeButtonTimer += Time.deltaTime;

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
                                        homeButtonTimer = -10 * 2;
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
                                        homeButtonTimer = -10 * 2;
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
                                        homeButtonTimer = -10 * 2;
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
                        switch (SelectMouse.p2Joy)
                        {
                            case "WASD":
                                if (Input.GetKeyDown(KeyCode.M))
                                {
                                    if (homeButtonTimer > 0)
                                    {
                                        homeButtonTimer = -10 * 2;
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
                                        homeButtonTimer = -10 * 2;
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
                                        homeButtonTimer = -10 * 2;
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
            transform.GetChild(8).transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, v, Vector3.forward) - transform.GetChild(8).GetComponent <ParticleSystem>().shape.arc/2+180);

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

        private void FixedUpdate()
        {
            v *= 0.9f;
            if (HardStraightA.magnitude > 0.8f)
            {
                HardStraightA -= (Vector2)Vector3.Normalize(HardStraightA)*0.8f;
            }
            else
            {
                HardStraightA = Vector3.zero;
            }
            if (DashA.magnitude >0.5f)
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
            for(int i = 0; i < counted.Count; i++)
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
            if (collision.collider.GetComponent<MonsterManager>() && !collision.collider.GetComponent<Slime>())
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
                            HP -= 20 * (100f- reducesDamage )/ 100f;
                            HardStraightTimer = 0.1f;
                        }
                        else
                        {
                            HardStraightA = (Vector2)Vector3.Normalize(transform.position - collision.transform.position) * 10;
                            HP -= 10 * (100f - reducesDamage) / 100f;
                            HardStraightTimer = 0;
                        }
                    }
                    else
                    {
                        HardStraightA = (Vector2)Vector3.Normalize(transform.position - collision.transform.position) * 10;
                        HP -= 15 * (100f - reducesDamage) / 100f;
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
            if(collision.collider.GetComponent<MonsterManager>())
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
            PlayerAttackLine playerAttackLine = Instantiate(attackLine).GetComponent<PlayerAttackLine>();
            for (int i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                if (player == transform)
                {
                    playerAttackLine.startPlayer = player;
                }
                else
                {
                    playerAttackLine.endPlayer = player;
                }
            }
        }
    }
}