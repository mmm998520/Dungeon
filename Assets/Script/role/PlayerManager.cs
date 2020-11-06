using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

namespace com.DungeonPad
{
    public class PlayerManager : MonoBehaviour
    {
        public static float MaxHP = 40, HP = 40;
        public static bool lockedHP = true;
        public float ATK, hand, atkTime;
        public bool continued = false;
        public float CD, CDTimer;
        public GameObject attack, regener, ridiculeWind;
        Vector3 lastPos;
        public bool locked = true, flash = false;
        public float beganTouchedTimer, flashTimer, flashTimerStoper;
        public float speed = 3;
        Transform hp;
        public List<Vector3> startRayPoss;

        public bool p1;
        public bool lastDirRight;
        public Vector2 v = Vector2.zero, HardStraightA = Vector2.zero, DashA = Vector2.zero;

        PlayerJoyVibration playerJoyVibration;

        public float StickTimer = 10, HardStraightTimer = 10, DashTimer = 10;
        public float ConfusionTimer = 100;
        public SpriteRenderer ConfusionUIRenderer;
        public ConfusionUIcontroler ConfusionUIcontroler;

        float lastUpdateHp;
        List<float> timerRecord = new List<float>(), recoveryRecord = new List<float>();
        public float lightRotateTimer, lightRotateTimerStoper;
        public Sprite[] lightSprites;
        private void Start()
        {
            playerJoyVibration = GetComponent<PlayerJoyVibration>();
            lastPos = transform.position;
            hp = transform.GetChild(0);
            lastUpdateHp = HP;
        }

        void Update()
        {
            if (lockedHP)
            {
                HP = 40;
            }
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);
            Behavior();
            if (HP <= 0)
            {
                SceneManager.LoadScene("Died");
            }
            else
            {
                float dis = Vector3.Distance(GameManager.players.GetChild(0).localPosition, GameManager.players.GetChild(1).localPosition);
                HP += (2.5f - dis) * Time.deltaTime * 2;
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

                if (StickTimer < 5)
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
                transform.GetChild(4).gameObject.SetActive(StickTimer < 5);

                transform.GetChild(5).GetChild(0).GetComponent<Light2D>().intensity = transition * brightness;
                transform.GetChild(6).GetChild(0).GetComponent<Light2D>().intensity = (1 - transition) * brightness;
            }
            timer();
        }

        void Behavior()
        {
            if (p1)
            {
                if (!lastDirRight)
                {
                    switch (SelectRole.p1Joy)
                    {
                        case "WASD":
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                        case "ArrowKey":
                            if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                ConfusionTimer++;
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
                            if (Input.GetAxis("HorizontalJoy" + SelectRole.p1Joy) > 0.8f)
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                    }
                }
                else
                {
                    switch (SelectRole.p1Joy)
                    {
                        case "WASD":
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                ConfusionTimer++;
                                lastDirRight = false;
                            }
                            break;
                        case "ArrowKey":
                            if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                ConfusionTimer++;
                                lastDirRight = false;
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
                            if (Input.GetAxis("HorizontalJoy" + SelectRole.p1Joy) < -0.8f)
                            {
                                ConfusionTimer++;
                                lastDirRight = false;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (!lastDirRight)
                {
                    switch (SelectRole.p2Joy)
                    {
                        case "WASD":
                            if (Input.GetKeyDown(KeyCode.D))
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                        case "ArrowKey":
                            if (Input.GetKeyDown(KeyCode.RightArrow))
                            {
                                ConfusionTimer++;
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
                            if (Input.GetAxis("HorizontalJoy" + SelectRole.p2Joy) > 0.8f)
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                    }
                }
                else
                {
                    switch (SelectRole.p2Joy)
                    {
                        case "WASD":
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                ConfusionTimer++;
                                lastDirRight = false;
                            }
                            break;
                        case "ArrowKey":
                            if (Input.GetKeyDown(KeyCode.LeftArrow))
                            {
                                ConfusionTimer++;
                                lastDirRight = false;
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
                            if (Input.GetAxis("HorizontalJoy" + SelectRole.p2Joy) < -0.8f)
                            {
                                ConfusionTimer++;
                                lastDirRight = false;
                            }
                            break;
                    }
                }
            }
            if ((ConfusionTimer+=Time.deltaTime) <= 10)
            {
                ConfusionUIRenderer.enabled = true;
                ConfusionUIcontroler.enabled = true;
                float tempVX = v.x + Random.Range(-6f, 6f);
                float tempVY = v.y + Random.Range(-6f, 6f);

                v.x += (tempVX + v.x) / 3;
                v.y += (tempVY + v.y) / 3;
            }
            else
            {
                ConfusionUIRenderer.enabled = false;
                ConfusionUIcontroler.enabled = false;
                #region//操作移動
                if (p1)
                {
                    switch (SelectRole.p1Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * 2;
                            v.y += Input.GetAxis("VerticalWASD") * 2;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("HorizontalArrowKey") * 2;
                            v.y += Input.GetAxis("VerticalArrowKey") * 2;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoy" + SelectRole.p1Joy) * 2;
                            v.y -= Input.GetAxis("VerticalJoy" + SelectRole.p1Joy) * 2;
                            break;
                    }
                }
                else
                {
                    switch (SelectRole.p2Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * 2;
                            v.y += Input.GetAxis("VerticalWASD") * 2;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("HorizontalArrowKey") * 2;
                            v.y += Input.GetAxis("VerticalArrowKey") * 2;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoy" + SelectRole.p2Joy) * 2;
                            v.y -= Input.GetAxis("VerticalJoy" + SelectRole.p2Joy) * 2;
                            break;
                    }
                }
                #endregion
            }
            if ((StickTimer += Time.deltaTime) < 5)
            {
                if (v.magnitude > 0.5f)
                {
                    v = v.normalized * 0.5f;
                }
            }
            else if (v.magnitude > 3)
            {
                v = v.normalized * 3;
            }
            if((HardStraightTimer+=Time.deltaTime) < 0.3f)
            {
                v = HardStraightA;
            }
            if ((Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectRole.p1Joy) + 1)) && p1))
            {
                DashA.x = Input.GetAxis("HorizontalJoy" + SelectRole.p1Joy);
                DashA.y = Input.GetAxis("VerticalJoy" + SelectRole.p1Joy);
                DashA = Vector3.Normalize(DashA) *15;
                DashTimer = 0;
            }
            if(Input.GetKeyDown((KeyCode)(330 + 20 * int.Parse(SelectRole.p2Joy) + 1)) && !p1)
            {
                DashA.x = Input.GetAxis("HorizontalJoy" + SelectRole.p2Joy);
                DashA.y = Input.GetAxis("VerticalJoy" + SelectRole.p2Joy);
                DashA = Vector3.Normalize(DashA) * 15;
                DashTimer = 0;
            }
            if ((DashTimer += Time.deltaTime) < 0.3f)
            {
                v = DashA;
                gameObject.layer = 16;
            }
            else
            {
                gameObject.layer = 8;
            }
            GetComponent<Rigidbody2D>().velocity = v;
            transform.GetChild(8).transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, v, Vector3.forward) - transform.GetChild(8).GetComponent <ParticleSystem>().shape.arc/2+180);
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
            if (DashA.magnitude > 1.2f)
            {
                DashA -= (Vector2)Vector3.Normalize(HardStraightA) * 1.2f;
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

        float countAverage(List<float> counted)
        {
            float total = 0;
            for(int i = 0; i < counted.Count; i++)
            {
                total += counted[i];
            }
            return total / counted.Count;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<MonsterManager>() && !collision.collider.GetComponent<Slime>())
            {
                if (HardStraightTimer > 0.3f)
                {
                    HardStraightTimer = 0;
                    HardStraightA = (Vector2)Vector3.Normalize(transform.position - collision.transform.position) * 10;
                    HP -= 5;
                    playerJoyVibration.hurt();
                }
            }
        }
    }
}