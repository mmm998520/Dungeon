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
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            for(int i = 0; i < toDoLists.Length; i++)
            {
                toDo.Add((toDoLists[i].Minutes * 60) + toDoLists[i].seconds, toDoLists[i].willDo);
            }
            toDoLists = new ToDo.toDoList[0];
            anglePreSummoning = 360f / summoningTimes;
        }

        void Update()
        {
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
            throwAxe,
            Punch,
            Summoning
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