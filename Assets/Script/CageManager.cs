using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class CageManager : MonsterManager
    {
        void Start()
        {
            target = MinDisPlayer();
        }

        void Update()
        {
            if (Vector3.Distance(transform.position * Vector2.one, target.position * Vector2.one) < 0.5f)
            {
                transform.position = target.position;
                target.GetComponent<PlayerManager>().speed = 0f;
            }
            if (HP <= 0)
            {
                target.GetComponent<PlayerManager>().speed = 3;
                Destroy(gameObject);
            }
        }
    }
}