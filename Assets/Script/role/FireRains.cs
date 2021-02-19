using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FireRains : MonoBehaviour
    {
        public Vector2[] playerPos;
        /// <summary>
        /// 紀錄各個玩家在岩漿中所待的時間，float[0]為計時用途，float[1]為計算傷害波數用途(每n秒觸發一次傷害，一次傷害為一波)，攻擊力隨波數提升
        /// </summary>
        public Dictionary<PlayerManager, float[]> hitedTimer = new Dictionary<PlayerManager, float[]>();
        [SerializeField] float hitedTimeSpan, SingleDamage;

        void Start()
        {
            playerPos = new Vector2[GameManager.players.childCount];
            for(int i = 0; i < GameManager.players.childCount; i++)
            {
                hitedTimer.Add(GameManager.players.GetChild(i).GetComponent<PlayerManager>(), new float[2] { 0, 0 });
            }
        }

        void LateUpdate()
        {
            int i, j;
            Transform child;

            for (i = 0; i < playerPos.Length; i++)
            {
                playerPos[i] = GameManager.players.GetChild(i).position;
            }

            for(i = 0; i < playerPos.Length; i++)
            {
                bool hited = false;
                PlayerManager playerManager = GameManager.players.GetChild(i).GetComponent<PlayerManager>();
                for (j=0;j< transform.childCount; j++)
                {
                    child = transform.GetChild(j);
                    if (Vector3.Distance(child.position, playerPos[i]) < child.localScale.x && child.GetComponent<FireRain>().CanHit)
                    {
                        hited = true;
                        break;
                    }
                }
                if (hited)
                {
                    hitedTimer[playerManager][0] += Time.deltaTime;
                    if (hitedTimer[playerManager][0] > hitedTimeSpan)
                    {
                        hitedTimer[playerManager][0] = 0;
                        PlayerManager.HP -= ++hitedTimer[playerManager][1] * SingleDamage;
                    }
                }
                else
                {
                    hitedTimer[playerManager][0] = 0;
                    hitedTimer[playerManager][1] = 0;
                }
            }

            Debug.LogError(i);
        }
    }
}