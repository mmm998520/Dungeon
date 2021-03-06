﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Bubble : MonoBehaviour
    {
        public Vector2 v, a;
        void Start()
        {
            v = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * 0.5f;
            a = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.right * 0.5f;
        }

        void Update()
        {
            a = Quaternion.Euler(0, 0, Random.Range(-10, 10)) * a;
            v += a * Time.deltaTime;
            transform.Translate(v * Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if ((collider.gameObject.layer == 8 || collider.gameObject.layer == 12) && collider.transform != transform.parent)
            {
                if (collider.GetComponent<PlayerManager>() && !PlayerManager.immunity)
                {
                    collider.GetComponent<PlayerManager>().ConfusionTimer = 0;
                    if (collider.GetComponent<PlayerManager>().p1)
                    {
                        GameManager.P1BubbleTimes++;
                    }
                    else
                    {
                        GameManager.P2BubbleTimes++;
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}