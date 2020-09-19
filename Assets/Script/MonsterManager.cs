using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterManager : DirectionChanger
    {
        void Update()
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
            target = minDisPlayer;
            changeDirection();
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
}