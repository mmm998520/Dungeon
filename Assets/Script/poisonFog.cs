using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    /// <summary> 毒霧 </summary>
    public class poisonFog : MonoBehaviour
    {

        bool hited = false;
        static HashSet<PlayerManager> playerList = new HashSet<PlayerManager>();
        Vector2[] rayPos = new Vector2[4];

        void Start()
        {
            transform.parent = GameManager.triggers;
            for (int i = 0; i < 4; i++)
            {
                rayPos[i] = transform.GetChild(i).transform.position;
            }
        }

        void Update()
        {
            hited = false;
            int i, j, k;
            for(i = 0; i < 4; i++)
            {
                for(j = 0; j < i; j++)
                {
                    RaycastHit2D[] raycasts = Physics2D.RaycastAll(rayPos[i],rayPos[j]-rayPos[i],1);
                    for (k = 0; k < raycasts.Length; k++)
                    {
                        if (raycasts[k].collider.GetComponent<PlayerManager>())
                        {
                            playerList.Add(raycasts[k].collider.GetComponent<PlayerManager>());
                        }
                    }
                }
            }
        }

        void LateUpdate()
        {
            if (!hited)
            {
                for (int i = 0; i < playerList.Count; i++)
                {
                    PlayerManager.HP -= Time.deltaTime * 10;
                }
                playerList.Clear();
                hited = true;
            }
        }
    }
}

