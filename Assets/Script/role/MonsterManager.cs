using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterManager : Navigate
    {
        public float MaxArmor, Armor, ArmorConsumption, ATK, CD, CDTimer, preparation1, preparation2, preparationTimer, hand, atkTime, speed;
        protected float energyDeficiencyTimer = 10;
        protected int prepare = 0;
        protected Transform ArmorBar;
        public GameObject attack;
        /// <summary> 守備區域 </summary>
        protected HashSet<int> guardPos = new HashSet<int>();
        /// <summary> 追擊區域 </summary>
        protected HashSet<int> pursuePos = new HashSet<int>();
        public int[] guardPoint;
        public int nextGrardNum;

        //仇恨目標
        public Transform TauntTarge;
        public float difference, repTimes, repTimer;
        protected bool attacked = false;

        protected enum Stat
        {
            guard,
            pursue,
            back
        }
        protected Stat stat = Stat.guard;


        /// <summary> 獲取最近玩家 </summary>
        public Transform MinDisPlayer()
        {
            int i;
            float minDis = float.MaxValue;
            Transform minDisPlayer = null;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                if (minDis > Vector3.Distance(player.position, transform.position))
                {
                    minDis = Vector3.Distance(player.position, transform.position);
                    minDisPlayer = player;
                }
            }
            return minDisPlayer;
        }


        /// <summary> 轉向並向下一個目標點前進 </summary>
        protected void moveToTarget()
        {
            if (prepare != 2)
            {
                changeDirection();
            }
            if (prepare == 0)
            {
                GetComponent<Rigidbody2D>().velocity = transform.right * speed;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Rigidbody2D>().angularVelocity = 0;
            }
        }

        protected void attackCD()
        {
            if ((CDTimer += Time.deltaTime) >= CD)
            {
                if (Vector3.Distance(transform.position * Vector2.one, MinDisPlayer().position * Vector2.one) < hand)
                {
                    if (prepare == 0)
                    {
                        prepare = 1;
                    }
                }
                else
                {
                    preparationTimer = 0;
                    prepare = 0;
                }
            }
        }

        protected void prepareAttack()
        {
            if (stat != Stat.back)
            {
                if ((preparationTimer += Time.deltaTime) >= preparation1)
                {
                    prepare = 2;
                }
                if ((preparationTimer += Time.deltaTime) >= preparation1 + preparation2)
                {
                    if (!attacked)
                    {
                        Attack();
                        attacked = true;
                    }
                }
            }
            else
            {
                CDTimer = 0;
                preparationTimer = 0;
                prepare = 0;
            }
        }

        protected virtual void Attack()
        {
            StartCoroutine("AttackRep");
        }

        protected WaitForSeconds RepTime;

        protected IEnumerator AttackRep()
        {
            for (int i = 0; i < repTimes; i++)
            {
                MonsterAttack monsterAttack = Instantiate(attack, transform.position, transform.rotation * Quaternion.Euler(0, 0, Random.Range(-difference, difference))).GetComponent<MonsterAttack>();
                Destroy(monsterAttack.gameObject, atkTime);
                monsterAttack.ATK = ATK;
                CDTimer = 0;
                Armor -= ArmorConsumption;
                if (Armor <= 0)
                {
                    energyDeficiencyTimer = 0;
                    Armor = 0;
                }
                yield return RepTime;
            }
            nextGrardNum = Random.Range(0, guardPoint.Length);
            preparationTimer = 0;
            prepare = 0;
            attacked = false;
            afterAttack();
        }

        protected virtual void afterAttack()
        {

        }

        protected void ArmorReCharge()
        {
            Armor = MaxArmor;
        }
    }
}