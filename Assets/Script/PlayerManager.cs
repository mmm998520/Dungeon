using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.Dungeon
{
    public class PlayerManager : MonoBehaviour
    {
        Vector3 lastPos;
        bool locked = true;
        float speed = 3;
        bool attackMode = false;
        float touchTimer = 0;
        public GameObject attackPrefab;

        private void Start()
        {
            lastPos = transform.position;
        }

        void Update()
        {
            timer();
            behavior();
        }

        void behavior()
        {
            int i;
            int touchCount = 0;
            int touchNum = -1;
            for (i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].phase == TouchPhase.Began && locked)
                {
                    if (Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 2)), lastPos) < 0.3f)
                    {
                        touchCount++;
                        touchNum = i;
                    }
                }
                else if ((Input.touches[i].phase == TouchPhase.Moved || Input.touches[i].phase == TouchPhase.Stationary) && !locked)
                {
                    if (Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(Input.touches[i].position.x, Input.touches[i].position.y, 2)), lastPos) < 1f)
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
                    if(touchTimer < 0.5f)
                    {
                        attackMode = true;
                    }
                    touchTimer = 0;
                }
                move(inputPos);
                if (attackMode)
                {
                    attackPreparation(inputPos);
                }
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

        void attackPreparation(Vector3 inputPos)
        {
            Vector3 dir = inputPos - transform.position;
            if (dir.sqrMagnitude > 0.7f)
            {
                attack(dir);
            }
        }

        void attack(Vector3 dir)
        {
            attackMode = false;
            Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
            Instantiate(attackPrefab, transform.position, Quaternion.identity);
        }

        void timer()
        {
            touchTimer += Time.deltaTime;
        }
    }
}