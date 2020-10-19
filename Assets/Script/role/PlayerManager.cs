using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class PlayerManager : MonoBehaviour
    {
        public static float MaxHP = 40, HP = 40;
        public static bool lockedHP = false;
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
        Vector2 v = Vector2.zero;

        public float StickTimer = 10;
        public float ConfusionTimer = 100;
        public SpriteRenderer ConfusionUIRenderer;
        public ConfusionUIcontroler ConfusionUIcontroler;
        private void Start()
        {
            lastPos = transform.position;
            hp = transform.GetChild(0);
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
                HP += (1.5f - dis) * Time.deltaTime * 2;
                if (HP > MaxHP)
                {
                    HP = MaxHP;
                }
                transform.GetChild(5).GetComponent<Light>().spotAngle = HP + 5;
                transform.GetChild(5).GetComponent<Light>().intensity = HP + 5;
                if (StickTimer < 5)
                {
                    transform.GetChild(4).gameObject.SetActive(true);
                    if ((int)(StickTimer * 10) % 6 < 1)
                    {
                        transform.GetChild(5).GetComponent<Light>().intensity -= 50;
                    }
                    else if ((int)(StickTimer * 10) % 6 < 2)
                    {
                        transform.GetChild(5).GetComponent<Light>().intensity -= 40;
                    }
                    else if ((int)(StickTimer * 10) % 6 < 3)
                    {
                        transform.GetChild(5).GetComponent<Light>().intensity -= 30;
                    }
                    else if ((int)(StickTimer * 10) % 6 < 4)
                    {
                        transform.GetChild(5).GetComponent<Light>().intensity -= 20;
                    }
                    else if ((int)(StickTimer * 10) % 6 < 5)
                    {
                        transform.GetChild(5).GetComponent<Light>().intensity -= 10;
                    }
                    else if ((int)(StickTimer * 10) % 6 < 6)
                    {
                        transform.GetChild(5).GetComponent<Light>().intensity -= 0;
                    }
                    transform.parent.GetChild(0).GetChild(5).GetComponent<Light>().intensity = 0;
                    transform.parent.GetChild(1).GetChild(5).GetComponent<Light>().intensity = 0;
                }
                else
                {
                    transform.GetChild(4).gameObject.SetActive(false);
                }
            }
            timer();
        }

        void Behavior()
        {
            if (p1)
            {
                if (!lastDirRight)
                {
                    switch (test.p1Joy)
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
                            if (Input.GetAxis("HorizontalJoy" + test.p1Joy) < 0.5f)
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                    }
                }
                else
                {
                    switch (test.p1Joy)
                    {
                        case "WASD":
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                        case "ArrowKey":
                            if (Input.GetKeyDown(KeyCode.LeftArrow))
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
                            if (Input.GetAxis("HorizontalJoy" + test.p1Joy) > -0.5f)
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (!lastDirRight)
                {
                    switch (test.p2Joy)
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
                            if (Input.GetAxis("HorizontalJoy" + test.p2Joy) < 0.5f)
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                    }
                }
                else
                {
                    switch (test.p2Joy)
                    {
                        case "WASD":
                            if (Input.GetKeyDown(KeyCode.A))
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
                            }
                            break;
                        case "ArrowKey":
                            if (Input.GetKeyDown(KeyCode.LeftArrow))
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
                            if (Input.GetAxis("HorizontalJoy" + test.p2Joy) > -0.5f)
                            {
                                ConfusionTimer++;
                                lastDirRight = true;
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
                    switch (test.p1Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * 2;
                            v.y += Input.GetAxis("VerticalWASD") * 2;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("Horizontal") * 2;
                            v.y += Input.GetAxis("Vertical") * 2;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoy" + test.p1Joy) * 2;
                            v.y -= Input.GetAxis("VerticalJoy" + test.p1Joy) * 2;
                            break;
                    }
                }
                else
                {
                    switch (test.p2Joy)
                    {
                        case "WASD":
                            v.x += Input.GetAxis("HorizontalWASD") * 2;
                            v.y += Input.GetAxis("VerticalWASD") * 2;
                            break;
                        case "ArrowKey":
                            v.x += Input.GetAxis("Horizontal") * 2;
                            v.y += Input.GetAxis("Vertical") * 2;
                            break;
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                            v.x += Input.GetAxis("HorizontalJoy" + test.p2Joy) * 2;
                            v.y -= Input.GetAxis("VerticalJoy" + test.p2Joy) * 2;
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
            GetComponent<Rigidbody2D>().velocity = v;
            transform.GetChild(7).transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, v, Vector3.forward) - transform.GetChild(7).GetComponent <ParticleSystem>().shape.arc/2+180);
        }

        private void FixedUpdate()
        {
            v *= 0.9f;
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
    }
}