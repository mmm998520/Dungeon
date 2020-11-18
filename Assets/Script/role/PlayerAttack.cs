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
            if (collider.gameObject.layer == 9)
            {
                if (collider.GetComponent<MonsterManager>())
                {
                    if (collider.GetComponent<MonsterManager>().Armor <= 0)
                    {
                        Debug.LogWarning("hitTimes");
                        PlayerManager.HP += 5;
                        collider.GetComponent<MonsterManager>().beforeDied();
                        Destroy(collider.gameObject);
                    }
                }
            }
            if (collider.GetComponent<Bubble>() || collider.GetComponent<MonsterShooter>())
            {
                Destroy(collider.gameObject);
            }
        }
    }
}