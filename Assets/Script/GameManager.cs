using System.Collections;
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
        void Awake()
        {
            players = GameObject.Find("Players").transform;
            monsters = GameObject.Find("Monsters").transform;
            triggers = GameObject.Find("Triggers").transform;
            mazeCreater = GameObject.Find("MazeCreater").GetComponent<MazeCreater>();
            smallMap = GameObject.Find("SmallMap").GetComponent<SmallMap>();
            smallMap.start();
            UI = smallMap.transform.parent;
            UI.gameObject.SetActive(false);
        }

        void Update()
        {
            smallMap.update();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UI.gameObject.SetActive(!UI.gameObject.activeSelf);
            }
        }
    }
}
