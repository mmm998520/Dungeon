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
        public static AudioSource DiedAudioSource;
        public static bool canTrack;
        public float trackBulletTimer;
        [SerializeField] GameObject playerTrack;

        private void Awake()
        {
            playerChildCount = transform.childCount;
            lineAttacks = new GameObject("lineAttacks").transform;
            draw = true;
            DiedAudioSource = GetComponent<AudioSource>();
        }
        void Update()
        {
            fightingTimer += Time.deltaTime;
            reTimer += Time.deltaTime;
            trackBullet();
            /*
            if (GameManager.players.GetChild(0).GetComponent<PlayerManager>().DashTimer < 0.5f || GameManager.players.GetChild(1).GetComponent<PlayerManager>().DashTimer < 0.5f)
            {
                draw = true;
            }
            else
            {
                draw = false;
            }
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
            }*/
        }

        void trackBullet()
        {
            if (canTrack)
            {
                trackBulletTimer += Time.deltaTime;
            }

            if (PlayerManager.trackBullet)
            {
                if (trackBulletTimer > 1 && PlayerManager.HP > 30)
                {
                    trackBulletTimer = 0;
                    Transform minDisMonster = null;
                    float minDis = 5;//距離至少要5以下才會觸發攻擊
                    Transform player = GameManager.players.GetChild(Random.Range(0, GameManager.players.childCount));
                    for (int i = 0; i < GameManager.monsters.childCount; i++)
                    {
                        Transform monster = GameManager.monsters.GetChild(i);
                        if (monster.gameObject.activeSelf)
                        {
                            if (Vector2.Distance(monster.position, player.position) < minDis)
                            {
                                float Dis = Vector2.Distance(monster.position, player.position);
                                minDisMonster = monster;
                                minDis = Dis;
                            }
                        }
                    }
                    if (minDisMonster != null)
                    {
                        trackBulletTimer = 0;
                        Instantiate(playerTrack, player.position, Quaternion.Euler(0, 0, Random.Range(0, 360))).GetComponent<PlayerTrack>().Target = minDisMonster;
                    }
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