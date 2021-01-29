using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterManager : Navigate
    {
        public float MaxHP, HP, CD, CDTimer, CDMin, CDMax, hand, atkTime, speed;
        protected float energyDeficiencyTimer = 10;
        protected int prepare = 0;

        public AudioSource prepareSource, attackSource;
        public GameObject attack;
        /// <summary> 守備區域 </summary>
        protected HashSet<int> guardPos = new HashSet<int>();
        /// <summary> 追擊區域 </summary>
        protected HashSet<int> pursuePos = new HashSet<int>();
        public int[] guardPoint;
        public int nextGrardNum;

        //仇恨目標
        public Transform TauntTarget;
        public float difference, repTimes, repTimer;
        protected bool attacked = false;

        protected float ChasePlayerWaitTimer;

        public Rigidbody2D rigidbody;

        protected int StuckTimes;
        protected float StuckTimer;
        protected Vector2 StuckPos = Vector2.zero;

        public GameObject Ammunition;
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

        protected virtual void Attack(int additionalAngle)
        {
            attackSource.Play();
            float angle = Vector3.SignedAngle(Vector3.right, MinDisPlayer().position - transform.position, Vector3.forward);
            MonsterAttack monsterAttack = Instantiate(attack, transform.position, Quaternion.Euler(0, 0, angle + Random.Range(-difference, difference) + additionalAngle)).GetComponent<MonsterAttack>();
            //Destroy(monsterAttack.gameObject, atkTime);
        }

        public void useArmor(int cost)
        {
            HP -= cost;
        }

        protected void ArmorReCharge()
        {
            HP = MaxHP;
        }

        public void ReCD()
        {
            CDTimer = 0;
            CD = Random.Range(CDMin, CDMax);
        }

        protected void randomTarget()
        {
            if (endRow != null && endRow.Length > 0)
            {
            }
            int r = Random.Range(0, canGo.Count);
            endRow = new int[] { canGo[r] / MazeCreater.totalCol };
            endCol = new int[] { canGo[r] % MazeCreater.totalCol };
        }

        protected void randomMove()
        {
            if (roads.Count == 0)
            {
                randomTarget();
                findRoad();
            }
            else
            {
                Vector3 nextPos = new Vector3(roads[roads.Count - 1][0], roads[roads.Count - 1][1]);
                rigidbody.velocity = Vector3.Normalize(nextPos - transform.position) * speed;
                if (Vector3.Distance(transform.position, nextPos) < 0.3f)
                {
                    roads.RemoveAt(roads.Count - 1);
                }
            }
        }

        protected void Move()
        {
            if (TauntTarget == null)
            {
                randomMove();
            }
            else if ((ChasePlayerWaitTimer += Time.deltaTime) > 0.3f)
            {
                ChasePlayerWaitTimer = 0;
                findRoad();
                Vector3 nextPos = new Vector3(roads[roads.Count - 1][0], roads[roads.Count - 1][1]);
                rigidbody.velocity = Vector3.Normalize(nextPos - transform.position) * speed;
                if (Vector3.Distance(transform.position, nextPos) < 0.5f)
                {
                    roads.RemoveAt(roads.Count - 1);
                }
            }
            if (rigidbody.velocity.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (rigidbody.velocity.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        protected void Stuck(float wait, float dis)
        {
            if ((StuckTimer += Time.deltaTime) >= wait)
            {
                StuckTimer = 0;
                if (Vector2.Distance(StuckPos, transform.position) < dis)
                {
                    if (StuckTimes++ > 10)
                    {
                        Debug.LogError("Stuck Over 10");
                        Debug.LogErrorFormat(gameObject, "");
                    }
                    randomTarget();
                    findRoad();
                }
                else
                {
                    StuckTimes = 0;
                }
                StuckPos = transform.position;
            }
        }

        public virtual void beforeDied()
        {
            if (Random.Range(0, 3) < 1)
            {
                Instantiate(Ammunition, transform.position, Quaternion.identity);
            }
            PlayerManager.HP += PlayerManager.killHpRecover;
            Players.reTimer = 0;
            Destroy(gameObject);
            if (gameObject.name.Contains("Big"))
            {
                insMoney(Random.Range(7, 11));
                if (Random.Range(0, 100) < 15)
                {
                    Instantiate(GameManager.gameManager.reLifeParticle, transform.position, Quaternion.identity);
                }
            }
            else if (gameObject.name.Contains("Spider"))
            {
                insMoney(Random.Range(1, 2));
                if (Random.Range(0, 100) < 1)
                {
                    Instantiate(GameManager.gameManager.reLifeParticle, transform.position, Quaternion.identity);
                }
            }
            else if (gameObject.name.Contains("Slime"))
            {
                insMoney(Random.Range(1, 3));
                if (Random.Range(0, 100) < 1)
                {
                    Instantiate(GameManager.gameManager.reLifeParticle, transform.position, Quaternion.identity);
                }
            }
            else
            {
                Debug.LogError(gameObject.name + " : 未定掉落金錢");
                insMoney(Random.Range(1, 2));
                if (Random.Range(0, 100) < 1)
                {
                    Instantiate(GameManager.gameManager.reLifeParticle, transform.position, Quaternion.identity);
                }
            }
        }

        void insMoney(int times)
        {
            for (int i = 0; i < times; i++)
            {
                Instantiate(GameManager.gameManager.money, transform.position, Quaternion.identity);
            }
        }
    }
}