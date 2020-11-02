using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterManager : Navigate
    {
        public float MaxArmor, Armor, ArmorConsumption, ATK, CD, CDTimer, CDMin, CDMax, preparation1, preparation2, preparationTimer, hand, atkTime, speed;
        protected float energyDeficiencyTimer = 10;
        protected int prepare = 0;
        protected Transform ArmorBar;

        bool firstUpdateInThisAttack = true;
        public AudioSource prepareSource, attackSource;
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
        
        /// <summary> 獲取最近玩家的左右方塊 </summary>
        public Transform MinDisPlayerCube()
        {
            int i, j;
            float minDis = float.MaxValue;
            Transform minDisPlayerCube = null;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                for (j = 0; j < 2; j++)
                {
                    Transform playerCube = GameManager.players.GetChild(i).GetChild(0).GetChild(j);
                    if (minDis > Vector3.Distance(playerCube.position, transform.position))
                    {
                        minDis = Vector3.Distance(playerCube.position, transform.position);
                        minDisPlayerCube = playerCube;
                    }
                }
            }
            return minDisPlayerCube;
        }


        /// <summary> 轉向並向下一個目標點前進 </summary>
        protected void moveToTarget()
        {
            if (prepare == 0)
            {
                if (nextPos != null && nextPos.Length > 1)
                {
                    Vector3 endPos = new Vector3(nextPos[0], nextPos[1], 0);
                    GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(endPos - transform.position) * speed;
                    if (GetComponent<Rigidbody2D>().velocity.x > 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    if (GetComponent<Rigidbody2D>().velocity.x < 0)
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                }
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
                        if (firstUpdateInThisAttack)
                        {
                            prepareSource.Play();
                            firstUpdateInThisAttack = false;
                        }
                    }
                }
                else
                {
                    preparationTimer = 0;
                    prepare = 0;
                    firstUpdateInThisAttack = true;
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
                firstUpdateInThisAttack = true;
            }
        }

        protected virtual void Attack()
        {
            attackSource.Play();
            float angle = Vector3.SignedAngle(Vector3.right, MinDisPlayer().position - transform.position, Vector3.forward);
            MonsterAttack monsterAttack = Instantiate(attack, transform.position, Quaternion.Euler(0, 0, angle + Random.Range(-difference, difference))).GetComponent<MonsterAttack>();
            Destroy(monsterAttack.gameObject, atkTime);
        }

        protected WaitForSeconds RepTime;

        protected IEnumerator AttackRep()
        {
            for (int i = 0; i < repTimes; i++)
            {
                attackSource.Play();
                float angle = Vector3.SignedAngle(Vector3.right, MinDisPlayer().position - transform.position, Vector3.forward);
                MonsterAttack monsterAttack = Instantiate(attack, transform.position, Quaternion.Euler(0, 0, angle + Random.Range(-difference, difference))).GetComponent<MonsterAttack>();
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
            firstUpdateInThisAttack = true;
            attacked = false;
            afterAttack();
        }

        protected virtual void afterAttack()
        {

        }

        public void useArmor(int cost)
        {
            Armor -= cost;
        }

        protected void ArmorReCharge()
        {
            Armor = MaxArmor;
        }
    }
}