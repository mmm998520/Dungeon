using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class PlayersLine : MonoBehaviour
    {
        public static int playerChildCount;
        Vector3 p1, p2, center, dir;
        float dis, unitDis = 0.18f, angle;
        public GameObject preAttack, attack ,attackCollider;
        Transform lineAttacks;
        float[] lockedTimer;

        private void Awake()
        {
            playerChildCount = transform.childCount;
            lineAttacks = new GameObject("lineAttacks").transform;
            lockedTimer = new float[playerChildCount];
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
                if (transform.GetChild(i).GetComponent<PlayerManager>().locked)
                {
                    lockedTimer[i] += Time.deltaTime;
                }
                else
                {
                    lockedTimer[i] = 0;
                }
            }

            for (i = 0; i < playerChildCount; i++)
            {
                for (j = 0; j < i; j++)
                {
                    float P1timer = lockedTimer[i];
                    float P2timer = lockedTimer[j];
                    if (P1timer > 3f && P2timer > 3f)
                    {
                        lockedTimer[i] = 0;
                        lockedTimer[j] = 0;
                    }
                    else if (P1timer > 1f && P2timer > 1f)
                    {

                    }
                    else if (P1timer > 0.4f && P2timer > 0.4f)
                    {
                        k = 1;
                        p1 = transform.GetChild(i).position;
                        p2 = transform.GetChild(j).position;
                        center = (p1 + p2) / 2;
                        dis = Vector3.Distance(p1, p2);
                        dir = (p2 - p1).normalized * unitDis;
                        angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
                        Transform collider = Instantiate(attackCollider, center, Quaternion.Euler(0, 0, angle), lineAttacks).transform;
                        collider.localScale = new Vector3(dis, 0.1f, 1);
                        Instantiate(attack, center, Quaternion.Euler(0, 0, angle), lineAttacks);
                        while (unitDis * k <= dis / 2)
                        {
                            Instantiate(attack, center + (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            Instantiate(attack, center - (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            k++;
                        }
                    }
                    else if (P1timer > 0.3f && P2timer > 0.3f)
                    {

                    }
                    else if (P1timer > 0.2f && P2timer > 0.2f)
                    {
                        k = 1;
                        p1 = transform.GetChild(i).position;
                        p2 = transform.GetChild(j).position;
                        center = (p1 + p2) / 2;
                        dis = Vector3.Distance(p1, p2);
                        dir = (p2 - p1).normalized * unitDis;
                        angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
                        Instantiate(preAttack, center, Quaternion.Euler(0, 0, angle), lineAttacks);
                        while (unitDis * k <= dis / 2)
                        {
                            Instantiate(preAttack, center + (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            Instantiate(preAttack, center - (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            k++;
                        }
                    }
                    else if (P1timer > 0.1f && P2timer > 0.1f)
                    {

                    }
                    else if(P1timer > 0f && P2timer > 0f)
                    {
                        k = 1;
                        p1 = transform.GetChild(i).position;
                        p2 = transform.GetChild(j).position;
                        center = (p1 + p2) / 2;
                        dis = Vector3.Distance(p1, p2);
                        dir = (p2 - p1).normalized * unitDis;
                        angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
                        Instantiate(preAttack, center, Quaternion.Euler(0, 0, angle), lineAttacks);
                        while (unitDis * k <= dis / 2)
                        {
                            Instantiate(preAttack, center + (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            Instantiate(preAttack, center - (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                            k++;
                        }
                    }
                }
            }
        }
    }
}