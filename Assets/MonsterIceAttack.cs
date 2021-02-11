using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class MonsterIceAttack : MonsterAttack
    {
        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == 8)
            {
                collider.GetComponent<PlayerManager>().SleepTimer = -3;
            }
            base.OnTriggerEnter2D(collider);
        }

    }
}