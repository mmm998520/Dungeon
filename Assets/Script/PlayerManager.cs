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
        public float lockedTimer;
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

        void OnTriggerStay2D(Collider2D collider)
        {
            if (CDTimer >= CD && (collider.gameObject.layer == 9 || collider.gameObject.layer == 11) && locked)
            {
                if (collider.GetComponent<MonsterManager>())
                {
                    CDTimer = 0;
                    PlayerAttack playerAttack = Instantiate(attack, transform.position, transform.GetChild(3).rotation).GetComponent<PlayerAttack>();
                    playerAttack.ATK = ATK;
                    playerAttack.continued = continued;
                    Destroy(playerAttack.gameObject, atkTime);
                }
            }
        }

        void timer()
        {
            CDTimer += Time.deltaTime;
            if (locked)
            {
                lockedTimer += Time.deltaTime;
            }
            else
            {
                lockedTimer = 0;
            }
        }

    }
}