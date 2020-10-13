using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class GameManager : MonoBehaviour
    {
        public static Transform players, maze, monsters, triggers;
        public static int layers = 1, level = 1;
        public static MazeCreater mazeCreater;
        void Awake()
        {
            players = GameObject.Find("Players").transform;
            monsters = GameObject.Find("Monsters").transform;
            triggers = GameObject.Find("Triggers").transform;
            mazeCreater = GameObject.Find("MazeCreater").GetComponent<MazeCreater>();
        }
    }
}
