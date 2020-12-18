using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace com.DungeonPad
{
    public class TaurenBoss : MonsterManager
    {
        public bool attacking = false;
        Transform minDisPlayer;
        public Text HPText;
        public float InvincibleTimer = 10;//無敵
        public Animator animator;
        public Vector3 RecordDir;

        bool throwByClockwise;
        float playerAngle;
        public GameObject Axe, accurateAxe;

        void Start()
        {
            speed = 3;
            rigidbody = GetComponent<Rigidbody2D>();
            addCanGoByHand();
            endRow = new int[1];
            endCol = new int[1];
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            InvincibleTimer += Time.deltaTime;
            HPText.text = HP + "";
            minDisPlayer = MinDisPlayer();
            if (Vector3.Distance(minDisPlayer.position, transform.position) > 5 && !attacking)
            {
                resetRoad();
                move();
            }
            else if(!animator.GetBool("Punch") && !animator.GetBool("ThrowAxe") && !animator.GetBool("AccurateAxe") && (CDTimer += Time.deltaTime) > CD)
            {
                int r = Random.Range(0, 3);
                print(r);
                if (r < -1)
                {
                    animator.SetBool("Punch", true);
                }
                else if (r < -2)
                {
                    throwByClockwise = (Random.Range(0, 2) > 0);
                    animator.SetBool("ThrowAxe", true);
                    playerAngle = Vector2.SignedAngle(Vector2.right, minDisPlayer.position - transform.position);
                }
                else
                {
                    animator.SetBool("AccurateAxe", true);
                }
                attacking = true;
            }
        }

        void reSpeed()
        {
            rigidbody.velocity = Vector3.zero;
        }

        #region//Punch
        void Record()
        {
            RecordDir = Vector3.Normalize(minDisPlayer.position - transform.position);
        }

        void prePunch()
        {
            Record();
            rigidbody.velocity = Vector3.zero;
        }

        void punch()
        {
            InvincibleTimer = 0;
            rigidbody.velocity = RecordDir * 15;
        }

        void endPunch()
        {
            animator.SetBool("Punch", false);
            CDTimer = 0;
        }
        #endregion

        #region//ThrowAxe
        void ThrowAxe(float angle)
        {
            if (!throwByClockwise)
            {
                angle *= -1;
            }
            GameObject temp = Instantiate(Axe, transform.position, Quaternion.Euler(0, 0, angle + playerAngle));
            Debug.LogErrorFormat(temp.name, temp);
        }

        void endThrowAxe()
        {
            animator.SetBool("ThrowAxe", false);
            CDTimer = 0;
        }
        #endregion

        #region//AccurateAxe
        void AccurateAxe()
        {
            for(int i = 0; i < GameManager.players.childCount; i++)
            {
                Transform target = GameManager.players.GetChild(i);
                Instantiate(accurateAxe, target.position, Quaternion.identity).GetComponent<AccurateAxe>().target = target;
            }
        }

        void endAccurateAxe()
        {
            animator.SetBool("AccurateAxe", false);
            CDTimer = 0;
        }
        #endregion

        void resetRoad()
        {
            endRow[0] = Mathf.RoundToInt(minDisPlayer.position.x);
            endCol[0] = Mathf.RoundToInt(minDisPlayer.position.y);
            findRoad();
        }

        void move()
        {
            Vector3 nextPos = new Vector3(roads[roads.Count - 2][0], roads[roads.Count - 2][1]);
            rigidbody.velocity = Vector3.Normalize(nextPos - transform.position) * speed;
            if (Vector3.Distance(transform.position, nextPos) < 0.5f)
            {
                roads.RemoveAt(roads.Count - 1);
            }
            if (rigidbody.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (rigidbody.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
}