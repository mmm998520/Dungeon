﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterShoter : MonoBehaviour
    {
        public float speed, timer, timerStoper, destoryTime;
        public WaitForSeconds destoryTimer;

        void Start()
        {
            destoryTimer = new WaitForSeconds(destoryTime);
        }
        void Update()
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed);
            if ((timer += Time.deltaTime) > timerStoper)
            {
                speed = 0;
                Destroy(gameObject, destoryTime);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8 || collider.gameObject.layer == 12)
            {
                speed = 0;
                Destroy(gameObject, destoryTime);
            }
        }
    }
}