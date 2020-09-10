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
            foreach(PlayerManager player in players)
            {
                if((player.Hurt -= Time.deltaTime * 3) < 0)
                {
                    player.Hurt = 0;
                }
            }
        }
        private void OnTriggerStay2D(Collider2D collider)
        {
            if (collider.tag == "player")
            {
                if (!players.Contains(collider.GetComponent<PlayerManager>()))
                {
                    players.Add(collider.GetComponent<PlayerManager>());
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.tag == "player")
            {
                players.Remove(collider.GetComponent<PlayerManager>());
            }
        }

        private void OnDestroy()
        {
            transform.parent.GetComponent<PlayerManager>().exit = false;
        }
    }
}