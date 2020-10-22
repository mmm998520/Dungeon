using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    /// <summary> 毒霧 </summary>
    public class poisonFog : MonoBehaviour
    {
        List<PlayerManager> playerList = new List<PlayerManager>();
        void Update()
        {
            for(int i = 0; i < playerList.Count; i++)
            {
                PlayerManager.HP -= Time.deltaTime * 2;
            }
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                playerList.Add(collider.GetComponent<PlayerManager>());
            }
        }

        void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                playerList.Remove(collider.GetComponent<PlayerManager>());
            }
        }
    }
}

