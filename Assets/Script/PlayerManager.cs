using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class PlayerManager : MonoBehaviour
    {
        public float MaxHP, HP, ATK, hand, atkTime;
        public bool continued = false;
        public float CD, CDTimer;
        public GameObject attack;
        Vector3 lastPos;
        bool locked = true;
        public float speed = 3;
        Transform hp;

        private void Start()
        {
            lastPos = transform.position;
            hp = transform.GetChild(0);
        }

        void Update()
        {
            hp.localScale = new Vector3(HP / MaxHP, hp.localScale.y, hp.localScale.z);
            behavior();
            if (HP <= 0)
            {
                SceneManager.LoadScene("Died");
            }
            CDTimer += Time.deltaTime;
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
                move(inputPos);
            }
        }

        void move(Vector3 inputPos)
        {
            if (Vector3.Distance(inputPos, transform.position) > speed * Time.deltaTime)
            {
                transform.GetComponent<Rigidbody2D>().velocity = (inputPos - transform.position).normalized * speed;
                Quaternion quaternion = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, (inputPos - transform.position), Vector3.forward));
                transform.GetChild(3).localRotation = quaternion;
            }
            else
            {
                transform.GetComponent<Rigidbody2D>().velocity = (inputPos - transform.position).normalized * 0.1f;
                Quaternion quaternion = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, (inputPos - transform.position), Vector3.forward));
                transform.GetChild(3).localRotation = quaternion;
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

        void OnTriggerStay2D(Collider2D collider)
        {
            if (CDTimer >= CD)
            {
                CDTimer = 0;
                if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
                {
                    if (collider.GetComponent<MonsterManager>())
                    {
                        PlayerAttack playerAttack = Instantiate(attack, transform.position, transform.GetChild(3).rotation).GetComponent<PlayerAttack>();
                        playerAttack.ATK = ATK;
                        playerAttack.continued = continued;
                        Destroy(playerAttack.gameObject, atkTime);
                    }
                }
            }
        }
    }
}