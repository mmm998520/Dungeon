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
                


            }
        }

        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                Transform player = collider.transform;
                PlayerManager playerManager = player.GetComponent<PlayerManager>();
                float disX = Mathf.Abs(player.position.x - pos.x), disY = Mathf.Abs(player.position.y - pos.y);
                if (!playerManager.IntoHole)
                {
                    playerManager.IntoHole = true;
                }
                if ((disX < 0.15f && disY < 0.15f) && playerManager.DashTimer > 0.3f)
                {
                    player.position = playerManager.nextPosBeforeIntoHole;
                    Debug.LogError(playerManager.nextPosBeforeIntoHole);
                    PlayerManager.HP -= 1;
                }
            }
        }
    }
}