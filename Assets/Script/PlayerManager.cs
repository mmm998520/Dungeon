using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class PlayerManager : MonoBehaviour
    {
        public float MaxHP, HP, ATK, preparation, preparationTimer, hand;
        public float AttackMode, AttackModeTimer, CD, CDTimer;
        public GameObject attack;
        Vector3 lastPos;
        bool locked = true;
        float speed = 3;
        float touchTimer = 0;
        public GameObject attackPrefab;

        private void Start()
        {
            lastPos = transform.position;
        }

        void Update()
        {
            behavior();
            timer();
        }

        void behavior()
        {
            int i;
            int touchCount = 0;
            int touchNum = -1;
            for (i = 0; i < Input.touches.Length; i++)
            {
                Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 2));
                if (Input.touches[i].phase == TouchPhase.Began && locked)
                {
                    if (Vector3.Distance(touchPos, lastPos) < 0.5f)
                    {
                        touchCount++;
                        touchNum = i;
                    }
                }
                else if ((Input.touches[i].phase == TouchPhase.Moved || Input.touches[i].phase == TouchPhase.Stationary) && !locked)
                {
                    if (Vector3.Distance(touchPos, lastPos) < 1f)
                    {
                        touchCount++;
                        touchNum = i;
                    }
                }
            }
            if (touchCount != 1)
            {
                locked = true;
                lastPos = transform.position;
                //玩家手指超過區域則強制停止
                transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
            else
            {
                locked = false;
                Vector3 inputPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[touchNum].position.x, Input.touches[touchNum].position.y, 2));
                lastPos = inputPos;
                if (Input.touches[touchNum].phase == TouchPhase.Began)
                {
                    touchTimer = 0;
                }
                move(inputPos);
            }
        }

        void move(Vector3 inputPos)
        {
            if (Vector3.Distance(inputPos, transform.position) > speed * Time.deltaTime)
            {
                transform.GetComponent<Rigidbody2D>().velocity = (inputPos - transform.position).normalized * speed;
            }
            else
            {
                transform.GetComponent<Rigidbody2D>().velocity = (inputPos - transform.position).normalized * 0.1f;
            }
        }

        void timer()
        {
            if (locked)
            {
                if ((AttackModeTimer += Time.deltaTime) >= AttackMode)
                {
                    if((CDTimer += Time.deltaTime) >= CD)
                    {
                        CDTimer = 0;
                        Vector3 minDisPos = minDisMonster().position;
                        Quaternion quaternion = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, minDisPos * Vector2.one - transform.position * Vector2.one, Vector3.forward));
                        Instantiate(attack, transform.position, quaternion);
                    }
                }
            }
            else
            {
                AttackModeTimer = 0;
                CDTimer = 0;
            }
        }

        Transform minDisMonster()
        {
            int i;
            float minDis = float.MaxValue;
            Transform minDisMonster = null;
            Transform monsters = GameManager.monsters;
            for (i = 0; i < monsters.childCount; i++)
            {
                Transform monster = monsters.GetChild(i);
                if(monster.gameObject.activeSelf)
                {
                    float dis = Vector3.Distance(monster.position, transform.position);
                    if (minDis > dis)
                    {
                        minDis = dis;
                        minDisMonster = monster;
                    }
                }
            }
            return minDisMonster;
        }
    }
}