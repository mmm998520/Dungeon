using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Bat : MonsterManager
    {
        bool stoping = false;
        public Animator SpriteAnimator;
        Vector2 minDisPlayerPos;
        float findRoadTimer, findRoadTimerStoper = 1f;
        public GameObject stickBat;

        void Start()
        {
            speed += Random.Range(-0.5f, 0.5f);
            ReCD();
            startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
            startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
            arriveNewRoom(startRoomRow, startRoomCol);
        }

        void Update()
        {
            minDisPlayerPos = MinDisPlayer().position;
            float distance = Vector2.Distance(minDisPlayerPos, transform.position);
            if (distance < 2)
            {
                rigidbody.velocity = (minDisPlayerPos - (Vector2)transform.position).normalized * speed;
            }
            else if (distance < 10f)
            {
                findRoadTimer += Time.deltaTime;
                if (findRoadTimer > findRoadTimerStoper)
                {
                    findRoadTimer = 0;
                    endRow = new int[] { Mathf.RoundToInt(GameManager.players.GetChild(0).position.x), Mathf.RoundToInt(GameManager.players.GetChild(1).position.x) };
                    endCol = new int[] { Mathf.RoundToInt(GameManager.players.GetChild(0).position.y), Mathf.RoundToInt(GameManager.players.GetChild(1).position.y) };
                    findRoad();
                }
                if (roads.Count <= 0)
                {

                }
                else
                {
                    Vector3 nextPos = new Vector3(roads[roads.Count - 1][0], roads[roads.Count - 1][1]);
                    rigidbody.velocity = ((Vector2)(nextPos - transform.position)).normalized * speed;
                    if (Vector3.Distance(transform.position, nextPos) < 0.7f)
                    {
                        roads.RemoveAt(roads.Count - 1);
                    }
                }
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }
        }

        bool stick = false;//是否黏住玩家(影響beforeDied行為)
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.GetComponent<PlayerManager>())
            {
                Instantiate(stickBat, collision.collider.GetComponent<PlayerManager>().sticksBat).transform.localPosition = (transform.position - collision.transform.position).normalized * 0.9f;
                Destroy(gameObject);
            }
        }

        public override void beforeDied()
        {
            base.beforeDied();
            if (!stick)
            {
                GameManager.KillSpider++;
                Destroy(gameObject);
            }
        }
    }
}