using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttackLineUnit : MonoBehaviour
    {
        public Transform endPlayer;
        public PlayerAttackLine playerAttackLine;
        public GameObject reLifeParticle, money;
        public static int hpRecover = 0;
        void Start()
        {

        }

        void Update()
        {
            float angle = Vector3.SignedAngle(transform.right, endPlayer.position - transform.position, Vector3.forward);
            transform.Rotate(0, 0, angle * 1f);
            transform.Translate(Vector3.right * Time.deltaTime * 40);
            if (Vector3.Distance(transform.position, endPlayer.position) < 0.35f)
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
                        }
                        print(collider.gameObject.name);
                        if (collider.GetComponent<MonsterManager>().HP <= 0)
                        {
                            Debug.LogWarning("hitTimes");
                            PlayerManager.HP += hpRecover;
                            Players.reTimer = 0;
                            collider.GetComponent<MonsterManager>().beforeDied();
                            Destroy(collider.gameObject);
                            if (collider.name.Contains("Big"))
                            {
                                insMoney(Random.Range(3, 6));
                                if (Random.Range(0, 100) < 15)
                                {
                                    Instantiate(reLifeParticle, transform.position, Quaternion.identity);
                                }
                            }
                            else if (collider.name.Contains("Spider"))
                            {
                                insMoney(Random.Range(1, 2));
                                if (Random.Range(0, 100) < 1)
                                {
                                    Instantiate(reLifeParticle, transform.position, Quaternion.identity);
                                }
                            }
                            else
                            {
                                insMoney(Random.Range(1, 2));
                                if (Random.Range(0, 100) < 1)
                                {
                                    Instantiate(reLifeParticle, transform.position, Quaternion.identity);
                                }
                            }
                        }
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack))
            {
                Destroy(collider.gameObject);
            }
        }

        void insMoney(int times)
        {
            for(int i = 0; i < times; i++)
            {
                Instantiate(money, transform.position, Quaternion.identity);
            }
        }
    }
}