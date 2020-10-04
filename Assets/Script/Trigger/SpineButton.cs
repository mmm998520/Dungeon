using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class SpineButton : Trigger
    {
        public Spine[] spines;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                triggerGen.rePos(transform);
                for (int i = 0; i < spines.Length; i++)
                {
                    spines[i].attack = true;
                    spines[i].attackTimer = 0;
                    spines[i].transform.GetChild(0).gameObject.SetActive(true);
                }
            }
        }
    }
}