﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class GameManager : MonoBehaviour
    {
        public static Transform players,maze;

        void Awake()
        {
            players = GameObject.Find("Players").transform;
        }
    }
}
