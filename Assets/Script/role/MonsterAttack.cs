using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterAttack : MonoBehaviour
    {
        public float ATK;
        public bool continued = false;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!continued)
            {
                if (collider.gameObject.layer == 8)
                {
                    PlayerManager.HP -= ATK;
                    print(collider.transform.GetChild(2).name);
                    collider.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (continued)
            {
                if (collider.gameObject.layer == 8)
                {
                    PlayerManager.HP -= ATK * Time.deltaTime;
                    print(collider.transform.GetChild(2).name);
                    collider.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }
    }
}