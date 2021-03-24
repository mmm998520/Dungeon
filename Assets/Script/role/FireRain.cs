using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class FireRain : MonoBehaviour
    {
        public float timer, stopTimer, destoryTimer;
        public bool CanHit = false;
        public SpriteRenderer spriteRenderer;
        Vector3[] playerPos = new Vector3[2];
        void OnDestroy()
        {
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    FireRainInser.insPoses.Remove(((int)transform.position.x + i) * MazeCreater.totalCol + ((int)transform.position.y + j));
                }
            }
        }

        private void Update()
        {
            int i, j;

            for (i = 0; i < playerPos.Length; i++)
            {
                playerPos[i] = GameManager.players.GetChild(i).position;
            }

            for (i = 0; i < playerPos.Length; i++)
            {
                Debug.LogError("距離 : " + Vector3.Distance(transform.position, playerPos[i]));
                Debug.LogError("CanHit : " + CanHit);
                if (Vector3.Distance(transform.position, playerPos[i]) < transform.localScale.x && CanHit)
                {
                    PlayerManager playerManager = GameManager.players.GetChild(i).GetComponent<PlayerManager>();
                    if (playerManager.HardStraightTimer > 0.5f)
                    {
                        playerManager.HardStraightA = (Vector2)Vector3.Normalize(playerPos[i] - transform.position) * 10;
                        if (PlayerManager.HP <= PlayerManager.MaxHP * 0.3f)
                        {
                            PlayerManager.HP -= 20 * (100f - PlayerManager.reducesDamage) / 100f;
                        }
                        else
                        {
                            PlayerManager.HP -= 20;
                        }
                        try
                        {
                            Camera.main.GetComponent<Animator>().SetTrigger("Hit");
                        }
                        catch
                        {
                            Debug.LogError("這場景忘了放畫面抖動");
                        }
                        Instantiate(GameManager.Hurted, transform.position, Quaternion.identity, transform);
                        playerManager.HardStraightTimer = 0;
                        GetComponent<Animator>().SetTrigger("Boom");
                    }
                }
            }
        }
    }
}