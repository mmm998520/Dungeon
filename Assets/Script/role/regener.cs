﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class regener : MonoBehaviour
    {
        public Transform master;
        void Update()
        {
            transform.position = master.position * Vector2.one;
            Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 2, 1 << 8);
            for (int i = 0; i < collider2Ds.Length; i++)
            {
                PlayerManager.HP+=Time.deltaTime*3;
            }
        }
    }
}