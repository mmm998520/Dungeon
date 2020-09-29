using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttack : MonoBehaviour
    {
        public float ATK;
        public bool continued = false, lineAttack = false;
        List<MonsterManager> monsters = new List<MonsterManager>();
        void Update()
        {
            for(int i = 0; i < monsters.Count; i++)
            {
                print("e");
                monsters[i].HP -= ATK * Time.deltaTime;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!continued)
            {
                if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
                {
                    if (collider.GetComponent<MonsterManager>())
                    {
                        if (lineAttack)
                        {
                            collider.GetComponent<MonsterManager>().HP -= ATK * Time.deltaTime;
                        }
                        else
                        {
                            collider.GetComponent<MonsterManager>().HP -= ATK;
                        }
                        collider.transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
            }
            else
            {
                if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
                {
                    if (collider.GetComponent<MonsterManager>())
                    {
                        if (!monsters.Contains(collider.GetComponent<MonsterManager>()))
                        {
                            monsters.Add(collider.GetComponent<MonsterManager>());
                        }
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (continued)
            {
                if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
                {
                    if (collider.GetComponent<MonsterManager>())
                    {
                        monsters.Remove(collider.GetComponent<MonsterManager>());
                    }
                }
            }
        }
    }
}