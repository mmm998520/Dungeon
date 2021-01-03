﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class GameManager : MonoBehaviour
    {
        public static Transform players, maze, monsters, triggers, UI;
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

        public static AbilityStore abilityStore;
        float emptyRoomNumCountTimer;

        void Awake()
        {
            MazeCreater.setTotalRowCol();
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
            if (GameObject.Find("AbilityStore"))
            {
                abilityStore = GameObject.Find("AbilityStore").GetComponent<AbilityStore>();
                abilityStore.initialStore();
            }
        }

        void Update()
        {
            if (abilityStore)
            {
                if ((emptyRoomNumCountTimer += Time.deltaTime) >= 1)
                {
                    emptyRoomNumCountTimer = 0;
                    if (Sensor.emptyRoomNum() >= abilityStore.appearRoomNum)
                    {
                        abilityStore.showStore();
                        abilityStore.appearRoomNum = 9999;
                    }
                    /*if (Sensor.finalRoomIsEmpty())
                    {
                        Debug.LogError(2312312132132);
                        abilityStore.showStore();
                        abilityStore.appearRoomNum = 9999;
                    }*/
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
