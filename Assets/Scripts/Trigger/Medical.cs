using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.BoardGameDungeon
{
    public class Medical : TriggerManager
    {
        public float recovery;

        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                collider.GetComponent<PlayerManager>().Hurt -= recovery;
                if (collider.GetComponent<PlayerManager>().Hurt < 0)
                {
                    collider.GetComponent<PlayerManager>().Hurt = 0;
                }
                Destroy(gameObject);
            }
        }
    }
}
