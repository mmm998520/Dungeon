using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class TractorBeamGen : MonoBehaviour
    {
        public enum TractorBeamStat
        {
            non,
            pre,
            attack
        }
        public TractorBeamStat stat;
        float statTimer;
        public GameObject tractorBeam;
        float speed = 20;
        public Transform[] genPos;
        void Start()
        {

        }

        void Update()
        {
            if (stat == TractorBeamStat.non)
            {
                tractorBeam.SetActive(false);
                statTimer += Time.deltaTime;
                if (statTimer >= 5)
                {
                    statTimer = 0;
                    stat = TractorBeamStat.pre;
                    transform.position = genPos[Random.Range(0, genPos.Length)].position;
                    float angle = Vector3.SignedAngle(Vector2.right, (Vector2)(GameManager.players.GetChild(Random.Range(0, 2)).position - transform.position), Vector3.forward);
                    transform.eulerAngles = new Vector3(0, 0, angle);
                }
            }
            else if(stat == TractorBeamStat.pre)
            {
                TractorBeamStatPre();
                tractorBeam.SetActive(false);
                statTimer += Time.deltaTime;
                if (statTimer >= 2)
                {
                    statTimer = 0;
                    stat = TractorBeamStat.attack;
                    tractorBeam.GetComponent<TractorBeam>().canBack = false;
                }
            }
            else if(stat == TractorBeamStat.attack)
            {
                TractorBeamStatAttack();
            }
        }

        void TractorBeamStatPre()
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
                transform.Rotate(0, 0, Time.deltaTime * speed);
            }
            else if (miner < 0)
            {
                transform.Rotate(0, 0, Time.deltaTime * -speed);
            }
        }

        void TractorBeamStatAttack()
        {
            tractorBeam.SetActive(true);
        }
    }
}