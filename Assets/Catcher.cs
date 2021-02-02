using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Catcher : MonoBehaviour
    {
        public enum CatcherStat
        {
            idle,
            lockPlayer,
            attcak,
            back
        }
        public CatcherStat catcherStat;

        float statTimer;
        float speed = 2, rotateSpeed = 30;
        public Hand hand;

        void Update()
        {
            statTimer += Time.deltaTime;
            if (catcherStat == CatcherStat.idle)
            {
                if (statTimer > 3)
                {
                    selectTarget();
                    catcherStat = CatcherStat.lockPlayer;
                    statTimer = 0;
                }
            }
            else if (catcherStat == CatcherStat.lockPlayer)
            {
                locked();
                if (statTimer > 3)
                {
                    catcherStat = CatcherStat.attcak;
                    statTimer = 0;
                }
            }
            else if (catcherStat == CatcherStat.attcak)
            {
                hand.transform.parent.localScale += Vector3.right * Time.deltaTime * speed;
                if (statTimer > 3)
                {
                    catcherStat = CatcherStat.back;
                }
            }
            else if(catcherStat == CatcherStat.back)
            {
                hand.transform.parent.localScale += Vector3.right * Time.deltaTime * -speed;
                statTimer = 0;
            }
        }

        void selectTarget()
        {
            float angle = Vector3.SignedAngle(Vector2.right, (Vector2)(GameManager.players.GetChild(Random.Range(0, 2)).position - transform.position), Vector3.forward);
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        void locked()
        {
            float player0angle = Vector3.SignedAngle(transform.right, GameManager.players.GetChild(0).position - transform.position, Vector3.forward);
            float player1angle = Vector3.SignedAngle(transform.right, GameManager.players.GetChild(1).position - transform.position, Vector3.forward);
            float miner;
            if (Mathf.Abs(player0angle) < Mathf.Abs(player1angle))
            {
                miner = player0angle;
            }
            else
            {
                miner = player1angle;
            }

            if (miner > 0)
            {
                transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);
            }
            else if (miner < 0)
            {
                transform.Rotate(0, 0, Time.deltaTime * -rotateSpeed);
            }
        }

    }
}