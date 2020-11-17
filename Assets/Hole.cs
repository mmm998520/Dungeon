using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Hole : MonoBehaviour
    {
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
                if(disX < 0.5f && disY < 0.5f && playerManager.DashTimer > 0.4f)
                {
                    player.position = playerManager.nextPosBeforeIntoHole[0];
                    playerManager.v = Vector3.zero;
                    playerManager.HardStraightA = Vector3.zero;
                    playerManager.DashA = Vector3.zero;
                    playerManager.HardStraightTimer = -1;
                    PlayerManager.HP -= 30;
                }
                if (!playerManager.IntoHole && disX < 0.5f && disY < 0.5f)
                {
                    playerManager.IntoHole = true;
                }
            }
        }
        /*
        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                Transform player = collider.transform;
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                float disX = Mathf.Abs(player.position.x - pos.x), disY = Mathf.Abs(player.position.y - pos.y);
                if ((disX < 0.15f && disY < 0.15f) && playerManager.DashTimer > 0.3f)
                {
                    player.position = playerManager.nextPosBeforeIntoHole;
                    Debug.LogError(playerManager.nextPosBeforeIntoHole);
                    PlayerManager.HP -= 1;
                }
            }
        }*/
    }
}