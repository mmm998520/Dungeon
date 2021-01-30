using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttackLine : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }


        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 9)
            {
                if (collider.GetComponent<MonsterManager>())
                {
                    if (!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                    {
                        collider.GetComponent<MonsterManager>().HP -= 1;
                        if (Random.Range(0, 100) < PlayerManager.criticalRate)
                        {
                            collider.GetComponent<MonsterManager>().HP -= 1;
                        }
                    }
                    print(collider.gameObject.name);
                    if (collider.GetComponent<MonsterManager>().HP <= 0)
                    {
                        collider.GetComponent<MonsterManager>().beforeDied();
                        Debug.LogWarning("hitTimes");
                    }
                }
            }

            if (collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
            {
                Destroy(collider.gameObject);
            }
        }
    }
}