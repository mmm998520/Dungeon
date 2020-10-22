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
            if (collider.gameObject.layer >= 8 && collider.transform != transform.parent)
            {
                if (collider.GetComponent<PlayerManager>())
                {
                    collider.GetComponent<PlayerManager>().ConfusionTimer = 0;
                    Collider2D[] monsterColliders = Physics2D.OverlapCircleAll(transform.position, 10, 1 << 9);
                    for(int i = 0; i < monsterColliders.Length; i++)
                    {
                        if (monsterColliders[i].GetComponent<MonsterManager>())
                        {
                            monsterColliders[i].GetComponent<MonsterManager>().TauntTarge = collider.transform;
                        }
                    }
                }
                Destroy(gameObject);
            }
        }
    }
}