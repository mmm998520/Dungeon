﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterAttack : MonoBehaviour
    {
        public float ATK;
        public bool continued = false;
        public string MonsterType;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (!continued)
            {
                if (collider.gameObject.layer == 8)
                {
                    PlayerManager.HP -= ATK;
                    collider.GetComponent<PlayerJoyVibration>().hurt();
                    if(MonsterType == "Spider")
                    {
                        if (collider.GetComponent<PlayerManager>().p1)
                        {
                            GameManager.P1SpiderShooted++;
                        }
                        else
                        {
                            GameManager.P2SpiderShooted++;
                        }
                        GameManager.DiedBecause = "SpiderShoot";
                        GameManager.DiedBecauseTimer = 0;
                    }
                    /*if (collider.transform.childCount>2)
                    {
                        collider.transform.GetChild(2).gameObject.SetActive(true);
                    }*/
                }
            }
        }

        void OnTriggerStay2D(Collider2D collider)
        {
            if (continued)
            {
                if (collider.gameObject.layer == 8)
                {
                    PlayerManager.HP -= ATK * Time.deltaTime;
                    print(collider.transform.GetChild(2).name);
                    //collider.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
        }
    }
}