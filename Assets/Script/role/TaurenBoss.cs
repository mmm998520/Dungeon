using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TaurenBoss : MonsterManager
    {
        [SerializeField]
        private ToDo.toDoList[] toDoLists;
        private Dictionary<float, ToDo.CanDo> toDo = new Dictionary<float, ToDo.CanDo>();

        Rigidbody2D rigidbody;
        public GameObject Axe, Tauren;
        public Transform InsAxePos;
        public bool canWalk = true, punching = false;
        public int summoningTimes;
        float anglePreSummoning, currentAngle;
        Animator animator;

        float timer;
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
            for(int i = 0; i < toDoLists.Length; i++)
            {
                toDo.Add((toDoLists[i].Minutes * 60) + toDoLists[i].seconds, toDoLists[i].willDo);
            }
            anglePreSummoning = 360f / summoningTimes;
            ArmorBar = transform.GetChild(3);
        }

        void Update()
        {
            int t = (int)(timer += Time.deltaTime);
            if (toDo.ContainsKey(t))
            {
                if (toDo[t] == ToDo.CanDo.Punch)
                {
                    animator.SetTrigger("Punch");
                    toDo.Remove(t);
                }
                else if (toDo[t] == ToDo.CanDo.Summoning)
                {
                    animator.SetTrigger("Summoning");
                    toDo.Remove(t);
                }
                else if (toDo[t] == ToDo.CanDo.ThrowAxe)
                {
                    animator.SetTrigger("ThrowAxe");
                    toDo.Remove(t);
                }
                else if (toDo[t] == ToDo.CanDo.Reset)
                {
                    timer = 0;
                    toDo.Clear();
                    for (int i = 0; i < toDoLists.Length; i++)
                    {
                        toDo.Add((toDoLists[i].Minutes * 60) + toDoLists[i].seconds, toDoLists[i].willDo);
                    }
                }
            }
            if (canWalk)
            {
                rigidbody.velocity = Vector3.Normalize((MinDisPlayer().position - transform.position) * Vector2.one);
                if (rigidbody.velocity.x > 0)
                {
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                }
                if (rigidbody.velocity.x < 0)
                {
                    transform.rotation = Quaternion.Euler(Vector3.up * 180);
                }
            }
            else if (punching)
            {
                rigidbody.velocity = transform.right * 5;
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }

            ArmorBar.gameObject.SetActive(Armor > 0);
            ArmorBar.localScale = Vector3.one * ((Armor / MaxArmor) * 0.6f + 0.4f);
        }

        /// <summary> 召喚術 </summary>
        public void Summoning()
        {
            Instantiate(Tauren, transform.position + (Quaternion.Euler(Vector3.forward * currentAngle) * Vector3.up * 2), Quaternion.identity);
            currentAngle += anglePreSummoning;
        }

        /// <summary> 丟斧頭 </summary>
        public void throwAxe()
        {
            float angle = Vector3.SignedAngle(Vector3.right, (MinDisPlayer().position - transform.position) * Vector2.one, Vector3.forward);
            Instantiate(Axe, InsAxePos.position, Quaternion.Euler(Vector3.forward * angle));
        }

        public void useArmor(int cost)
        {
            Armor -= cost;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<PlayerManager>())
            {
                collision.GetComponent<PlayerManager>().a += (Vector2)Vector3.Normalize((collision.transform.position - transform.position) * Vector2.one);
                print("player");
            }
        }
    }

    /// <summary> 儲存資料用 </summary>
    [System.Serializable]
    public class ToDo
    {
        public enum CanDo
        {
            ThrowAxe,
            Punch,
            Summoning,
            Reset
        }

        /// <summary> 儲存資料用 </summary>
        [System.Serializable]
        public struct toDoList
        {
            public float Minutes, seconds;
            public CanDo willDo;
        }
    }
}