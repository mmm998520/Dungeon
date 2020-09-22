using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.DungeonPad
{
    public class Exit : MonoBehaviour
    {
        float time = 3, timer;
        public Transform MinDisPlayer()
        {
            int i;
            float minDis = float.MaxValue;
            Transform minDisPlayer = null;
            for (i = 0; i < GameManager.players.childCount; i++)
            {
                Transform player = GameManager.players.GetChild(i);
                if (minDis > Vector3.Distance(player.position, transform.position))
                {
                    minDis = Vector3.Distance(player.position, transform.position);
                    minDisPlayer = player;
                }
            }
            return minDisPlayer;
        }

        void Update()
        {
            Vector3 minDisPlayerDir = (transform.position * Vector2.one - MinDisPlayer().position * Vector2.one);
            transform.GetComponent<Rigidbody2D>().velocity = minDisPlayerDir.normalized * 4;
            if (minDisPlayerDir.sqrMagnitude < 0.5f)
            {
                if ((timer += Time.deltaTime) >= time)
                {
                    SceneManager.LoadScene("Win");
                }
            }
            else
            {
                timer = 0;
            }
        }
    }
}