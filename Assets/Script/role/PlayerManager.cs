using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class PlayerManager : MonoBehaviour
    {
        public static float MaxHP = 40, HP = 40;
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

        public bool WASD;
        Vector2 v = Vector2.zero;

        public float StickTimer = 10;
        private void Start()
        {
            lastPos = transform.position;
            hp = transform.GetChild(0);
        }

        void Update()
        {
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);
            ComputerBehavior();
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
                transform.GetChild(5).GetComponent<Light>().spotAngle = HP + 10;
                transform.GetChild(5).GetComponent<Light>().intensity = HP + 10;
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

        void ComputerBehavior()
        {
            if (WASD)
            {
                v.x += Input.GetAxis("HorizontalWASD") * 2;
                v.y += Input.GetAxis("VerticalWASD") * 2;

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
            }
            else
            {
                v.x += Input.GetAxis("Horizontal") * 2;
                v.y += Input.GetAxis("Vertical") * 2;
                if ((StickTimer += Time.deltaTime) < 5)
                {
                    if (v.magnitude > 1f)
                    {
                        v = v.normalized * 1f;
                    }
                }
                else if (v.magnitude > 3)
                {
                    v = v.normalized * 3;
                }
                GetComponent<Rigidbody2D>().velocity = v;
            }
        }

        private void FixedUpdate()
        {
            v *= 0.93f;
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