using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Players : MonoBehaviour
    {
        public static int playerChildCount;
        Vector3 p1, p2, center, dir;
        float dis, unitDis = 0.18f, angle;
        public GameObject preAttack, attack ,attackCollider;
        Transform lineAttacks;
        public bool draw = false;
        public static float fightingTimer = 10, reTimer = 10;
        public static Color32 spriteColor;

        private void Awake()
        {
            playerChildCount = transform.childCount;
            lineAttacks = new GameObject("lineAttacks").transform;
            draw = true;
        }
        void Update()
        {
            fightingTimer += Time.deltaTime;
            reTimer += Time.deltaTime;
            if (draw)
            {
                drawLine();
            }
            else
            {
                if (lineAttacks != null)
                {
                    Destroy(lineAttacks.gameObject);
                }
            }
        }

        void drawLine()
        {
            if (lineAttacks != null)
            {
                Destroy(lineAttacks.gameObject);
            }
            lineAttacks = new GameObject("lineAttacks").transform;

            int i, j;

            for (i = 0; i < playerChildCount; i++)
            {
                for (j = 0; j < i; j++)
                {
                    Line(i, j);
                }
            }
        }

        void Line(int i, int j)
        {
            int k;
            k = 1;
            p1 = transform.GetChild(i).position;
            p2 = transform.GetChild(j).position;
            center = (p1 + p2) / 2;
            dis = Vector3.Distance(p1, p2);
            dir = (p2 - p1).normalized * unitDis;
            angle = Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
            spriteColor = new Color32((byte)(Mathf.Clamp(dis,0,5) * 44), 219, 0, (byte)(255 - Mathf.Clamp(dis, 0, 5) * 28));
            //Transform collider = Instantiate(attackCollider, center, Quaternion.Euler(0, 0, angle), lineAttacks).transform;
            attackCollider.transform.position = center;
            attackCollider.transform.rotation = Quaternion.Euler(0, 0, angle);
            attackCollider.transform.localScale = new Vector3(dis, 0.1f, 1);
            Instantiate(attack, center, Quaternion.Euler(0, 0, angle), lineAttacks);
            while (unitDis * k <= dis / 2)
            {
                Instantiate(attack, center + (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                Instantiate(attack, center - (dir * k), Quaternion.Euler(0, 0, angle), lineAttacks);
                k++;
            }
        }

        IEnumerator LineButtonWait()
        {
            draw = true;
            yield return new WaitForSeconds(5);
            draw = false;
        }
    }
}