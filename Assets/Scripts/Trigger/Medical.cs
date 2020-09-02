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
                collider.GetComponent<PlayerManager>().Hurt += recovery;
                Destroy(gameObject);
            }
        }
    }
}
