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
                    collider.GetComponent<PlayerManager>().HP -= ATK;
                    print("a");
                }
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (continued)
            {
                if (collider.gameObject.layer == 8)
                {
                    collider.GetComponent<PlayerManager>().HP -= ATK * Time.deltaTime;
                }
            }
        }
    }
}