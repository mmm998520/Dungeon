using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayerAttackLine : PlayerAttack
    {
        public Transform rayPos1, rayPos2;
        public bool hit = false;
        HashSet<Transform> sellers = new HashSet<Transform>();

        [SerializeField] GameObject AttackCircle;
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
            sellers.Clear();
        }

        void Hit()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayPos1.position, rayPos2.position - rayPos1.position, (rayPos2.position - rayPos1.position).magnitude);
            Collider2D collider;
            for (int i = 0; i < hits.Length; i++)
            {
                collider = hits[i].collider;
                attack(collider, 1);
            }
        }

        protected override void attack(Collider2D collider, float damage)
        {
            if (collider.GetComponent<MonsterManager>() && !monsterManagers.Contains(collider.GetComponent<MonsterManager>()))
            {
                monsterManagers.Add(collider.GetComponent<MonsterManager>());
                if (!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                {
                    collider.GetComponent<MonsterManager>().HP -= damage;
                    if (Random.Range(0, 100) < PlayerManager.criticalRate)
                    {
                        collider.GetComponent<MonsterManager>().HP -= 1;
                    }
                    if (PlayerManager.circleAttack)
                    {
                        Instantiate(AttackCircle, collider.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<PlayerCircleAttack>().monsterManagers.Add(collider.GetComponent<MonsterManager>());
                    }
                    if (PlayerManager.poison)
                    {
                        collider.GetComponent<MonsterManager>().poisonTimers.Add(0);
                        if (collider.GetComponent<MonsterManager>().poisonTimers.Count > 10)
                        {
                            collider.GetComponent<MonsterManager>().poisonTimers.RemoveAt(0);
                        }
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
            if (collider.name == "場外商人")
            {
                GameObject.Find("shop").transform.GetChild(0).gameObject.SetActive(true);
                GameObject.Find("shop").transform.GetChild(0).GetChild(0).GetComponent<AbilityDatas>().start();
            }
            else if (collider.GetComponent<LifeSeller>())
            {
                if (!sellers.Contains(collider.transform))
                {
                    sellers.Add(collider.transform);
                    collider.GetComponent<LifeSeller>().buy();
                }
            }
        }
    }
}