using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

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
        public static float fightingTimer = 10;
        public static Color32 spriteColor;
        public static AudioSource DiedAudioSource;
        public static bool canTrack;
        public float trackBulletTimer;
        [SerializeField] GameObject playerTrack;

        Transform[] players = new Transform[2];
        SpriteRenderer[] playerRenderers = new SpriteRenderer[2];
        Light2D[] playerColorLights = new Light2D[2], playerLights = new Light2D[4];
        int[] behind = new int[6], infront = new int[6];

        private void Awake()
        {
            playerChildCount = transform.childCount;
            lineAttacks = new GameObject("lineAttacks").transform;
            draw = true;
            DiedAudioSource = GetComponent<AudioSource>();

            int k = 0;
            for(int i = 0; i < 2; i++)
            {
                players[i] = transform.GetChild(i);
                playerRenderers[i] = players[i].Find("頭像").GetComponent<SpriteRenderer>();
                playerColorLights[i] = playerRenderers[i].transform.GetChild(0).GetComponent<Light2D>();
                playerLights[k++] = players[i].Find("GameObject").GetChild(0).GetComponent<Light2D>();
                playerLights[k++] = players[i].Find("GameObject (1)").GetChild(0).GetComponent<Light2D>();
            }
            
            behind[0] = infront[0] = SortingLayer.NameToID("floor");
            behind[1] = SortingLayer.NameToID("Behind");
            infront[1] = SortingLayer.NameToID("Infront");
            behind[2] = infront[2] = SortingLayer.NameToID("Default");
            behind[3] = infront[3] = SortingLayer.NameToID("Bubble");
            behind[4] = infront[4] = SortingLayer.NameToID("wall(front)");
            behind[5] = infront[5] = SortingLayer.NameToID("wall(top)");
        }
        void Update()
        {
            fightingTimer += Time.deltaTime;
            trackBullet();
            if(players[0].position.y > players[1].position.y)
            {
                playerRenderers[0].sortingLayerName = "Behind";
                playerColorLights[0].m_ApplyToSortingLayers[0] = SortingLayer.NameToID("Behind");
                playerLights[0].m_ApplyToSortingLayers = behind;
                playerLights[1].m_ApplyToSortingLayers = behind;

                playerRenderers[1].sortingLayerName = "Infront";
                playerColorLights[1].m_ApplyToSortingLayers[0] = SortingLayer.NameToID("Infront");
                playerLights[2].m_ApplyToSortingLayers = infront;
                playerLights[3].m_ApplyToSortingLayers = infront;
            }
            else
            {
                playerRenderers[0].sortingLayerName = "Infront";
                playerColorLights[0].m_ApplyToSortingLayers[0] = SortingLayer.NameToID("Infront");
                playerLights[0].m_ApplyToSortingLayers = infront;
                playerLights[1].m_ApplyToSortingLayers = infront;

                playerRenderers[1].sortingLayerName = "Behind";
                playerColorLights[1].m_ApplyToSortingLayers[0] = SortingLayer.NameToID("Behind");
                playerLights[2].m_ApplyToSortingLayers = behind;
                playerLights[3].m_ApplyToSortingLayers = behind;
            }
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