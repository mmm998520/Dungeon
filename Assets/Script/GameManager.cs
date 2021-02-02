using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager gameManager;
        public static Transform players, monsters, triggers, UI;
        public static int layers = 1, level = 1;
        public static MazeCreater mazeCreater;
        public static SmallMap smallMap;
        public static float Gammar = 1;

        public static float DiedBecauseTimer;
        public static string DiedBecause = "Distance";
        public static float PlayTime;
        public static int KillSpider, KillSlime;
        public static int P1SpiderShooted, P1SpiderHit, P1SlimeHit, P1BubbleTimes;
        public static int P2SpiderShooted, P2SpiderHit, P2SlimeHit, P2BubbleTimes;

        float emptyRoomNumCountTimer;

        public GameObject seller, reLifeParticle, money, moneyB;
        public bool haveFinalRoomStore;
        void Awake()
        {
            MazeCreater.setTotalRowCol();
            gameManager = this;
            players = GameObject.Find("Players").transform;
            monsters = GameObject.Find("Monsters").transform;
            triggers = GameObject.Find("Triggers").transform;
            if (GameObject.Find("MazeCreater"))
            {
                mazeCreater = GameObject.Find("MazeCreater").GetComponent<MazeCreater>();
            }
            if (GameObject.Find("SmallMap"))
            {
                smallMap = GameObject.Find("SmallMap").GetComponent<SmallMap>();
                smallMap.start();
                UI = smallMap.transform.parent;
            }
        }

        void Update()
        {
            //if (abilityStore)
            {
                if ((emptyRoomNumCountTimer += Time.deltaTime) >= 1)
                {
                    emptyRoomNumCountTimer = 0;
                    if((Sensor.WeAreInEmptyFinalRoom() && !haveFinalRoomStore))
                    {
                        haveFinalRoomStore = true;
                        Vector3 sellerPos;
                        for (int i = 0; i < 51; i++)
                        {
                            sellerPos = CameraManager.center + new Vector3(Random.Range(3, -3), Random.Range(3, -3), 10);
                            RaycastHit2D hit = Physics2D.BoxCast(sellerPos, Vector3.one, 0, Vector2.right, 0, 0 << 13);
                            if (!hit)
                            {
                                Instantiate(seller, sellerPos, Quaternion.identity);
                                break;
                            }
                            else
                            {
                                Debug.LogError("撞牆");
                            }
                            if (i >= 50)
                            {
                                Debug.LogError(sellerPos + hit.collider.name, hit.collider.gameObject);
                            }
                        }
                    }
                }
            }

            DiedBecauseTimer += Time.deltaTime;

            if (PlayerManager.HP <= 0)
            {
                if (GameObject.Find("lineAttacks"))
                {
                    Destroy(GameObject.Find("lineAttacks"));
                }
            }
        }
    }
}
