using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CrystalRecoveryLight : MonoBehaviour
    {
        HashSet<PlayerManager> players = new HashSet<PlayerManager>();
        float recoveryTimer, recoveryTimerStoper = 3;

        void Update()
        {
            recoveryTimer += Time.deltaTime;
            if (recoveryTimer < recoveryTimerStoper)
            {
                for(int i = 0; i < players.Count; i++)
                {
                    PlayerManager.HP += 10 * Time.deltaTime;
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                players.Add(collider.GetComponent<PlayerManager>());
            }
        }
        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                players.Remove(collider.GetComponent<PlayerManager>());
            }
        }
    }
}