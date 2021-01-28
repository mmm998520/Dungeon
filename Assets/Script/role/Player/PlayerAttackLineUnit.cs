using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttackLineUnit : MonoBehaviour
    {
        public Transform endPlayer;
        public PlayerAttackLine playerAttackLine;
        void Start()
        {

        }

        void Update()
        {
            float angle = Vector3.SignedAngle(transform.right, endPlayer.position - transform.position, Vector3.forward);
            transform.Rotate(0, 0, angle * 1f);
            transform.Translate(Vector3.right * Time.deltaTime * 40);
            if (Vector3.Distance(transform.position, endPlayer.position) < 1f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            if (!playerAttackLine.attackedColliders.Contains(collider))
            {
                if (collider.gameObject.layer == 9)
                {
                    if (collider.GetComponent<MonsterManager>())
                    {
                        playerAttackLine.attackedColliders.Add(collider);
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
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
            {
                Destroy(collider.gameObject);
            }
        }
    }
}