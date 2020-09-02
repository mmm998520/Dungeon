using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Exp : TriggerManager
    {
        int exp = 5;
        public ValueSet.Career career;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                if(career == collider.GetComponent<PlayerManager>().career)
                {
                    collider.GetComponent<PlayerManager>().exp += exp;
                }
                Destroy(gameObject);
            }
        }
    }
}