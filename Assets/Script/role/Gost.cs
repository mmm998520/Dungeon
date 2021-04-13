﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class Gost : MonsterManager
    {
        public enum GostBehavior
        {
            ramdomMove = 0,
            attack = 1
        }
        public GostBehavior gostBehavior;

        [HideInInspector]public List<GameObject> Attacks = new List<GameObject>();
        bool attacking = false;
        public Animator SpriteAnimator;

        void Start()
        {
            ReCD();
            addCanGoByHand();
            if (SceneManager.GetActiveScene().name == "Game 1")
            {
                startRoomRow = Mathf.RoundToInt(transform.position.x) / GameManager.mazeCreater.objectCountRowNum;
                startRoomCol = Mathf.RoundToInt(transform.position.y) / GameManager.mazeCreater.objectCountColNum;
                arriveNewRoom(startRoomRow, startRoomCol);
                randomTarget();
            }
        }

        protected override void Update()
        {
            base.Update();
            if ((CDTimer += Time.deltaTime) >= CD && Vector2.Distance(MinDisPlayer().position, transform.position) <= 10)
            {
                attack();
            }
            else
            {
                if (SceneManager.GetActiveScene().name != "Tutorial1")
                {
                    Move();
                    Stuck(2, 1);
                }
                attacking = false;
                SpriteAnimator.SetBool("Attack", false);
            }
        }

        public new void attack()
        {
            if (!attacking)
            {
                attacking = true;
                GetComponent<Animator>().SetTrigger("Attack");
            }
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularDrag = 0;
            if (MinDisPlayer().transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            if (MinDisPlayer().transform.position.x < transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            SpriteAnimator.SetBool("Attack", true);
        }

        protected override void Attack(int additionalAngle)
        {
            attackSource.Play();
            string AtttackType;
            if (gameObject.name.Contains("Big"))
            {
                AtttackType = "FireRainsBigGost";
                for (int i = 0; i < 1; i++)
                {
                    Attacks.Add(Instantiate(base.attack, transform.position + Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector3.right * Random.Range(0.5f, 5f), Quaternion.identity, GameObject.Find(AtttackType).transform));
                }
            }
            else
            {
                AtttackType = "FireRainsGost";
                Vector3 minDisPlayerVector = Vector3.Normalize((Vector2)(MinDisPlayer().position - transform.position));
                float space = 2;
                for (int i = 0; i < 3; i++)
                {
                    Attacks.Add(Instantiate(base.attack, transform.position + minDisPlayerVector * (i + 1) * space, Quaternion.identity, GameObject.Find(AtttackType).transform));
                }
            }

        }

        public override void beforeDied()
        {
            base.beforeDied();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /*
            GameManager.KillGost++;
            GameManager.KillSpider++;
            */
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            for(int i = 0; i < Attacks.Count; i++)
            {
                Destroy(Attacks[i]);
            }

            SpriteAnimator.transform.parent = null;
            SpriteAnimator.SetTrigger("Destroy");
        }
    }
}