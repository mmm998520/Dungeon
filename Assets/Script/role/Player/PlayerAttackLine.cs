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

        protected override bool attack(Collider2D collider, float damage)
        {
            bool attack = false;
            if (collider.GetComponent<MonsterManager>() && !monsterManagers.Contains(collider.GetComponent<MonsterManager>()))
            {
                monsterManagers.Add(collider.GetComponent<MonsterManager>());
                if (!(collider.GetComponent<TaurenBoss>() && collider.GetComponent<TaurenBoss>().InvincibleTimer < 0.4f))
                {
                    collider.GetComponent<MonsterManager>().HP -= damage;
                    attack = true;
                    /*
                    if (PlayerManager.trackBullet)
                    {
                        Transform minDisMonster = null;
                        float minDis = 5;//距離至少要5以下才會觸發攻擊
                        for (int i = 0; i < GameManager.monsters.childCount; i++)
                        {
                            Transform monster = GameManager.monsters.GetChild(i);
                            if (monster.gameObject.activeSelf)
                            {
                                if (Vector2.Distance(monster.position, transform.position) < minDis)
                                {
                                    float Dis = Vector2.Distance(monster.position, transform.position);
                                    minDisMonster = monster;
                                    minDis = 0;
                                }
                            }
                        }
                        if (minDisMonster != null)
                        {
                            for(int i = 0; i < GameManager.players.childCount; i++)
                            {
                                Instantiate(playerTrack, GameManager.players.GetChild(i).position, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<PlayerTrack>().Target = minDisMonster;
                            }
                        }
                    }
                    */
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
            return attack;
        }
    }
}