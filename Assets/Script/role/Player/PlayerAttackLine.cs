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

        [SerializeField] GameObject AttackCircle, playerTrack;
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
            monsters.Clear();
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

        protected override bool attack(Collider2D collider, float damage)
        {
            bool attack = false;
            if (collider.GetComponent<MonsterManager>())
            {
                MonsterManager monsterManager = collider.GetComponent<MonsterManager>();
                if (!monsters.Contains(monsterManager))
                {
                    monsters.Add(monsterManager);
                    if (!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                    {
                        monsterManager.HP -= damage;
                        try
                        {
                            monsterManager.HitedAnimator.SetTrigger("Hit");
                        }
                        catch
                        {
                            Debug.LogError("這種怪沒放到受傷特效 : " + collider.name, collider.gameObject);
                        }
                        attack = true;
                        if (Random.Range(0, 100) < PlayerManager.criticalRate)
                        {
                            monsterManager.HP -= 1;
                        }
                        if (PlayerManager.circleAttack)
                        {
                            Instantiate(AttackCircle, collider.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<PlayerCircleAttack>().monsters.Add(collider.GetComponent<MonsterManager>());
                        }
                        if (PlayerManager.poison)
                        {
                            monsterManager.poisonTimers.Add(0);
                            if (monsterManager.poisonTimers.Count > 10)
                            {
                                monsterManager.poisonTimers.RemoveAt(0);
                            }
                        }
                    }
                    print(collider.gameObject.name);
                    if (monsterManager.HP <= 0)
                    {
                        monsterManager.beforeDied();
                        Debug.LogWarning("hitTimes");
                    }
                }
            }
            if (collider.GetComponent<BatSticked>())
            {
                Destroy(collider.gameObject);
                if (PlayerManager.circleAttack)
                {
                    Instantiate(AttackCircle, collider.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<PlayerCircleAttack>().monsters.Add(collider.GetComponent<MonsterManager>());
                }
                if (PlayerManager.poison)
                {
                    collider.GetComponent<MonsterManager>().poisonTimers.Add(0);
                    if (collider.GetComponent<MonsterManager>().poisonTimers.Count > 10)
                    {
                        collider.GetComponent<MonsterManager>().poisonTimers.RemoveAt(0);
                    }
                }
                int r = Random.Range(1, 3);
                for (int i = 0; i < r; i++)
                {
                    Instantiate(GameManager.gameManager.money, transform.position, Quaternion.identity);
                }
                if (Random.Range(0, 100) < 1)
                {
                    Instantiate(GameManager.gameManager.reLifeParticle, transform.position, Quaternion.identity);
                }
            }
            if (collider.GetComponent<Bubble>() || (collider.GetComponent<MonsterShooter>() && collider.GetComponent<MonsterShooter>().canRemoveByPlayerAttack) || (collider.GetComponent<MonsterShooter_Bounce>() && collider.GetComponent<MonsterShooter_Bounce>().canRemoveByPlayerAttack))
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
            else if (collider.GetComponent<LifeSeller>())
            {
                if (!sellers.Contains(collider.transform))
                {
                    sellers.Add(collider.transform);
                    collider.GetComponent<LifeSeller>().buy();
                }
            }
            return attack;
        }
    }
}