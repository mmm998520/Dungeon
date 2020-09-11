using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Recover : MonoBehaviour
    {
        List<PlayerManager> players = new List<PlayerManager>();
        private void Start()
        {
            players.Add(transform.parent.GetComponent<PlayerManager>());
        }
        private void Update()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius, 1 << 0);
            foreach(Collider2D collider in colliders)
            {
                if (collider.GetComponent<PlayerManager>())
                {
                    PlayerManager player = collider.GetComponent<PlayerManager>();
                    if ((player.Hurt -= Time.deltaTime * 3) < 0)
                    {
                        player.Hurt = 0;
                    }
                }
            }
        }
    }
}