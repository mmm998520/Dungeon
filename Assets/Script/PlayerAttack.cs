using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttack : MonoBehaviour
    {
        public float ATK;
        public bool continued = false;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!continued)
            {
                if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
                {
                    if (collider.GetComponent<MonsterManager>())
                    {
                        collider.GetComponent<MonsterManager>().HP -= ATK;
                        collider.transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (continued)
            {
                if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
                {
                    if (collider.GetComponent<MonsterManager>())
                    {
                        collider.GetComponent<MonsterManager>().HP -= ATK * Time.deltaTime;
                        collider.transform.GetChild(3).gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}