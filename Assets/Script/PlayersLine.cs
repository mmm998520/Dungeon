using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayersLine : MonoBehaviour
    {
        public static int playerChildCount;
        Vector3 p1, p2, center, dir;
        float dis, unitDis = 1, angle;
        public GameObject attack;
        Transform lineAttacks;

        private void Awake()
        {
            playerChildCount = transform.childCount;
            lineAttacks = new GameObject("lineAttacks").transform;
        }
        void Update()
        {
            drawLine();
        }

        void drawLine()
        {
            Destroy(lineAttacks.gameObject);
            lineAttacks = new GameObject("lineAttacks").transform;

            int i, j, k;
            for (i = 0; i < playerChildCount; i++)
            {
                for (j = 0; j < i; j++)
                {
                    if (transform.GetChild(i).GetComponent<PlayerManager>().lockedTimer>0.3f && transform.GetChild(2).GetComponent<PlayerManager>().lockedTimer > 0.3f)
                    {
                        k = 1;
                        p1 = transform.GetChild(i).position;
                        p2 = transform.GetChild(j).position;
                        center = (p1 + p2) / 2;
                        dis = Vector3.Distance(p1, p2);
                        dir = (p2 - p1).normalized * unitDis;
                        angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
                        Instantiate(attack, center, Quaternion.Euler(0,0,angle), lineAttacks);
                        while (unitDis * k <= dis / 2)
                        {
                            Instantiate(attack, center + (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            Instantiate(attack, center - (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            k++;
                        }
                    }
                }
            }
        }
    }
}