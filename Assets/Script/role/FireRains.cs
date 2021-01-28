using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FireRains : MonoBehaviour
    {
        public Vector2[] playerPos;
        public HashSet<PlayerManager> hited;
        public float rainAttackDistance;

        void Start()
        {
            playerPos = new Vector2[GameManager.players.childCount];
        }

        void LateUpdate()
        {
            hited = new HashSet<PlayerManager>();
            int i, j;
            Transform child;

            for (i = 0; i < playerPos.Length; i++)
            {
                playerPos[i] = GameManager.players.GetChild(i).position;
            }

            for(i = 0; i < playerPos.Length; i++)
            {
                for (j=0;j< transform.childCount; j++)
                {
                    child = transform.GetChild(j);
                    if (Vector3.Distance(child.position, playerPos[i]) < rainAttackDistance && child.GetComponent<FireRain>().CanHit)
                    {
                        hited.Add(GameManager.players.GetChild(i).GetComponent<PlayerManager>());
                        break;
                    }
                }
            }

            for (i = 0; i < hited.Count; i++)
            {
                PlayerManager.HP -= 20 * Time.deltaTime;
            }
            Debug.LogError(i);
        }
    }
}