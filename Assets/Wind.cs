using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class Wind : MonoBehaviour
    {
        public float windSize;
        void Start()
        {

        }

        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.GetComponent<PlayerManager>())
            {
                collider.GetComponent<PlayerManager>().a += (Vector2)transform.right * windSize;
            }
        }
    }
}