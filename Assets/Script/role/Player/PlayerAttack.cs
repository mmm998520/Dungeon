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
                    if(!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                    {
                        collider.GetComponent<MonsterManager>().HP -= 1;
                    }
                    print(collider.gameObject.name);
                    if(collider.GetComponent<MonsterManager>().HP <= 0)
                    {
                        Debug.LogWarning("hitTimes");
                        PlayerManager.HP += 10;
                        Players.reTimer = 0;
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