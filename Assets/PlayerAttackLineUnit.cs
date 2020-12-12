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
                            PlayerManager.HP += 10;
                            Players.reTimer = 0;
                            collider.GetComponent<MonsterManager>().beforeDied();
                            Destroy(collider.gameObject);
                        }
                    }
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            if (collider.GetComponent<Bubble>() || collider.GetComponent<MonsterShooter>())
            {
                Destroy(collider.gameObject);
            }
        }
    }
}