using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttack : MonoBehaviour
    {
        void Update()
        {

        }
        
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 9 || collider.gameObject.layer == 11)
            {
                if (collider.GetComponent<MonsterManager>())
                {
                    if (collider.GetComponent<MonsterManager>().Armor <= 0)
                    {
                        Destroy(collider.gameObject);
                    }
                }
            }
        }
    }
}