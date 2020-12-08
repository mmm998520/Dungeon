using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.DungeonPad
{
    public class StabTrigger : MonoBehaviour
    {
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

            }
        }
    }
}