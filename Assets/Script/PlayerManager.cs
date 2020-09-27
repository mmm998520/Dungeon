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
        public List<Vector3> startRayPos;

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
            GetComponent<Rigidbody2D>().WakeUp();

            RaycastHit2D? hit = Hit();
            if (hit.HasValue)
            {
                print(transform.position);
                print(hit.Value.point);
                print(Vector3.Distance(transform.position, hit.Value.point));
                print((Vector3.Distance(transform.position, hit.Value.point) / 4.45f));
                transform.GetChild(3).GetChild(0).GetChild(0).localScale = new Vector3(1 - (Vector3.Distance(transform.position * Vector2.one, hit.Value.point * Vector2.one) / 4.45f), 1, 1);
                if (hit.Value.collider.GetComponent<MonsterManager>())
                {
                    Attack();
                }
            }
            else if(transform.GetChild(3).GetChild(0).childCount != 0)
            {
                transform.GetChild(3).GetChild(0).GetChild(0).localScale = new Vector3(0, 1, 1);
            }
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
            if (Vector3.Distance(inputPos, transform.position) > speed * Time.deltaTime*8)
            {
                transform.GetComponent<Rigidbody2D>().velocity = (inputPos - transform.position).normalized * speed;
                Quaternion quaternion = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, (inputPos - transform.position), Vector3.forward));
                transform.GetChild(3).localRotation = quaternion;
            }
            else if (Vector3.Distance(inputPos, transform.position) > speed * Time.deltaTime*3)
            {
                float a = Vector3.Distance(inputPos, transform.position);
                
                transform.GetComponent<Rigidbody2D>().velocity = (inputPos - transform.position).normalized * speed* (a / speed / Time.deltaTime)*0.1f;
                Quaternion quaternion = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, (inputPos - transform.position), Vector3.forward));
                transform.GetChild(3).localRotation = quaternion;
            }
            else
            {
                transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                Quaternion quaternion = Quaternion.Euler(0, 0, Vector3.SignedAngle(Vector3.right, (inputPos - transform.position), Vector3.forward));
                transform.GetChild(3).localRotation = quaternion;
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
            {
                if (collider.GetComponent<MonsterManager>())
                {
                    Attack();
                }
            }
        }

        void Attack()
        {
            if(CDTimer >= CD && locked)
            {
                CDTimer = 0;
                PlayerAttack playerAttack = Instantiate(attack, transform.position, transform.GetChild(3).rotation).GetComponent<PlayerAttack>();
                playerAttack.ATK = ATK;
                playerAttack.continued = continued;
                Destroy(playerAttack.gameObject, atkTime);
                print(atkTime+","+CD);
            }
        }

        /// <summary> 從玩家身上打出射線看何處命中(命中後將mask拉伸到命中點)，如果是戰士等不會有設限問題的攻擊則直接返回null不多加處理 </summary>
        RaycastHit2D? Hit()
        {
            RaycastHit2D? Hit = null;
            int i;
            for (i = 0; i < startRayPos.Count; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position + startRayPos[i], Quaternion.Euler(0, 0, transform.GetChild(3).rotation.z) * Vector3.right, hand, 1 << 9 | 1 << 10 | 1 << 11 | 1 << 12);
                Debug.DrawRay(transform.position + startRayPos[i], Quaternion.Euler(0, 0, transform.GetChild(3).rotation.z) * Vector3.right);
                if (hit)
                {
                    if (!Hit.HasValue)
                    {
                        Hit = hit;
                    }
                    else if (Vector3.Distance((transform.position + startRayPos[i]) * Vector2.one, hit.point) < Vector3.Distance((transform.position + startRayPos[i]) * Vector2.one, Hit.Value.point))
                    {
                        Hit = hit;
                    }
                }
            }
            return Hit;
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