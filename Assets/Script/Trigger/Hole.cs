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
                if(isHole && disX < 0.5f / 3f && disY < 0.5f / 3f && playerManager.DashTimer > 0.3f)
                {
                    if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("PlayerIdle"))
                    {
                        player.GetComponent<Animator>().SetTrigger("Fall");
                        //player.position = playerManager.nextHoleSide.position;
                        playerManager.v = Vector3.zero;
                        playerManager.HardStraightA = Vector3.zero;
                        playerManager.DashA = Vector3.zero;
                        playerManager.HardStraightTimer = -0.5f;
                        if (!PlayerManager.immunity)
                        {
                            if(PlayerManager.HP <= PlayerManager.MaxHP * 0.3f)
                            {
                                PlayerManager.HP -= 10 * (100f - PlayerManager.reducesDamage) / 100f;
                                player.GetComponent<PlayerJoyVibration>().hurt();
                            }
                            else
                            {
                                PlayerManager.HP -= 10;
                            }
                            Instantiate(GameManager.Hurted, player.position, Quaternion.identity, player);
                            Debug.LogError(gameObject.name, gameObject);
                        }
                    }
                }
                //if(disX < 0.5f / 3 * 2.5f && disY < 0.5f / 3 * 2.5f)
                if (!isHole && disX < 0.76875f && disY < 0.76875f)
                {
                    playerManager.nextHoleSide = transform;
                }
            }
        }
    }
}