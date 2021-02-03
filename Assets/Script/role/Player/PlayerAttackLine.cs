using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttackLine : MonoBehaviour
    {
        public Transform rayPos1, rayPos2;
        public bool hit = false;
        HashSet<MonsterManager> monsterManagers = new HashSet<MonsterManager>();
        void Start()
        {

        }

        void Update()
        {
            if (hit)
            {
                Hit();
            }
        }

        void clearMonsterManagers()
        {
            monsterManagers.Clear();
        }

        void Hit()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayPos1.position, rayPos2.position - rayPos1.position, (rayPos2.position - rayPos1.position).magnitude);
            Collider2D collider;
            for (int i = 0; i < hits.Length; i++)
            {
                collider = hits[i].collider;
                if (collider.GetComponent<MonsterManager>() && !monsterManagers.Contains(collider.GetComponent<MonsterManager>()))
                {
                    monsterManagers.Add(collider.GetComponent<MonsterManager>());
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
                if (collider.GetComponent<BatSticked>() || collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
                {
                    Destroy(collider.gameObject);
                }
                if (collider.name == "hit role collider")
                {
                    Destroy(collider.transform.parent.gameObject);
                }
                if (collider.GetComponent<Crystal>())
                {
                    collider.GetComponent<Crystal>().hited();
                }
            }
        }
    }
}