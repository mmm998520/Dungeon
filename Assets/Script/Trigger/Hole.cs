using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Hole : MonoBehaviour
    {
        public bool isHole;
        Vector3 pos;
        void Start()
        {
            pos = transform.position;
        }

        void Update()
        {
            for (int i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                float disX = Mathf.Abs(player.position.x - pos.x), disY = Mathf.Abs(player.position.y - pos.y);
                if(disX < 0.5f / 3f && disY < 0.5f / 3f && playerManager.DashTimer > 0.3f)
                {
                    if (isHole)
                    {
                        player.position = playerManager.nextHoleSide.position;
                        playerManager.v = Vector3.zero;
                        playerManager.HardStraightA = Vector3.zero;
                        playerManager.DashA = Vector3.zero;
                        playerManager.HardStraightTimer = -1;
                        PlayerManager.HP -= 10;
                        Debug.LogError("死");
                    }
                }
                if(disX < 0.5f / 3 * 2.5f && disY < 0.5f / 3 * 2.5f)
                {
                    if (!isHole)
                    {
                        playerManager.nextHoleSide = transform;
                        Debug.LogError("紀錄");
                    }
                }
            }
        }
    }
}